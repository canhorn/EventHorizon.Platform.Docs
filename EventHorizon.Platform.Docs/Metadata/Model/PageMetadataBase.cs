namespace EventHorizon.Platform.Docs.Metadata.Model
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading.Tasks;
    using EventHorizon.Platform.Docs.Localization;
    using EventHorizon.Platform.Docs.Localization.Api;
    using EventHorizon.Platform.Docs.Metadata.Api;
    using Microsoft.AspNetCore.Components;

    public class PageMetadataBase
        : ComponentBase,
        PageMetadata
    {
        [Inject]
        public Localizer<SharedResource> Localizer { get; set; } = null!;
        [Inject]
        public PageMetadataRepository Repository { get; set; } = null!;

        public PageMetadataModel PageMetadata { get; private set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            PageMetadata = Repository.Get(
                GetType().GetCustomAttribute<RouteAttribute>().Template
            );
        }
    }
}
