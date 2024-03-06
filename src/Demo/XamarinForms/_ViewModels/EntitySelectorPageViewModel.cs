using Xamarin.Forms;

namespace Shipwreck.ViewModelUtils.Demo.XamarinForms;

public sealed class EntitySelectorPageViewModel : PageViewModel
{
    public sealed class Item
    {
        public static readonly Item[] Items = Enumerable.Range(1, 100).Select(i => new Item(i)).ToArray();

        public Item(int id)
        {
            Id = id;
            Code = id.ToString("0000");
            DisplayName = id.ToString("'Item '0");
        }

        public int Id { get; }
        public string Code { get; }
        public string DisplayName { get; }
    }

    public sealed class ItemSelector : EntitySelectorBase<int, Item>
    {
        public ItemSelector(PageViewModel page)
            : base(page) { }

        protected override string EntityDisplayName => nameof(Item);

        public override Item GetById(int id)
            => Item.Items.FirstOrDefault(e => e.Id == id);

        public override string GetCode(Item item)
            => item?.Code;

        public override int GetId(Item item)
            => item?.Id ?? 0;

        public override int GetMatchDistance(string code, Item item)
            => item?.Code == code ? 0
            : item?.Code?.StartsWith(code) == true ? 1
            : item?.Code?.Contains(code) == true ? 2
            : int.MaxValue;

        public override string GetName(Item item)
            => item?.DisplayName;

        protected override async Task<IReadOnlyList<Item>> GetItemsAsync()
        {
            await Task.Delay(1500);
            return Item.Items;
        }

        public override async Task<IEnumerable<Item>> SearchAsync(string query, int maxCount, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1500);

            if (query.StartsWith("Id:") && int.TryParse(query.Substring(3), out var id))
            {
                return Item.Items.Where(e => e.Id == id).ToList();
            }
            else
            {
                if (maxCount <= 0)
                {
                    maxCount = int.MaxValue;
                }
                if (query.StartsWith("^=") && query.Length > 2)
                {
                    var q = query.Substring(2);
                    return Item.Items.Where(e => GetMatchDistance(q, e) <= 1).Take(maxCount).ToList();
                }
                else
                {
                    return Item.Items.Where(e => GetMatchDistance(query, e) <= 2).Take(maxCount).ToList();
                }
            }
        }

        public override async Task<bool> SelectByCodeAsync(string code, bool isExactMatch = false)
        {
            await Task.Delay(1500);

            Func<Item, bool> pred = isExactMatch ? e => e.Code == code : e => GetMatchDistance(code, e) <= 1;
            var item = Item.Items.FirstOrDefault(pred);
            if (item == null)
            {
                return false;
            }
            else
            {
                SelectedItem = item;
                SelectedId = item.Id;
                return true;
            }
        }

        public override bool TryParseId(string s, out int id)
            => int.TryParse(s, out id);
    }

    public EntitySelectorPageViewModel(Page page)
        : base(page)
    {
        Title = "EntitySelector";
    }

    #region CodeSelector

    private ItemSelector _CodeSelector;

    public ItemSelector CodeSelector => _CodeSelector ??= new ItemSelector(this);

    #endregion CodeSelector

    #region ListSelector

    private ItemSelector _ListSelector;

    public ItemSelector ListSelector => _ListSelector ??= new ItemSelector(this)
    {
        UseList = true
    };

    #endregion ListSelector
}
