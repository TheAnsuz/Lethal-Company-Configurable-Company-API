# Creating Configuration Types

Configuration types are the validators for the values that a configuration can accept. They check and accept or deny values and serialize them to transmit or store data in files.

If you just want to access the default configuration types, use the class `CTypes`.

| [Extending the class](#extending-the-base-class) | [Using the type](#using-the-custom-type) |
| ------------------------------------------------ | --------------------------- |

## Extending The Base Class

To create you own configuration type you must extend the `CType` class.

### Parameters

```csharp
public object Default {get; }
```

This property request the default value to be used for this configuration type. It must always be acceptable or errors will occur.

This value is only used as a fallback value, realistically it will never be used.

```csharp
public string TypeName {get; }
```

This is the name that will be shown in the menu tooltip for each configuration that uses this type.

It should represent a short text saying what values are acceptable.

```csharp
protected ConfigDisplay {get; }
```

This property must return a new instance of a Configuration display everytime it's called.

I recommend that if you create a custom `CType` you also create a custom `ConfigDisplay`.  
If you want to use the default ones, they are located under the namespace `Amrv.ConfigurableCompany.Core.Display.ConfigTypes`.

### Methods

```csharp
public bool IsValidValue(object value) {}
```

This method should return true whenever a value is directly compatible to the ones that this configuration type accepts.

```csharp
public bool TryConvert(object value, out object result, IFormatProvider formatProvider = null) {}
```

This method should return true or false depending if the configuration was succesful.

With this method you should try (best effort) to convert the provided object to an acceptable value by this configuration type. If it needs to be formatted you must use the provided formatter or an invariable one (otherwise the value will depend on the region of the player).

```csharp
public bool TryGetAs<T>(object value, out T result, Type type, TypeCode code, IFormatProvider formatProvider = null) {}
```

This method should try to convert object `value` (which is not granted to be acceptable by the configuration in edge cases) to a resulting object of the specified type `T`.  
You can convert it however you want or even deny the conversion (returning false) but if you make a lossy conversion, try to make it reasonably.

> **TIP**  
> Configurable Company provides the basic utility methods for conversion in the class `NumberUtils`.

```csharp
protected bool Serialize(in object value, out string serialized) {}
```

This method should convert object `value` into a serialized string that could be converted back into the same object `value` without losing any information.

You must not throw any exception but return false when the serialization cannot be performed.

```csharp
protected bool Deserialize(in string data, out object value) {}
```

This method is the reversal of the `Serialize` method, the provided data should be converted back into the original value.

## Using The Custom Type

To use your custom type you should decide if it needs to be a singleton or be instantiated once per configuration. For optimization purposes you should try to keep it as a singleton if possible.

You can manually assign the type of any configuration to an instance of this class by setting the `CConfigBuilder::Type`.

```csharp
// You store the instance on a variable
CType myCustomType = new CustomConfigType();

CConfig config = new CConfigBuilder() {
    // And then you assign it to any configuration
    Type = myCustomType,
}
```

You can also register this custom type so any configuration that contains a value of an acceptable type will be automatically assigned to this type.

This can be done by mapping it on your plugin class (make sure this method gets executed before registering any configuration).

```csharp
CType.SetMapping<T>(new CustomConfigType());
```
