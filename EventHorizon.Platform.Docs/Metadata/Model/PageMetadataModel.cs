namespace EventHorizon.Platform.Docs.Metadata.Model
{
    using System;
    using System.Collections.Generic;

    public class PageMetadataModel
    {
        public string Route { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;

        public IDictionary<string, string> CustomProperties { get; set; } = new Dictionary<string, string>();

        public string GetString(
            string propertyName
        )
        {
            if (CustomProperties.TryGetValue(
                propertyName,
                out var value
            ))
            {
                return value;
            }
            return string.Empty;
        }
    }
}
