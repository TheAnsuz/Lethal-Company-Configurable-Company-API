# Developing with ConfigurableCompany

_The guide assumes you already know the basics of programming, c# and the basic usage of visual studio._

[Creation](##Creating-a-category)  
[Parameters](##Parameters)  
[Usage](##Using-a-category)

## Creating a page

Creating a page is the simplest of the operations you can do. Call `LethalConfiguration.CreatePage()` and it will provide you with the needed builder.

Here is an example on how you can create a page:

```csharp
ConfigurationPage page = LethalConfiguration.CreatePage()
    .SetName("Page 2")
    .Build(); // Optional
```

## Parameters

-   **Name**/`SetName(string)`: The name that will be displayed on the in-game menu.

## Using a page

To use a page, you need to store the page variable itself and assign configuration categories to it.

`ConfigurationCategoryBuilder.SetPage(ConfigurationPage)`
