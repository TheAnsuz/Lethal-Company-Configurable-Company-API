# Developing with ConfigurableCompany

_The guide assumes you already know the basics of programming, c# and the basic usage of visual studio._

[Creation](##Creating-a-configuration)  
[Parameters](##Parameters)  
[Configuration Types](##Configuration-types)  
[Usage](##Using-a-configuration)

## Creating a configuration

Configurations are the core of the mod and provide a way for the user to modify some in-game options that are stored with each file save separately.

To create a configuration is as simple as calling `LethalConfiguration.CreateConfig`. You can choose to provide an ID right from the start or later, however, all configurations must contain an unique ID.

**IDs** must be lowercase, contain only letters, numbers, hypens ( **-** ) and underscores ( **\_** ). I'd recommend to make your IDs look something like `author_configuration-name` or `author_mod_configuration-name`.

Here is an example on how you can create a configuration:

```csharp
LethalConfiguration.CreateConfig()
                .SetID("amrv_configurable-company_custom-configuration")
                .SetName("Custom configuration")
                .SetTooltip( // Optional but recommended
                    "This is a custom configuration that does nothing",
                    "This is a second line for the tooltip",
                    "",
                    "This is another line")
                .SetCategory(category) // Optional
                .SetType(ConfigurationTypes.String)
                .SetValue("Random value")
                .SetExperimental(false) // Optional
                .SetSynchronized(false) // Optional
                .Build(); // Optional
```

It's not necesary to call `Build()` if you are assigning the creation to a `Configuration` as it will implicitly call the build method to create the configuration.

## Parameters

_This is a description on what does every parameter actually mean._

-   **ID**/`SetID(string)`: The unique ID of the configuration.
-   **Name**/`SetName(string)`: The name that will be displayed on the in-game menu.
-   **Tooltip**/`SetTooltip(string array)`: Each line of the configuration tooltip. Keep it short and informative. _**OPTIONAL**_
-   **Category**/`SetCategory(string/ConfigurationCategory)`: Wich category will hold this configuration. There must be always a category, however a default one will be used as failsafe. _**OPTIONAL**_
-   **Type**/`SetType(ConfigurationType)`: The type of values that this configuration accepts. You can choose one from the alredy existing types `ConfigurationTypes.` or create your own.
-   **Value**/`SetValue(object)`: The value that will contain upon creation. Might be changed instantly if read from file.
-   **Experimental**/`SetExperimental(bool)`: If this configuration is not guaranteed to work. This is only visual notification for the users. _**OPTIONAL**_
-   **Synchronized**/`SetSynchronized(bool)`: Marks the configuration to be synchronized with other clients when they join the game. Useful if a configuration might only change client-side.
-   **NeedsRestart**/`SetNeedsRestart(bool)`: Marks the configuration that the client must restart the game for it to work

    `Build()` Will create the configuration. It will not be created until the method is called (However as mentioned above, the method will called automatically if you assign the creation to a configuration).

Once the `Build()` is called, you will **not** be able to modify the configuration any further.

## Configuration types

You can choose to create a configuration of your own type however it will take you less time to use one of the existing ones:

-   `String`: Allows any text (up to 32 characters)
-   `SmallString`: Allows a short text (up to 10 characters)
-   `Boolean`: Allows true or false
-   `Percent`: Allows a float value that goes from **0** to **100**
-   `Float`: Allows any float or whole number value
-   `Integer`: Allows any whole number value
-   `RangeInteger(min, max)`: A integer that only accepts value within the specified range
-   `RangeFloat(min, max)`: A float that only accepts values within the specified range
-   `Slider(min, max)`: A slider that allows any non-rounded value within the specified range
-   `StringOfLength(length)`: A string that allows you to set a maximum amount of characters that can go from 1 to 48.
-   `Options(Enumeration/object collection or array)`: A choosable option that allows for a specific value in a collection _The provided collection must be of just one type, you **can't** use an heterogeneous array_.

## Using a configuration

There are multiple ways you can get the value of a configuration. Each one might be used according to the situation.

-   `configuration.Get<T>()`: This will retrieve the value as an instance of T, no failsafes are used if the conversion of the value to T fails.
-   `configuration.Get<T>(T failsafe)`: This will retrieve the value as an instance of T but if the conversion fails it will return the `failsafe` value instead.
-   `configuration.TryGet<T>(out T value)`: This is a standard TryGet that will return true if the Get succeded and false if it failed. The resulting value will be stored in `T value`.
-   `configuration.Value`: Will return the raw object for the configuration without any cast or check.

_Some configuration types will be automatically converted to other types if you request it. For example the **Options** type will allow you to get the index of the item if you use `Get<int>()` or the value itself if you request it as `Get<T>()` (T being the type of the collection)_

To set a configuration you need to use `configuration.TrySet(newValue, ChangeReason)` and will return true if the set was succesful.

You can also reset a configuration to it's default value using `configuration.Reset()`.

_Keep in mind that setting or resetting configurations is not recommended as may interfere with the needs of the final user. I encourage you to **not** set configurations by yourself._
