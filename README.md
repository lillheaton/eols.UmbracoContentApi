# Umbraco Content Api
A generic content api for the Umbraco platform. The nuget comes with a standard set of "property converters" but this converters can be overridden and handled differently to suit your needs (make usage of CDN for example)

Follows the same output structure as [EPiServer Content Api](https://github.com/lillheaton/eols.EPiContentApi)

###Installation

    PM> Install-Package EOls.UmbracoContentApi

###Usage

    [GET] /umbraco/api/content/{id}

###Custom Property Converters
By using the IApiPropertyConverter interface and the PropertyTypeAttribute you will be able to convert the property to suit your needs.

```C#
[PropertyType(Type = "Umbraco.TinyMCEv3")]
public class RichTextEditorConverter : IApiPropertyConverter
{
    public object Convert(IPublishedProperty property, IPublishedContent owner)
    {
        var htmlString = property.Value as HtmlString;
        return htmlString != null ? htmlString.ToHtmlString() : string.Empty;
    }
}
```

### Extend Document Type
Let's say you have a document type named UmbHomePage. You will be albe to extend this model by creating a class with the IDocumentTypeExtender interface.
When the api print content with that document type. It will also include the dictionary exampled below.

```C#
public class UmbHomePageModel : IDocumentTypeExtender
{
    public Dictionary<string, object> Extend(IPublishedContent content)
    {
        return new Dictionary<string, object>() { { "Foo", "Bar" } };
    }
}
```

#### In the future:
 * Maybe add a caching layer (not sure if needed)
 * More documentation
 * Unit tests!