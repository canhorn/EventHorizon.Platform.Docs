namespace Website.Localization.Api;

public interface Localizer<T>
{
    public string this[string name] { get; }
    public string this[string name, params object[] arguments] { get; }
}
