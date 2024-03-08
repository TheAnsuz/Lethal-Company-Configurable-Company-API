# 3.1.0

### Fixed

-   Hard crash when trying to get value of range configuration type
-   Typo on `CConfigBuilder` **Toggleable**

### Added

-   Accesibility methods for `RangeTypes`

### Removed

-   Accesibility methods for number variables now require to input both arguments
-   Unnecesary console logs in release version
# 3.0.1

### Modified

-   Code cleanup

### Fixed

-   Now you can change configurations for BetterSaves's new file
-   Fixed incorrect Canvas attachment of the menu with some mods

# 3.0.0

### Modified

-   Configurations now are not shared between profiles
-   Reworked the whole interface
-   Reimplemented everything

### Added

-   Configurations now will synchronize in real time
-   Added sections to split categories even more
-   New configuration type `Range` wich accepts any agrupation of two values (array[2], Tuple<,> and ValueTuple<,>)

### Fixed

-   Compatibility issues with many mods such as better saves

# 2.6.0

### Fixed

-   Menu not appearing when hosting after joining a game

### Added

-   Configurations can be reverted by clicking while holding Shift key
-   Configurations can be reset by clicking while holding Ctrl + Shift
-   Added `experimental` tag
-   Added buttons to `copy` and `paste` configurations so you can share them with your friends

### Deprecated

-   The attribute `Needs restart` is now obsolete, it will be removed in future releases.  
    The decision has been made because developers should try to make their settings modificable even after the game has started. This might be hard to understand but Ill be glad to help anyone who needs asistance with it, you can also check Lethal company variables and see how it implements this functionality.

### Removed

-   Removed the log entry when loading configurations from file

# 2.5.2

### Modified

-   Now configurations will not depend upon the region/language of the computer

# 2.5.3

-   Updated project target framework for `net standard 2.1`

# 2.5.2

### Added

-   [BetterSaves](https://thunderstore.io/c/lethal-company/p/Pooble/LCBetterSaves/) compatibility

### Modified

-   Internal changes to in-game menu for better compatibility implementation

### Fixed

-   Configurations won't reset to their previous state when clicking `[ BACK ]` button

# 2.5.1

### Added

-   Added public project repository
-   Added Issues
-   Added github wiki
-   Added new contact methods
-   Added `Information / Guide` button to show a simple usage for the menu

### Modified

-   Updated contact information in readme

# 2.5.0

_If you created your own ConfigurationType you **MUST** update your mod with the newer internal implementation_

### Added

-   New configuration type `ConfigurationTypes.StringOfLength(int)` that allows for a string with a maximum length (this can go up to 48 characters)
-   New configuration type `ConfigurationTypes.Options(Enumeration / collection of values)` that let the user choose one option from a list, you can get back the value as an `int` (for the index) or as a `T` (for the value at the index)

### Modified

-   Now config getters allow to get their value as other types, for example a float config as an int without casting
-   The image now is from a trusted somain so everyone should be able to see it (NuGet only)
-   Added spacing to the tooltip's type range values
-   Category open state now persists during sesions
-   Changed icon to a more modern design

### Fixed

-   Removed strict casting when getting configuration values as a `T` type
-   Configuration values now reset correctly with newly generated configurations

# 2.4.1

### Fixed

-   Configuration pages now have a better title fitting with font scaling

### Modified

-   Default page and category now will only appear if a developer uses them (a getter will spawn them)

# 2.4.0

### Added

-   New configuration types: RangedInteger, RangedFloat and Slider (a slider that allows to set a min and max values)

### Fixed

-   Page changing to the left is now cyclic

# 2.3.4

### Fixed

-   The disabled menu information now displays correctly
-   Menu not being displayed again when leaving using leaderboard button

# 2.3.3

### Modified

-   Now there is no configuration menu for challenge mode (this is a temporal change and might change depending on the result of the voting. You can vote in the [official modding discord](https://discord.gg/lcmod) or contact directly to `the_ansuz`)

### Added

-   The voting information is displayed when selecting the challenge file

# 2.3.2

_There is absolutely nothing added, some people just want to see the mod updated to make sure it works with the next update_

# 2.3.1

### Added

-   v47 compatibility

### Modified

-   Updated mod information
-   **Reset file** now resets even configs that are not loaded (deleting the config file)

### Fixed

-   Fixed change file error when there are no mod configs

# 2.3.0

### Added

-   Configurations are now organized in different pages

### Fixed

-   Reworked 2.2.0 changes
-   Quit button not working on specific circumstances
