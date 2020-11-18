# TwitchFX

*Beat Saber plugin to add lighting and color effects to be triggered by twitch commands*

Commissioned by and developed in collaboration with [xariao].

[xariao]: https://www.twitch.tv/xariao

## Installation

1. Install the dependencies using [ModAssistant].
1. Download the [latest release] of TwitchFX.
1. Extract the contents of the zip into your Beat Saber directory.

[ModAssistant]: https://github.com/Assistant/ModAssistant
[latest release]: https://github.com/rakso20000/TwitchFX/releases/latest

### Dependencies

* [ChatCore]

[ChatCore]: https://github.com/brian91292/ChatCore

## Feedback

If you encounter a bug, have a feature request, or need assistance with setup feel free to [open an issue] or message me on Discord (rakso2#4449).

[open an issue]: https://github.com/rakso20000/TwitchFX/issues

## License

For the respective licenses of parts of this software, see [LICENSE].

[LICENSE]: https://github.com/rakso20000/TwitchFX/blob/master/LICENSE

## Commands

* `!setlightcolor <color>` Changes all lights to the specified color.  
  `!setlightcolor <left color> <right color>`  Changes all lights to the specified colors. The left color replaces the lights that would be red by default and the right color replaces the lights that would be blue by default.  
  `!setlightcolor <left color> <right color> <duration>` Changes all lights to the specified colors for the specified duration. The duration is given in seconds and doesn't have to be an integer.
* `!disablelights` Disables all lights and note spawn effects.  
  `!disablelights <duration>` Disables all lights and note spawn effects for the specified duration.
* `!restorelights` Restores default light behavior.
* `!boostlights <duration>` Makes all lights brighter for the specified duration. By default the lights have an alpha value of ~0.55 and flashing lights have an alpha value of ~0.75. This increases the alpha value to 1.
* `!setnotecolor <left color> <right color>` Changes notes and note spawn effects to the specified colors.  
  `!setnotecolor <left color> <right color> <duration>` Changes notes and note spawn effects to the specified colors for the specified duration.
* `!resetnotecolor` Resets notes and note spawn effects to their default colors.
* `!setsabercolor <left color> <right color>` Changes saber colors to the specified colors. This works with custom sabers.  
  `!setsabercolor <left color> <right color> <duration>` Changes sabers to the specified colors for the specified duration.
* `!resetsabercolor` Resets sabers to their default colors.
* `!setsabernotecolor <left color> <right color>` Changes notes, note spawn effects, and sabers to the specified colors.  
  `!setsabernotecolor <left color> <right color> <duration>` Changes notes, note spawn effects, and sabers to the specified colors for the specified duration.
* `!setwallcolor <color>` Changes walls to the specified color.  
  `!setwallcolor <color> <duration>` Changes walls to the specified color for the specified duration.
* `!resetwallcolor` Resets walls to their default color.
* `!spinrings` Makes the track lane rings spin.
* `!activatepreset <name>` Activates the specified [color preset] to set light colors, note colors, saber colors, and wall color simultaneously.  
  `!activatepreset <name> <duration>` Activates the specified [color preset] for the specified duration.
* `!lightshow <name>` Plays the specified [custom lightshow] effect.

[custom lightshow]: https://github.com/rakso20000/TwitchFX#custom-lightshows

### Arguments

#### duration

The duration is given in seconds and does not have to be an integer.

#### color

Colors are given either as a [hex triplet] with a leading `#` or as one of the following presets: `red`, `green`, `blue`, `yellow`, `cyan`, `magenta`, `white`, `gray`, `grey`, `black`

You can also pass `rainbow` instead of a color to get rainbow colors. The rainbow colors of left and right light, saber, and note colors are always on the opposite side of the color spectrum so you can easily differentiate between them. The rainbow colors for lights, sabers, and notes are synchronized so you can see which saber to use for a rainbow note by its.

[hex triplet]: https://en.wikipedia.org/wiki/Web_colors#Hex_triplet

## Configuration

The config file is located at `Beat Saber\UserData\TwitchFX.json` and follows the [JSON] format.

### Command names

In the `commands` object you can change the names of commands.

It follows this format: `"originalcommandname": "yourchoice"`

### Permissions

In the `commandsRequiredPermissions` object you can set the permissions level required to execute each command.

It follows this format: `"originalcommandname": "permissionslevelname"`

The available permissions levels are as follows, in descending order: `broadcaster`, `moderator`, `vip`, `subscriber`, `everyone`

Higher permissions levels can execute commands assigned to lower permissions levels (e.g. a VIP will be able to execute commands assigned to subscribers).

I, as the developer of TwitchFX, have added an override that lets me execute all commands. If you dislike this you can disable the `allowRaksoPermissionsOverride` option.

### Sample config

```json
{
	"commands": {
		"activatepreset": "preset",
		"boostlights": "boost",
		"disablelights": "disable",
		"lightshow": "lightshow",
		"resetnotecolor": "rnotes",
		"resetsabercolor": "rsabers",
		"resetwallcolor": "rwalls",
		"setlightcolor": "lights",
		"setnotecolor": "notes",
		"setsabercolor": "sabers",
		"setwallcolor": "walls",
		"setsabernotecolor": "sabernotes",
		"spinrings": "spin",
		"restorelights": "restore"
	},
	"commandsRequiredPermissions": {
		"activatepreset": "subscriber",
		"boostlights": "everyone",
		"disablelights": "subscriber",
		"lightshow": "everyone",
		"resetnotecolor": "broadcaster",
		"resetsabercolor": "moderator",
		"resetwallcolor": "vip",
		"setlightcolor": "vip",
		"setnotecolor": "broadcaster",
		"setsabercolor": "moderator",
		"setwallcolor": "vip",
		"setsabernotecolor": "everyone",
		"spinrings": "everyone",
		"restorelights": "subscriber"
	},
	"allowRaksoPermissionsOverride": true
}
```

## Color presets

Color presets are a shorthand for changing all colors simultaneously using the `!activatepreset` command.  
Besides being shorter and easier to use than calling the respective commands individually, they have the added benefit that all colors will actually be changed simultaneously without any delay that might be introduced from trying to send multiple twitch chat messages simultaneously.

The presets are located in the `Beat Saber\UserData\TwitchFX\ColorPresets` folder and follow the [JSON] format.

Each preset has to define a valid [color] for each of the following: `leftLightColor`, `rightLightColor`, `leftNoteColor`, `rightNoteColor`, `leftSaberColor`, `rightSaberColor`, `wallColor`

The presets must have the `.json` extension to be read and can be referred to using the `!activatepreset` command by their file names.

**Note:** If your preset doesn't work, see if the error is printed in the TwitchFX log at `Beat Saber\Logs\TwitchFX\_latest.log`.

[color]: https://github.com/rakso20000/TwitchFX#color

### Sample color preset

This preset is also included in the installation under the name `#1f1e33`.

```json
{
	"leftLightColor": "#1f1e33",
	"rightLightColor": "#1f1e33",
	"leftNoteColor": "#1f1e33",
	"rightNoteColor": "gray",
	"leftSaberColor": "#1f1e33",
	"rightSaberColor": "gray",
	"wallColor": "gray"
}
```

## Custom lightshows

Custom lightshows are quick series of lighting effects that override the default lighting for their duration. They are activated using the `!lightshow` command and can for example be used to add an effect for raids or new subscriptions.

The lightshows are located in the `Beat Saber\UserData\TwitchFX\Lightshows` folder and follow the [JSON] format.

Each lightshow has to include an `_events` [JSON] array with an object for each lighting event. These objects must contain a number `_time`, an integer `_type`, and an integer `_value`. This is the same format that Beat Saber itself uses for custom maps so you can create custom lightshows using any Beat Saber map editor.

Additionally, lightshows can have a `colorPreset` tag which can either be a string referring to a [color preset] or a [JSON] object following the format of a [color preset]. This preset will be active for the duration of the lightshow.

The lightshows must have the `.json` extension to read and can be referred to using the `!lightshow` command by their file names.

**Note:** If your lightshow doesn't work, see if the error is printed in the TwitchFX log at `Beat Saber\Logs\TwitchFX\_latest.log`.

### Creating custom lightshows

To create custom lightshows it is recommended to use a Beat Saber map editor.  
If you use a Beat Saber map editor to create your lightshow, the file will contain extra information that is not necessary for lightshows. You can either manually remove it or just leave it.

1. Create a new map in your editor of choice.
1. You may have to add a song file to be able to play the lightshow. Just add any sound file of sufficient length.
1. Add lighting events. The lightshow will end after the last event so if you want the lightshow to fade out, add another event at the end of the fade.
1. Save the map.
1. Open the map folder and look for the file for the difficulty you selected. (`Easy.dat`, `Normal.dat`, `Hard.dat`, `Expert.dat`, or `ExpertPlus.dat`)
1. Move or copy the file to `Beat Saber\UserData\TwitchFX\Lightshows`.
1. Rename the file to whatever you want the lightshow to be called and change the extension to `.json`.

### Sample custom lightshow

The following is a shortened version of a custom lightshow which is also included in the installation under the name `drumroll`.  
The full version is available [here][full lightshow].

```json
{
	"colorPreset": {
		"leftLightColor": "cyan",
		"rightLightColor": "yellow",
		"leftNoteColor": "gray",
		"rightNoteColor": "yellow",
		"leftSaberColor": "gray",
		"rightSaberColor": "yellow",
		"wallColor": "green"
	},
	"_events": [
		{
			"_time": 0,
			"_type": 2,
			"_value": 6
		},
		{
			"_time": 0.125,
			"_type": 3,
			"_value": 6
		},
		{
			"_time": 0.25,
			"_type": 2,
			"_value": 2
		},
		{
			"_time": 0.375,
			"_type": 3,
			"_value": 2
		}
	]
}
```

[full lightshow]: https://github.com/rakso20000/TwitchFX/blob/master/Lightshows/drumroll.json

[color preset]: https://github.com/rakso20000/TwitchFX#color-presets
[JSON]: https://www.json.org/json-en.html