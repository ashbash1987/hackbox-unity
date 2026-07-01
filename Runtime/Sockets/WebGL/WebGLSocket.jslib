// Browser-WebSocket transport for the hackbox relay (WebGL builds).
//
// Replaces the old socket.io WebGL bridge. The relay speaks raw WebSocket with a
// { type, payload } JSON envelope; this owns the connection, the keepalive ping,
// and the reconnect backoff (WebGL has no background threads, so the timers live
// here in JS). Every non-"pong" frame is forwarded verbatim to the managed side,
// which parses the envelope and dispatches it. Lifecycle is surfaced through the
// callbacks passed to WebSocketInit, matching the StandaloneSocket events.
var LibraryHackboxSocket =
{
	$hbws:
	{
		socket: null,
		url: null,
		pingTimer: null,
		attempt: 0,
		connected: false,
		manualClose: false,
		callbacks: null,

		// Keepalive interval; must match the relay's setWebSocketAutoResponse.
		PING_INTERVAL: 25000,
		// Reconnect backoff (mirrors the player client's partysocket settings).
		MIN_RECONNECT_DELAY: 250,
		MAX_RECONNECT_DELAY: 10000,
		RECONNECT_GROW_FACTOR: 2,
		// Close codes >= 4000 are deliberate relay rejections: do not reconnect.
		FATAL_CLOSE_THRESHOLD: 4000,

		clearPing: function()
		{
			if (hbws.pingTimer !== null)
			{
				clearInterval(hbws.pingTimer);
				hbws.pingTimer = null;
			}
		},

		backoff: function(attempt)
		{
			var ms = hbws.MIN_RECONNECT_DELAY * Math.pow(hbws.RECONNECT_GROW_FACTOR, attempt - 1);
			return Math.min(ms, hbws.MAX_RECONNECT_DELAY);
		},

		sendString: function(callback, value)
		{
			var length = lengthBytesUTF8(value) + 1;
			var buffer = _malloc(length);
			stringToUTF8(value, buffer, length);
			Module.dynCall_vi(callback, buffer);
			_free(buffer);
		},

		open: function(isReconnect)
		{
			var socket = new WebSocket(hbws.url);
			hbws.socket = socket;

			socket.onopen = function()
			{
				hbws.connected = true;

				hbws.clearPing();
				hbws.pingTimer = setInterval(function()
				{
					if (hbws.socket && hbws.socket.readyState === 1)
					{
						Module.dynCall_v(hbws.callbacks.onPing);
						hbws.socket.send('ping');
					}
				}, hbws.PING_INTERVAL);

				if (isReconnect)
				{
					Module.dynCall_vi(hbws.callbacks.onReconnect, hbws.attempt);
				}
				else
				{
					Module.dynCall_v(hbws.callbacks.onConnect);
				}
				hbws.attempt = 0;
			};

			socket.onmessage = function(event)
			{
				if (typeof event.data !== 'string') return;
				// The relay's auto-response to our keepalive; nothing to dispatch.
				if (event.data === 'pong')
				{
					Module.dynCall_v(hbws.callbacks.onPong);
					return;
				}
				hbws.sendString(hbws.callbacks.onMessage, event.data);
			};

			// connect/transport errors surface through onclose, which drives reconnect.
			socket.onerror = function() {};

			socket.onclose = function(event)
			{
				hbws.connected = false;
				hbws.clearPing();

				if (hbws.manualClose) return;

				if (event.code >= hbws.FATAL_CLOSE_THRESHOLD)
				{
					// Terminal rejection: surface the reason and stop (no reconnect).
					var reason = event.reason || 'io server disconnect';
					hbws.sendString(hbws.callbacks.onError, reason);
					hbws.sendString(hbws.callbacks.onDisconnect, reason);
					return;
				}

				// Transient drop: reconnect with growing backoff.
				hbws.attempt += 1;
				Module.dynCall_vi(hbws.callbacks.onReconnectAttempt, hbws.attempt);
				setTimeout(function() { hbws.open(true); }, hbws.backoff(hbws.attempt));
			};
		}
	},

	WebSocketInit: function(url, onConnect, onError, onDisconnect, onReconnectAttempt, onReconnect, onPing, onPong, onMessage)
	{
		hbws.url = Pointer_stringify(url);
		hbws.manualClose = false;
		hbws.attempt = 0;
		hbws.callbacks =
		{
			onConnect: onConnect,
			onError: onError,
			onDisconnect: onDisconnect,
			onReconnectAttempt: onReconnectAttempt,
			onReconnect: onReconnect,
			onPing: onPing,
			onPong: onPong,
			onMessage: onMessage
		};
	},

	WebSocketConnect: function()
	{
		hbws.manualClose = false;
		hbws.attempt = 0;
		hbws.open(false);
	},

	WebSocketDisconnect: function()
	{
		hbws.manualClose = true;
		hbws.clearPing();
		if (hbws.socket)
		{
			hbws.socket.close(1000, 'client disconnect');
			hbws.socket = null;
		}
		hbws.connected = false;
	},

	WebSocketSend: function(message)
	{
		if (hbws.socket && hbws.socket.readyState === 1)
		{
			hbws.socket.send(Pointer_stringify(message));
		}
	},

	WebSocketConnected: function()
	{
		return hbws.connected;
	},

	WebSocketDisconnected: function()
	{
		return !hbws.connected;
	}
};

autoAddDeps(LibraryHackboxSocket, '$hbws');
mergeInto(LibraryManager.library, LibraryHackboxSocket);
