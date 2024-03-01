# Creating Configuration Displays

Configuration displays are the way to render your own configuration types in the in-game menu, if you are not creating your own configuration types you should not need to create custom displays.

| [Extending the class](#extending-the-base-class) | [Inherited fields](#utility-methods) | [Usage](#using-the-display) |
| ------------------------------------------------ | ------------------------------------ | --------------------------- |

If you want to see how these are implemented or use the default ones, you can check out the default implementations inside the namespace `Amrv.ConfigurableCompany.Core.Display.ConfigTypes`.

## Extending The Base Class

To create your own type, you must create a class that extends the `ConfigDisplay` class.

This will force you to implement the following methods.

```csharp
protected GameObject CreateContainer(CConfig config) {}
```

This method will be executed upon creation of the page and requires the to return the `GameObject` that contains the display. This is where the initialization of the component should be done.

```csharp
protected void LoadFromConfig(in object value) {}
```

This method will be called right after the `CreateContainer` method is called and every time the assigned configuration is changed.

The `value` parameter is what the configuration is containing. You must parse it to be displayed in the menu or you can use `Config.Type.TryGetAs(value, out T result)` to try to convert it to the value you want (only if the type is compatible with what the configuration can convert).

```csharp
protected void SaveToConfig(out object value) {}
```

This method will be called automatically when a request to save configurations is called. You must return a value that the configuration can accept (if the configuration does not accept the value, it will be ignored).

```csharp
protected void WhenToggled(bool enabled) {}
```

If the defined configuration can be toggled, this method will be called after a request to change it's state to active/inactive is called.

You should use this method to block the input.

```csharp
// Optional
protected void WhenReset(bool enabled) {}
```

This method gets called after a reset request has been done.

You don't need to implement this method as the `LoadFromConfig` will also be called upon reset.

_It's only there just in case you create a weird display type that needs to do something extra after resetting the displayed value._

```csharp
// Optional
protected void WhenRestored(bool enabled) {}
```

This method gets called after a restore request has been done.

You don't need to implement this method as the `LoadFromConfig` will also be called upon restore.

_It's only there just in case you create a weird display type that needs to do something extra after restoring the displayed value._

## Utility Methods

Extending the class allows you to access the following methods:

-   `Reset()`: Trigger a rest of the displayed configuration.
-   `Restore()`: Trigger a restore of the displayed configuration.
-   `Toggle(bool)`: Toggle active/inactive the configuration (ignored if the configuration cannot be disabled).
-   `Container`: A property that allows you to access the **GameObject** that you provided in the `CreateContainer` method.
-   `Config`: A property containing the CConfig assigned to this display (It wont change).

## Using The Display

To show the display you just need to return a new instance from your custom type, nothing else.

You should not create any instance of this class anywhere else, nor call any of the methods it provides anywhere. Only implement the methods or use the utility methods provided.
