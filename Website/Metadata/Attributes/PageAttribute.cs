namespace Website.Metadata.Attributes;

using System;

[AttributeUsage(AttributeTargets.Class)]
public class PageMetadataAttribute : Attribute
{
    public float Order { get; set; }
    public bool InNavigation { get; set; } = true;
    public string Title { get; set; } = string.Empty;
}
