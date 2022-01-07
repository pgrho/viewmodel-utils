namespace Shipwreck.ViewModelUtils.Client;

internal static class Utf8JsonReaderHelper
{
    public static List<string> ReadStringList(this ref Utf8JsonReader reader)
    {
        if (!reader.Read())
        {
            throw new InvalidOperationException();
        }
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }
        else if (reader.TokenType == JsonTokenType.StartArray)
        {
            List<string> list = null;
            for (; ; )
            {
                if (!reader.Read())
                {
                    throw new InvalidOperationException();
                }
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    return list;
                }
                (list ??= new List<string>()).Add(reader.ReadString());
            }
        }
        else
        {
            throw new InvalidOperationException();
        }
    }
}
