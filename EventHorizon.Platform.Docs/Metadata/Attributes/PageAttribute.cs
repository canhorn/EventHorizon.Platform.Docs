namespace EventHorizon.Platform.Docs.Metadata.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class PageMetadataAttribute
        : Attribute
    {
        public string Title { get; set; } = string.Empty;
    }
}
