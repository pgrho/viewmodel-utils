﻿namespace Shipwreck.ViewModelUtils.Demo.Blazor.ViewModels;

public sealed class ListTestPageViewModel : PageViewModel
{
    public ListTestPageViewModel(PageBase page)
        : base(page)
    {
    }

    public sealed class Item : ObservableModel
    {
        public string Name { get; set; }

        public TypeCode Enum { get; set; }
        public byte Byte { get; set; }
        public byte? NullableByte { get; set; }
        public short Int16 { get; set; }
        public short? NullableInt16 { get; set; }
        public int Int32 { get; set; }
        public int? NullableInt32 { get; set; }
        public long Int64 { get; set; }
        public long? NullableInt64 { get; set; }
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.Now;
    }

    public BulkUpdateableCollection<Item> Items { get; } = new BulkUpdateableCollection<Item>();

    private bool _IsLoading;

    public bool IsLoading
    {
        get => _IsLoading;
        private set
        {
            if (SetProperty(ref _IsLoading, value))
            {
                _AddBulkCommand?.Invalidate();
            }
        }
    }

    private CommandViewModelBase _AddBulkCommand;

    public CommandViewModelBase AddBulkCommand => _AddBulkCommand ??= CommandViewModel.CreateAsync(async () =>
    {
        IsLoading = true;

        for (var i = 0; i < 5; i++)
        {
            await Task.Delay(200);
            var item = new Item() { Name = Random.Shared.Next().ToString() };

            Items.Set(Items.Append(item).ToList());
        }

        IsLoading = false;
    }, titleGetter: () => IsLoading ? "Adding" : "Add items", iconGetter: () => IsLoading ? "fas fa-spinner fa-pulse" : " fas fa-plus");
}
