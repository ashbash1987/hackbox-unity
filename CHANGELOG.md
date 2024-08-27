# Changelog
All notable changes to this project will be documented in this file.
 
The format is based on [Keep a Changelog](http://keepachangelog.com/) and this project adheres to [Semantic Versioning](http://semver.org/).

## [v0.11.0] - 2024-08-27

### Added
- Add `ConnectExisting` to Host to allow connecting to an existing room on Hackbox with a given room code and host user ID.

## [v0.10.0] - 2024-08-12

### Added
- Add handling of `change` member events (they occur as the user makes changes to a component, debounced by some interval). Use `OnValueChange` or `ValueChangeEvents` to listen for these value change events.

### Changed
- Updated the parameter defaults for TextInput preset to allow specifying a `type` of `number` for constricting to numeric input only (and also allow `min`/`max`/`step` parameters).
- More exception handling code in place to more reliably catch unexpected behaviour should it happen.

## [v0.9.1] - 2024-04-13

### Added
- Add `Create` methods to Theme, Preset, UIComponent, and State to avoid having to use ScriptableObject.CreateInstance<T>() each time. (The `State.Create()` isn't strictly needed as it doesn't derive from ScriptableObject, but it's implemented to maintain a consistent creation method naming scheme.)
- Add `HeaderBackgroundColor` and `MainBackgroundColor` properties to Theme to make it easier to set the backgrounds to solid colors.
- Add `HeaderText`, `HeaderColor` and `HeaderBackground` properties to State to make it easier to initialise/set those parameters. `GetHeaderText()` and `SetHeaderText()` methods will remain to maintain backwards compatibility.

### Changed
- Compressed the event foldout in the Host inspector into a scrollview.

## [v0.9.0] - 2024-04-10

### Added
- Added `TryGetComponent` on State to access a component much in the same vein as `Dictionary<T>.TryGetValue`.
- Add the `persistent` parameter to TextInput presets & components (https://github.com/devanhurst/hackbox/pull/9).

### Changed
- Add slightly better room code validation and ownership verification in so it can error out more meaningfully before trying to connect to a non-existent or non-owned room.

### Fixed
- Tweak the regular expression used on the dimension parameter.

## [v0.8.0] - 2024-02-06

### Added
- Add new fields (`ConnectOnStart`, `ReconnectOnEnable`, `DisconnectOnEnable`) to control automatic connect/disconnect behaviour.

### Fixed
- Fix Disconnect event on JavaScript on WebGL builds.

## [v0.7.0] - 2023-11-09

### Added
- New tooltips added to help describe parameters.
- Supports new Choices grid display (`grid`, `gridColumns`, `gridGap`, `gridRowHeight`).
- Supports `minWidth`/`maxWidth` on main part of theme, and `minHeight`/`maxHeight` on header part.

### Changed
- Updated parameters in line with latest Hackbox changes.

### Fixed
- Fix for the dimension magic regex.
- Limit hover style parameters to `background` and `color` only as those are the only ones supported.

## [v0.6.0] - 2023-09-23

### Added
- Custom fonts are now supported! Setup the font names in the theme, and then use them as you wish in the preset and component objects!

## [v0.5.0] - 2023-09-23

### Changed
- Background 'string' drawer now extracted and made more use in other places in the package (namely the Theme main and header backgrounds).
- Helper methods in UIComponent and ParameterList are far more feature complete now. Can get/set parameter and style parameter values far more easily.
- Set the default version for new instances of the Host component to 2 as version 2 is far more feature rich and also fairly stable.
- ParameterList drawer is now foldable, meaning that the inspector doesn't get completely overwhelmed with all of the parameter properties drawing at all times.
- Parameter creation in code is far more friendly now, accepting parameterless constructors and typed constructors for the appropriate types the parameters box around.
- Can now construct states, parameter lists, etc. with object initializers and collection initializers, meaning that code-created elements can take up just a few lines of code now instead of lots of boilerplate-style code being required. The older construction methods are still 100% supported and backwards compatible alongside this new initializer interface.

## [v0.4.6] - 2023-08-12

### Changed
- Add implicit operator for simple StateAsset to State conversion.

### Fixed
- Fix performance issues on serializing/deserializing JSON.
- Add newer syntax to reduce warnings about deprecated/obsolete APIs on UnityWebRequest.
- Try and fix some style properties not being combined correctly.

## [v0.4.5] - 2023-07-09

### Fixed
- Fix the Host Editor script to allow the new Twitch Required feature to work!!

## [v0.4.3] - 2023-07-09

### Added
- Add `twitchRequired` to the room creation POST request, restricting room to only members that have Twitch credentials authenticated.

## [v0.4.2] - 2023-02-25

### Fixed
- Fix components not getting local style being applied in V2 hosted rooms.

## [v0.4.1] - 2023-02-16

### Changed
- Updated to reflect the new Member data structure proposed by Dev with regards to new Twitch metadata.

## [v0.4.0] - 2023-02-08

### Added
- The inspectors for Presets and Components will be more intelligent in the offerings of what parameters are available and where, according to the latest supported fields in Hackbox.
- The Range component is now supported since the last official version change, allowing integer based input, either typed in or as a horizontal slider.
- Hackbox now supports Twitch identity provisioning - the Unity side of things now parses this information and makes it available in each Member if they have logged in with Twitch.

### Changed
- You can now control the level of debugging from Minimal, to Full, or to Off if you don't want any debugging at all (thanks Royal_Flu$h for the suggestion!). There's also a crude JSON viewer to help debug Member states and how they are getting constructed in the Hackbox Unity system. Oh and, if you're doing really funky debugging stuff, you can also point to a local development URL right in the Host inspector now, rather than having to spelunk into the code to do that!

## [v0.3.0] - 2023-01-14

### Added
- Added WebGL implementation for socketio via jslib.

### Changed
- Slight update to Host component Editor appearance.

### Fixed
- JSON construction/deconstruction rewritten in places to support non-reflection platforms like WebGL.
- Refactored SocketIO code to now have platform implementations for Standalone, Editor, and WebGL.

## [v0.2.4] - 2022-12-27

### Added
- Added debugging option to log some extra information out, such as ping/pong event calls and raw JSON that is sent/received.
- Added `OnRoomReconnecting`, `OnRoomReconnectFailed`, and `OnPingPong` events.

### Changed
- Now depends on updated socket.io-client-csharp v3.0.6 (newtonsoft-json branch).

### Fixed
- Updated version now correctly makes disconnect and reconnect calls.
- Updated reconnect logic to send latest updated member states on reconnecting.
- Updated logging.

## [v0.2.3] - 2022-12-18

### Fixed
- Fixed style parameters not being correctly cloned in state objects, which lead to UIComponents not inheriting their styling.

## [v0.2.2] - 2022-12-09

### Added
- Added `key` member to UIComponent, so that individual component state can be persisted across a full state change. Will only populate the "key" value in the JSON if the value is not empty and not null.

## [v0.2.1] - 2022-09-26

### Changed
- Style parameters have been migrated to a new fields in the serialized data. Checking through your StateAsset objects should be enough to migrate the appropriate parameters into the style parameters field.
- `hover` has been moved to a style parameter.
- In version 2+ hosting, each individual Choice can now be styled with the new style field per choice.

### Fixed
- Fixed state payload definition so that it now sends a correctly versioned payload. (Reminder: Change the version field on the Host component to change what version state payload is sent.)
- `hover`, `submit`, and other ParameterList parameter inspectors have been fixed and should now be visible.

## [v0.2.0] - 2022-09-26

### Added
- Initial update to attempt version 2 state payloads. Change the version field on the Host component to change what version state payload is sent.

### Changed
- Style parameters have been migrated to a new fields in the serialized data. Checking through your StateAsset objects should be enough to migrate the appropriate parameters into the style parameters field.

### Fixed
- Fix a bug in GetMemberByID which compared against the gameobject name instead of the userID parameter.

## [v0.1.4] - 2022-09-26

### Added
- Added `padding` and `margin` styling parameters.

### Fixed
- Fixed assembly definitions so that the package should be more "plug & play" and should no longer complain about Unity Editor dependencies for runtime builds.

## [v0.1.3] - 2022-09-26

### Added
- First release with fixes to Unity Editor inspector problems.
