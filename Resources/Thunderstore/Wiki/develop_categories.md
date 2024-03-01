# Develop Categories

Categories are the way to split configurations into different organized parts of a panel so players have a better time searching for what they want to modify.

Categories directly depend on the page and must be attached to one.

## Creating Categories

If you don't care about the categories that much you can use the easy access method to create categories.

```csharp
CCategory category = ConfigAPI.CreateCategoryAuto("Name", myPage, Color.white);
```

However if this method does not fit your needs, you can use the category builder.

```csharp
CPage page = new CCategoryBuilder() {
    Page = myPage,
    ID = "my-mod-name_category_my-custom-category",
    Color = Color.blue,
    Name = "My category",
}
```

## Parameters

These are the values you can modify from pages:

-   `string` ID:  
    The **unique** identifier for this section definition, no other existing section should have the same value, I recommend using the ID convention **[mod-name]**\_category\_**[category-name]**.

-   `string` Name:  
    The name that will be shown in the in-game menu.

-   `string` Page / `CPage` CPage:  
    The parent page of this category (uses the defaul page if no one is provided).

-   `Color` Color / `(byte, byte, byte)` ColorRGB:  
    The background color of the category display (defaults to white if no color is provided).

-   `bool` HideIfEmpty:  
    True if the category should be hidden if there are no configurations attached, false otherwise.

## Get Existing Pages

Sometimes you might just want to get categories provided by other mods. You can check if a category is registered by other mod calling:

```csharp
if (ConfigAPI.TryGetCategory("other-mod_category_some-name", out CCategory category)) {
    // The category exists and has been stored into "category"
} else {
    // The category does not exist
}
```
