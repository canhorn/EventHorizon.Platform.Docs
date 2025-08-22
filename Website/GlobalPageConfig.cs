namespace Website;

using System;
using System.Collections.Generic;

public static class GlobalPageConfig
{
    public static readonly IDictionary<string, float> PageOrders = new Dictionary<string, float>()
    { };

    public static string BaseUrl = Environment.GetEnvironmentVariable("EHZ_BASE_URL") ?? "";
}
