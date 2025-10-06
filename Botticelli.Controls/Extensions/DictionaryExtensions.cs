using System.Collections;

namespace Botticelli.Controls.Extensions;

public static class DictionaryExtensions
{
    public static T ReturnValueOrDefault<T>(this IDictionary dict, object key)
    {
        if (dict == null) return default!;

        foreach (var k in dict.Keys)
        {
            var val = dict[k];

            if (k == key) return val is T value ? value : default!;
        }

        return default!;
    }
}