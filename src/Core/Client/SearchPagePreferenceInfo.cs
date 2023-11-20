namespace Shipwreck.ViewModelUtils.Client;

[DataContract]
public partial class SearchPagePreferenceInfo : FrameworkMessageObject
{

    public SearchPagePreferenceInfo()
        : base(null)
    {
    }

    #region Conditions

    private Collection<SearchPageDefaultConditionInfo> _Conditions;

    [DataMember]
    public IList<SearchPageDefaultConditionInfo> Conditions
    {
        [DynamicDependency(nameof(ShouldSerializeConditions))]
        get => CollectionHelper.GetOrCreate(ref _Conditions);

        [DynamicDependency(nameof(ShouldSerializeConditions))]
        set => CollectionHelper.Set(ref _Conditions, value);
    }

    public bool ShouldSerializeConditions() => !AreConditionsGenerated;

    #endregion Conditions

    #region Orders

    private Collection<string> _Orders;

    [DataMember]
    public IList<string> Orders
    {
        [DynamicDependency(nameof(ShouldSerializeOrders))]
        get => CollectionHelper.GetOrCreate(ref _Orders);

        [DynamicDependency(nameof(ShouldSerializeOrders))]
        set => CollectionHelper.Set(ref _Orders, value);
    }

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
