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
