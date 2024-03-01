# Develop Pages

Pages are the way to split configurations into different panels so users have a better time searching for what they want to modify.

They are only used for organization and visualization purposes.

## Creating Pages

If you don't care about the page that much you can use the easy access method to create one page for your mod.

```csharp
CPage page = ConfigAPI.CreatePageAuto("Name","Description");
```

However if this method does not fit your needs, you can use the page builder.

```csharp
CPage page = new CPageBuilder() {
    ID = "my-mod-name_page_my-custom-page",
    Name = "My custom page",
    Description = "My page description"
}
```

You can also make use of the **disposable object** pattern:

```csharp
public static void CreatePage() {
    using CPageBuilder builder = new CPageBuilder();
    builder.ID = "my-mod-name_page_my-custom-page";
    builder.Name = "My custom page"
    // After exiting the method, the page will be automatically created
}
```

## Parameters

These parameters are everything that you can choose to modify for the pages.

-   `string` ID:  
    The **unique** identifier for this page definition, no other existing page should have the same value, I recommend using the ID convention **[mod-name]**\_page\_**[page-name]**.

-   `string` Name:  
    The name that will be shown in the in-game menu.

-   `string` Description:  
    A brief tooltip about the page that will be shown in-game.

## Get Existing Pages

Some times you might just want to get pages provided by other mods. You can check if a page is registered by other mod calling:

```csharp
if (ConfigAPI.TryGetPage("other-mod_page_some-name", out CPage page)) {
    // The page exists and has been stored into "page"
} else {
    // The page does not exist
}
```
