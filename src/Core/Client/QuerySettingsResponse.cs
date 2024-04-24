namespace Shipwreck.ViewModelUtils.Client;

[DataContract]
public partial class QuerySettingsResponse : FrameworkMessageBase
{
    #region Types

    private QueryTypeInfoCollection _Types;

    [DataMember]
    public IList<QueryTypeInfo> Types
    {
        get => _Types ??= new(this);
        set => ((QueryTypeInfoCollection)Types).Set(value);
    }

    #endregion Types

    #region Groups

    private QueryGroupInfoCollection _Groups;

    [DataMember]
    public IList<QueryGroupInfo> Groups
    {
        get => _Groups ??= new(this);
        set => ((QueryGroupInfoCollection)Groups).Set(value);
    }

    #endregion Groups
}
