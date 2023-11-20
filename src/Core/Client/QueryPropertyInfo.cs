namespace Shipwreck.ViewModelUtils.Client;

[DataContract]
public partial class QueryPropertyInfo : ICloneable
{
    public QueryPropertyInfo()
    {
    }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string DisplayName { get; set; }

    [DataMember]
    public string TypeName { get; set; }

    [DataMember]
    public string DefaultOperator { get; set; }

    protected virtual QueryPropertyInfo CreateInstance() => new QueryPropertyInfo();

    public virtual void CopyTo(QueryPropertyInfo other)
    {
        other.Name = Name;
        other.DisplayName = DisplayName;
        other.TypeName = TypeName;
        other.DefaultOperator = DefaultOperator;
    }

    public QueryPropertyInfo Clone(string newName = null, string newDisplayName = null, string newTypeName = null, string newDefaultOperator = null)
    {
        var r = CreateInstance();
        CopyTo(r);
        r.Name = newName ?? r.Name;
        r.DisplayName = newDisplayName ?? r.DisplayName;
        r.TypeName = newTypeName ?? r.TypeName;
        r.DefaultOperator = newDefaultOperator ?? r.DefaultOperator;
        return r;
    }

    object ICloneable.Clone() => Clone();
}
