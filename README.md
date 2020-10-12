# TwitchFX

*Beat Saber plugin to add lighting and color effects to be triggered by twitch commands*

## Installation

1. Install the dependencies using [ModAssistant].
2. Download the [latest release] of TwitchFX.
3. Extract the contents of the zip into your Beat Saber directory.

[ModAssistant]: https://github.com/Assistant/ModAssistant
[latest release]: https://github.com/rakso20000/TwitchFX/releases/latest

### Dependencies

* [ChatCore]

[ChatCore]: https://github.com/brian91292/ChatCore

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
* `!activatepreset <name>` Activates the specified [color preset] to set light colors, note colors, saber colors, and wall color simultaneously.  
  `!activatepreset <name> <duration>` Activates the specified [color preset] for the specified duration.
* `!lightshow <name>` Plays the specified [custom lightshow] effect.

[color preset]: https://github.com/rakso20000/TwitchFX#color-presets
[custom lightshow]: https://github.com/rakso20000/TwitchFX#custom-lightshows

### Arguments

#### duration

The duration is given in seconds and does not have to be an integer.

#### color

TODO: explain colors

## Configuration

TODO: explain configuration options

## Color Presets

TODO: explain color presets

## Custom lightshows

TODO: explain custom lightshows
