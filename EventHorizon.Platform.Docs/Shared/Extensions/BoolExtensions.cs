public static class BoolExtensions
{
    public static string ToLower(
        this bool boolValue
    ) => boolValue.ToString(
        System.Globalization.CultureInfo.CurrentCulture
    ).ToLower(
        System.Globalization.CultureInfo.CurrentCulture
    );
}
