namespace Website.Localization.Api;

public interface Localizer<T>
{
    string this[string name] { get; }
    string this[string name, params object[] arguments] { get; }
}
