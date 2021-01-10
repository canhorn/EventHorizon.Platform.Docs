namespace EventHorizon.Platform.Docs.Localization.Model
{
    using EventHorizon.Platform.Docs.Localization.Api;
    using Microsoft.Extensions.Localization;

    public class StringBasedLocalizer<T>
        : Localizer<T>
    {
        private readonly IStringLocalizer<T> _localizer;

        public string this[string name]
            => _localizer[name] ?? name;

        public string this[string name, params object[] arguments]
            => _localizer[name, arguments] ?? string.Format(name, arguments);

        public StringBasedLocalizer(
            IStringLocalizer<T> localizer
        )
        {
            _localizer = localizer;
        }
    }
}
