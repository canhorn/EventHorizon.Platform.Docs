namespace Website;

using System.Collections.Generic;

public static class GlobalPageConfig
{
    public static readonly IDictionary<string, float> PageOrders = new Dictionary<string, float>()
    { };

    public const string PrivacyPolicyEmail = "[[EMAIL_ADDRESS]]";
    public const string DocsUrl = "[[DOCS_URL]]";
    public const string DocsSite = "[[DOCS_SITE]]";
    public const string DocsDescription = "[[DOCS_DESCRIPTION]]";
}
