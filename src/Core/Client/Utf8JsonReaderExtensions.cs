namespace Shipwreck.ViewModelUtils.Client;

internal static partial class Utf8JsonReaderExtensions
{
    public static string ReadString(ref this Utf8JsonReader reader)
    {
        if (reader.Read())
        {
            return reader.GetString();
        }

        throw new InvalidOperationException();
    }

    public static bool ReadBoolean(ref this Utf8JsonReader reader)
    {
        if (reader.Read())
        {
            return reader.GetBoolean();
        }

        throw new InvalidOperationException();
    }

    public static long ReadInt64(ref this Utf8JsonReader reader)
    {
        if (reader.Read())
        {
            return reader.GetInt64();
        }

        throw new InvalidOperationException();
    }
}
