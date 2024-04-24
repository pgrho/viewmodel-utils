namespace Shipwreck.ViewModelUtils.Client;

internal sealed partial class QueryGroupInfoCollection : MessageObjectCollection<FrameworkMessageBase, QueryGroupInfo>
{
    public QueryGroupInfoCollection(FrameworkMessageBase message)
        : base(message)
    { }
}
