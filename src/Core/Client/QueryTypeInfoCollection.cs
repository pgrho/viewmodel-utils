namespace Shipwreck.ViewModelUtils.Client;

internal sealed partial class QueryTypeInfoCollection : MessageObjectCollection<FrameworkMessageBase, QueryTypeInfo>
{
    public QueryTypeInfoCollection(FrameworkMessageBase message)
        : base(message)
    { }
}
