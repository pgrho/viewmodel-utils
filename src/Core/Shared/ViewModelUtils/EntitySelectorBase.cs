using System.Collections;

namespace Shipwreck.ViewModelUtils;

public abstract class EntitySelectorBase<TId, TItem> : ObservableModel, IEntitySelector<TId, TItem>
    where TItem : class
{
    protected EntitySelectorBase(FrameworkPageViewModel page)
    {
        Page = page;
    }

    public FrameworkPageViewModel Page { get; }

    protected abstract string EntityDisplayName { get; }

    #region TId

    protected virtual bool Equals(TId x, TId y)
        => EqualityComparer<TId>.Default.Equals(x, y);

    public virtual bool IsValid(TId id)
        => !Equals(id, default);

    bool IEntitySelector.IsValid(object id) => id is TId v && IsValid(v);

    public abstract bool TryParseId(string s, out TId id);

    bool IEntitySelector.TryParseId(string s, out object result)
    {
        if (TryParseId(s, out var id))
        {
            result = id;
            return true;
        }
        result = null;
        return false;
    }

    #endregion TId

    #region TItem

    public abstract TId GetId(TItem item);

    object IEntitySelector.GetId(object item) => GetId((TItem)item);

    public abstract string GetCode(TItem item);

    string IEntitySelector.GetCode(object item) => GetCode((TItem)item);

    public abstract string GetName(TItem item);

    string IEntitySelector.GetName(object item) => GetName((TItem)item);

    public virtual string GetDisplayText(TItem item)
        => $"{GetCode(item)}: {GetName(item)}";

    string IEntitySelector.GetDisplayText(object item) => GetDisplayText((TItem)item);

    int IEntitySelector.GetMatchDistance(string code, object item)
        => GetMatchDistance(code, (TItem)item);

    public abstract int GetMatchDistance(string code, TItem item);

    public abstract TItem GetById(TId id);

    object IEntitySelector.GetById(object id) => id is TId i ? GetById(i) : null;

    #endregion TItem

    #region UseList

    private protected bool _UseList;

    public bool UseList
    {
        get => _UseList;
        set
        {
            if (SetProperty(ref _UseList, value))
            {
                InvalidateItems();
            }
        }
    }

    public void SetUseList(bool value)
        => UseList = value;

    #endregion UseList

    #region IsEnabled

    private bool _IsEnabled = true;

    public bool IsEnabled
    {
        get => _IsEnabled;
        set
        {
            if (SetProperty(ref _IsEnabled, value))
            {
                _ShowModalCommand?.Invalidate();
                _ClearCommand?.Invalidate();
            }
        }
    }

    public void SetIsEnabled(bool value)
        => IsEnabled = value;

    #endregion IsEnabled

    #region Items

    private BulkUpdateableCollection<TItem> _Items;
    private Task<BulkUpdateableCollection<TItem>> _ItemsTask;

    public BulkUpdateableCollection<TItem> Items
    {
        get
        {
            ItemsTask.GetHashCode();
            return _Items ??= new BulkUpdateableCollection<TItem>();
        }
    }

    internal Task<BulkUpdateableCollection<TItem>> ItemsTask
        => _ItemsTask ?? CreateItemsTask();

    private Task<BulkUpdateableCollection<TItem>> CreateItemsTask()
    {
        if (UseList)
        {
            var tcs = new TaskCompletionSource<BulkUpdateableCollection<TItem>>();
            _ItemsTask = tcs.Task;
            _Items ??= new BulkUpdateableCollection<TItem>();

            async void SetItemsCore()
            {
                try
                {
                    var core = await GetItemsAsync().ConfigureAwait();
                    if (_ItemsTask == tcs.Task)
                    {
                        SetItems(core);
                        tcs.TrySetResult(_Items);
                    }
                    else
                    {
                        tcs.TrySetCanceled();
                    }
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }

            SetItemsCore();
        }
        else
        {
            _Items ??= new BulkUpdateableCollection<TItem>();
            _ItemsTask = Task.FromResult(_Items);
            SetItems(Array.Empty<TItem>());
        }
        return _ItemsTask;
    }

    Task<IReadOnlyList<TItem>> IEntitySelector<TId, TItem>.GetItemsTask() => ItemsTask.ContinueWith(t => (IReadOnlyList<TItem>)t.Result);

    Task<IList> IEntitySelector.GetItemsTask() => ItemsTask.ContinueWith(t => (IList)t.Result);

    public virtual void InvalidateItems()
        => CreateItemsTask().GetHashCode();

    protected virtual void SetItems(IReadOnlyList<TItem> items)
    {
        if (!items.SequenceEqual(Items))
        {
            Items.Set(items);
            OnItemsSet();
        }
    }

    protected virtual void OnItemsSet()
    {
        if (UseList)
        {
            if (SelectedItem != null)
            {
                if (!Items.Contains(SelectedItem))
                {
                    SelectedItem = null;
                }
            }
            else if (SelectedId != null
                && GetById(SelectedId) is TItem item)
            {
                SelectedItem = item;
            }
            ItemLoaded?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler ItemLoaded;

    #endregion Items

    #region 選択項目

    #region SelectedId

    private TId _SelectedId;

    public TId SelectedId
    {
        get => _SelectedId;
        set => SetSelection(
            IsValid(value) ? value : default,
            IsValid(value) ? GetById(value) : null,
            true);
    }

    object IEntitySelector.SelectedId
    {
        get => SelectedId;
        set => SelectedId = (TId)value;
    }

    #endregion SelectedId

    #region SelectedItem

    private TItem _SelectedItem;

    public TItem SelectedItem
    {
        get => _SelectedItem;
        set
        {
            var id = GetId(value);
            SetSelection(
                IsValid(id) ? id : default,
                IsValid(id) ? value : null,
                false);
        }
    }

    object IEntitySelector.SelectedItem
    {
        get => SelectedItem;
        set => SelectedItem = (TItem)value;
    }

    public void Select(TItem item)
    {
        if (item != SelectedItem)
        {
            SelectedItem = item;
        }
        else if (item != null)
        {
            RaisePropertyChanged(nameof(SelectedItem));
        }
    }

    void IEntitySelector.Select(object item)
        => Select((TItem)item);

    #endregion SelectedItem

    #region Code

    private string _Code = string.Empty;

    public string Code
    {
        get => _Code;
        set => SetProperty(ref _Code, value ?? string.Empty);
    }

    #endregion Code

    private void SetSelection(TId id, TItem item, bool shouldLoadItem)
    {
        var idChanged = !Equals(id, _SelectedId);
        var itemChanged = _SelectedItem != item;

        _SelectedId = id;
        _SelectedItem = item;

        if (idChanged)
        {
            RaisePropertyChanged(nameof(SelectedId));
        }
        if (itemChanged)
        {
            RaisePropertyChanged(nameof(SelectedItem));
        }
        _SelectOrClearCommand?.Invalidate();
        _ClearCommand?.Invalidate();

        if (shouldLoadItem
            && Equals(SelectedId, id)
            && IsValid(SelectedId)
            && SelectedItem == null)
        {
            if (_UseList)
            {
                ItemsTask.ContinueWith(t =>
                {
                    if (Equals(id, _SelectedId) && t.Status == TaskStatus.RanToCompletion)
                    {
                        SelectedItem = t.Result.FirstOrDefault(e => Equals(GetId(e), id));
                    }
                });
            }
            else
            {
                GetByIdAsync(id, default).ContinueWith(t =>
                {
                    if (Equals(id, _SelectedId) && t.Status == TaskStatus.RanToCompletion)
                    {
                        SelectedItem = t.Result;
                    }
                });
            }
        }
    }

    #region SelectCommand

    private CommandViewModelBase _SelectCommand;

    public CommandViewModelBase SelectCommand
        => _SelectCommand ??= CommandViewModel.CreateAsync(
            async () =>
            {
                var c = Code.Trim();
                if (string.IsNullOrEmpty(c))
                {
                    SelectedItem = null;
                    Focus();
                }
                else if (SelectedItem != null && GetMatchDistance(c, SelectedItem) == 0)
                {
                    Select(SelectedItem);
                }
                else
                {
                    if (!await SelectByCodeAsync(c, true).ConfigureAwait())
                    {
                        await Page.ShowErrorToastAsync("{0}'{1}'が見つかりませんでした。", EntityDisplayName, c).ConfigureAwait();
                    }
                }
            },
            style: BorderStyle.Primary,
            iconGetter: () => IsSearching ? "fas fa-pulse fa-spinner" : "fas fa-search");

    #endregion SelectCommand

    #region SelectOrClearCommand

    private CommandViewModelBase _SelectOrClearCommand;

    public CommandViewModelBase SelectOrClearCommand
        => _SelectOrClearCommand ??= CommandViewModel.Create(
            () =>
            {
                if (SelectedItem != null)
                {
                    SelectedItem = null;
                }
                else
                {
                    SelectCommand.Execute();
                }
            },
            iconGetter: () => SelectedItem != null ? "fas fa-times" : IsSearching ? "fas fa-pulse fa-spinner" : "fas fa-search");

    #endregion SelectOrClearCommand

    #region ClearCommand

    private CommandViewModelBase _ClearCommand;

    public CommandViewModelBase ClearCommand
        => _ClearCommand ??= CommandViewModel.Create(
            Clear,
            icon: "fas fa-times",
            style: BorderStyle.OutlineDanger,
            isVisibleGetter: () => IsEnabled && SelectedId != null);

    public void Clear()
    {
        _SelectedId = default;
        _SelectedItem = null;
        RaisePropertyChanged(nameof(SelectedId));
        RaisePropertyChanged(nameof(SelectedItem));
        _ClearCommand?.Invalidate();
    }

    #endregion ClearCommand

    #endregion 選択項目

    #region 検索

    #region IsSearching

    private int _IsSearchingCount;

    protected int IsSearchingCount
    {
        get => _IsSearchingCount;
        set
        {
            value = Math.Max(value, 0);
            if (value != _IsSearchingCount)
            {
                var wasSearching = IsSearching;
                _IsSearchingCount = value;
                if (wasSearching != IsSearching)
                {
                    RaisePropertyChanged(nameof(IsSearching));
                    _SelectCommand?.Invalidate();
                    _SelectOrClearCommand?.Invalidate();
                }
            }
        }
    }

    public bool IsSearching
        => _IsSearchingCount > 0;

    #endregion IsSearching

    Task<IEnumerable> IEntitySelector.SearchAsync(string query, int maxCount, CancellationToken cancellationToken)
        => SearchAsync(query, maxCount, cancellationToken)
            .ContinueWith(t => (IEnumerable)t.Result);

    public abstract Task<IEnumerable<TItem>> SearchAsync(string query, int maxCount, CancellationToken cancellationToken = default);

    public virtual async Task<TItem> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        var items = await SearchAsync("Id:" + id, 1, cancellationToken);
        return items.FirstOrDefault();
    }

    protected virtual Task<IReadOnlyList<TItem>> GetItemsAsync()
        => Task.FromResult<IReadOnlyList<TItem>>(Array.Empty<TItem>());

    public abstract Task<bool> SelectByCodeAsync(string code, bool isExactMatch = false);

    #endregion 検索

    #region モーダル

    public bool HasModal
        => CreateModalViewModel() is var m
        && m != null
        && Page.IsModalSupported(m.GetType());

    #region ShowModalCommand

    private CommandViewModelBase _ShowModalCommand;

    public CommandViewModelBase ShowModalCommand
        => _ShowModalCommand ??= CommandViewModel.Create(
            ShowModal,
            icon: "fas fa-ellipsis-h",
            style: BorderStyle.OutlineSecondary,
            isVisibleGetter: () => HasModal && IsEnabled);

    #endregion ShowModalCommand

    public async void ShowModal()
    {
        var m = CreateModalViewModel();
        if (m != null && Page.IsModalSupported(m.GetType()))
        {
            // TODO: suppresswarning
            try
            {
                await Page.OpenModalAsync(m);
            }
            catch { }
        }
    }

    protected virtual IFrameworkModalViewModel CreateModalViewModel()
        => null;

    #endregion モーダル

    #region IRequestFocus

    public event PropertyChangedEventHandler RequestFocus;

    public void Focus() => RequestFocus?.Invoke(this, new PropertyChangedEventArgs("."));

    #endregion IRequestFocus
}
