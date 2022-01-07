namespace Shipwreck.ViewModelUtils;

public static class ExtensibleEntityHelper
{
    public static string GetString(ref string stringCache, IEnumerable<KeyValuePair<string, string>> dictionary)
    {
        if (stringCache == null && dictionary?.Any() == true)
        {
            var sb = new StringBuilder();
            foreach (var kv in dictionary)
            {
                if (!string.IsNullOrWhiteSpace(kv.Value))
                {
                    foreach (var c in kv.Key)
                    {
                        if (c == '\\' || c == ',')
                        {
                            sb.Append('\\');
                        }
                        sb.Append(c);
                    }
                    sb.Append(',');
                    foreach (var c in kv.Value)
                    {
                        if (c == '\\' || c == ',')
                        {
                            sb.Append('\\');
                        }
                        sb.Append(c);
                    }
                    sb.Append(',');
                }
            }
            stringCache = sb.ToString();
        }
        return stringCache;
    }

    public static void SetString(ref string stringCache, ref Dictionary<string, string> dictionary, string value)
    {
        if (value != stringCache)
        {
            stringCache = value;
            dictionary = null;
        }
    }

    public static Dictionary<string, string> GetDictionary(string stringCache, ref Dictionary<string, string> dictionary)
    {
        if (dictionary == null && stringCache?.Length > 1)
        {
            StringBuilder sb = null;
            var escaped = false;
            string key = null;
            foreach (var c in stringCache)
            {
                if (escaped)
                {
                    escaped = false;
                }
                else
                {
                    if (c == '\\')
                    {
                        escaped = true;
                        continue;
                    }
                    else if (c == ',')
                    {
                        var v = sb.ToString();
                        sb.Clear();

                        if (key != null)
                        {
                            if (!string.IsNullOrWhiteSpace(v))
                            {
                                (dictionary ??= new Dictionary<string, string>())[key] = v.TrimEnd();
                            }
                            key = null;
                        }
                        else
                        {
                            key = v;
                        }
                        continue;
                    }
                }
                (sb ??= new StringBuilder()).Append(c);
            }
        }
        return dictionary;
    }

    public static IEnumerable<KeyValuePair<string, string>> Enumerate(string stringCache, ref Dictionary<string, string> dictionary)
        => GetDictionary(stringCache, ref dictionary)
            ?.Where(e => !string.IsNullOrWhiteSpace(e.Value))
            ?.Select(e => new KeyValuePair<string, string>(e.Key, e.Value.TrimEnd()))
        ?? Enumerable.Empty<KeyValuePair<string, string>>();

    public static string Get(string stringCache, ref Dictionary<string, string> dictionary, string key)
    {
        if (key == null)
        {
            return null;
        }
        var d = GetDictionary(stringCache, ref dictionary);
        return d != null && d.TryGetValue(key, out var v) ? v : null;
    }

    public static void Set(ref string stringCache, ref Dictionary<string, string> dictionary, string key, string value)
    {
        if (key != null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (dictionary != null)
                {
                    if (dictionary.Remove(key))
                    {
                        stringCache = null;
                    }
                }
                else if (!string.IsNullOrEmpty(stringCache))
                {
                    if (GetDictionary(stringCache, ref dictionary) != null)
                    {
                        if (dictionary.Remove(key))
                        {
                            stringCache = null;
                        }
                    }
                    else
                    {
                        stringCache = null;
                    }
                }
            }
            else
            {
                value = value.TrimEnd();
                if (dictionary != null)
                {
                    dictionary[key] = value;
                    stringCache = null;
                }
                else if (!string.IsNullOrEmpty(stringCache))
                {
                    if (GetDictionary(stringCache, ref dictionary) != null)
                    {
                        dictionary[key] = value;
                        stringCache = null;
                    }
                    else
                    {
                        stringCache = null;
                        dictionary = new Dictionary<string, string>()
                        {
                            [key] = value
                        };
                    }
                }
                else
                {
                    stringCache = null;
                    dictionary = new Dictionary<string, string>()
                    {
                        [key] = value
                    };
                }
            }
        }
    }
}
