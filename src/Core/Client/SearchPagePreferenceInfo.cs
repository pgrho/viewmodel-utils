namespace Shipwreck.ViewModelUtils.Client;

[DataContract]
public partial class SearchPagePreferenceInfo : FrameworkMessageObject
{
    [RequiresUnreferencedCode("Default Constructor")]
    public SearchPagePreferenceInfo()
        : base(null)
    {
    }

    #region Conditions

    private Collection<SearchPageDefaultConditionInfo> _Conditions;

    [DataMember]
    public IList<SearchPageDefaultConditionInfo> Conditions
    {
        get => CollectionHelper.GetOrCreate(ref _Conditions);
        set => CollectionHelper.Set(ref _Conditions, value);
    }

    [RequiresUnreferencedCode("ShouldSerialize")]
    public bool ShouldSerializeConditions() => !AreConditionsGenerated;

    #endregion Conditions

    #region Orders

    private Collection<string> _Orders;

    [DataMember]
    public IList<string> Orders
    {
        get => CollectionHelper.GetOrCreate(ref _Orders);
        set => CollectionHelper.Set(ref _Orders, value);
    }

    [RequiresUnreferencedCode("ShouldSerialize")]
    public bool ShouldSerializeOrders() => !AreOrdersGenerated;

    #endregion Orders

    public SearchPagePreferenceInfo AddOrder(string propertyName, bool isDescending)
    {
        Orders.Add(propertyName + (isDescending ? " DESC" : null));
        return this;
    }

    public SearchPagePreferenceInfo AddOrder(string propertyName1, string propertyName2, bool isDescending)
    {
        Orders.Add(propertyName1 + "." + propertyName2 + (isDescending ? " DESC" : null));
        return this;
    }

    [IgnoreDataMember]
    public bool AreConditionsGenerated { get; set; }

    [IgnoreDataMember]
    public bool AreOrdersGenerated { get; set; }

    [IgnoreDataMember]
    public bool IsGenerated => AreConditionsGenerated && AreOrdersGenerated;
}
