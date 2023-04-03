namespace Shipwreck.ViewModelUtils.Demo.Blazor.ViewModels;

public sealed class ListTestPageViewModel : PageViewModel
{
    public ListTestPageViewModel(PageBase page)
        : base(page)
    {
    }

    public sealed class Item : ObservableModel
    {
        public string Name { get; set; }
    }

    public BulkUpdateableCollection<Item> Items { get; } = new BulkUpdateableCollection<Item>();

    private bool _IsLoading;

    public bool IsLoading
    {
        get => _IsLoading;
        private set => SetProperty(ref _IsLoading, value);
    }

    public async void Add()
    {
        IsLoading = true;

        for (var i = 0; i < 5; i++)
        { 
            await Task.Delay(200);
            var item = new Item() { Name = Random.Shared.Next().ToString() };

            Items.Set(Items.Append(item).ToList());
        }

        IsLoading = false;
    }
}
