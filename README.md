# Umbraco Content Api
A generic content api for Umbraco CMS. <br>Serializes the content in Umbraco to Json. It's possible to adapt how a specific Umbraco property gets serialized by adding custom "Property converter". The nuget package include a set of standard property converters.


### Installation

    PM> Install-Package EOls.UmbracoContentApi

### Usage

    [GET] /umbraco/api/content/get/{id}

### Custom Property Converters
If you want to adapt how a specific Umbraco property is serialized. Just add a class that implements the interface IApiPropertyConverter, and decorate the class with the PropertyTypeAttribute. 
<br><br>
All property converters (and document extenders) will be located by using reflection during site startup.


#### Example
In this example the text "Awesome!" will be added to the content when an Umbraco property of type "Umbraco.TinyMCEv3" is serialized.

```C#
[PropertyType(Type = "Umbraco.TinyMCEv3")]
public class RichTextEditorConverter : IApiPropertyConverter
{
    public object Convert(IPublishedProperty property, IPublishedContent owner, ContentSerializer serializer, UmbracoHelper umbraco)
    {
        var htmlString = property.Value as HtmlString;
        return htmlString != null ? htmlString.ToHtmlString() + "Awesome!" : string.Empty;
    }
}
```


### Extend Document Type
It's possible to extend a document type by creating a class with the IDocumentTypeExtender interface.
When the api returns content with this document type, it will include the additional data.

#### Example
Let's say you have a document type named "Home". For this specific document type you also want to return a dictionary with som additional data.

```C#
public class HomeDocument : IDocumentTypeExtender
{
    public Dictionary<string, object> Extend(IPublishedContent content)
    {
        return new Dictionary<string, object>() { { "Foo", "Bar" } };
    }
}
```


### Hide Document
If you don't want pages of a specific document type to be returned by the API.<br>
#### Example
Hide document type named "Settings"

```C#
[DocumentTypeSettingsAttribute(Hide = true)]
public class SettingsDocument { }
```