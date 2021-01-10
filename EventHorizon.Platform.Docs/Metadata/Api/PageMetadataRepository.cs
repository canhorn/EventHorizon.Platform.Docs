namespace EventHorizon.Platform.Docs.Metadata.Api
{
    using System;
    using System.Collections.Generic;
    using EventHorizon.Platform.Docs.Metadata.Model;

    public interface PageMetadataRepository
    {
        PageNavigation Nav();
        IEnumerable<PageMetadataModel> All();
        PageMetadataModel Get(
            string route
        );
    }
}
