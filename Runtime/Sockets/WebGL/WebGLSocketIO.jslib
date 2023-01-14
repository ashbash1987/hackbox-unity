var LibraryWebSocket = 
{
	$wss:
	{
		socket: null
	},

	WebSocketInit: function(uri, protocol, query, onConnect, onError, onDisconnect, onReconnectAttempt, onReconnect, onReconnectFail, onPing, onPong)
	{
		var queryObject = JSON.parse(Pointer_stringify(query));
		var socket = io(Pointer_stringify(uri), { query: queryObject });
		socket.protocol = protocol;

		socket.on('connect', function() { Module.dynCall_v(onConnect); });
		socket.on('disconnect', function() { Module.dynCall_v(onDisconnect); });
		socket.on('connect_error', function(error)
		{ 
			var bufferLength = lengthBytesUTF8(error) + 1;
			var buffer = _malloc(bufferLength);
			stringToUTF8(error, buffer, bufferLength);
			Module.dynCall_vi(onError, buffer);
		});

		socket.io.on('error', function(error)
		{ 
			var bufferLength = lengthBytesUTF8(error) + 1;
			var buffer = _malloc(bufferLength);
			stringToUTF8(error, buffer, bufferLength);
			Module.dynCall_vi(onError, buffer);
		});

		socket.io.on('ping', function() { Module.dynCall_v(onPing); });
		socket.io.on('pong', function() { Module.dynCall_v(onPong); });

		socket.io.on('reconnect_attempt', function(attempt) { Module.dynCall_vi(onReconnectAttempt, attempt); });
		socket.io.on('reconnect', function(attempt) { Module.dynCall_vi(onReconnect, attempt); });
		socket.io.on('reconnect_error', function(error) { Module.dynCall_v(onReconnectFail); });

		wss.socket = socket;
	},

	WebSocketConnect: function()
	{
		wss.socket.connect();
	},

	WebSocketDisconnect: function()
	{
		wss.socket.disconnect();
	},

	WebSocketEmit: function(eventName, message)
	{
		wss.socket.emit(Pointer_stringify(eventName), JSON.parse(Pointer_stringify(message)));
	},

	WebSocketOn: function(eventName, handleIndex, onMessage)
	{
		wss.socket.on(Pointer_stringify(eventName), function(data)
		{
			var dataString = JSON.stringify(data);
			var bufferSize = lengthBytesUTF8(dataString) + 1;
			var buffer = _malloc(bufferSize);
			stringToUTF8(dataString, buffer, bufferSize);
			Module.dynCall_vii(onMessage, handleIndex, buffer);
		});
	},

	WebSocketOff: function(eventName)
	{
		wss.socket.off(Pointer_stringify(eventName));
	},

	WebSocketConnected: function()
	{
		return wss.socket.connected;
	},

	WebSocketDisconnected: function()
	{
		return wss.socket.disconnected;
	},
};

autoAddDeps(LibraryWebSocket, '$wss');
mergeInto(LibraryManager.library, LibraryWebSocket);
