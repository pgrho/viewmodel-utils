namespace Shipwreck.ViewModelUtils.Controls;

/// <summary>
/// Paginator.xaml の相互作用ロジック
/// </summary>
public partial class Paginator
{
    private static readonly DependencyPropertyKey IsFilteredPropertyKey
      = DependencyProperty.RegisterReadOnly(nameof(IsFiltered), typeof(bool), typeof(Paginator), new FrameworkPropertyMetadata(false));

    public static readonly DependencyProperty TotalCountProperty
        = DependencyProperty.Register(nameof(TotalCount), typeof(int), typeof(Paginator), new FrameworkPropertyMetadata(0, OnPageChanged));

    public static readonly DependencyProperty FilteredCountProperty
        = DependencyProperty.Register(nameof(FilteredCount), typeof(int?), typeof(Paginator), new FrameworkPropertyMetadata(null, OnPageChanged));

    public static readonly DependencyProperty PageSizeProperty
        = DependencyProperty.Register(nameof(PageSize), typeof(int), typeof(Paginator), new FrameworkPropertyMetadata(0, OnPageChanged));

    public static readonly DependencyProperty PageIndexProperty
        = DependencyProperty.Register(nameof(PageIndex), typeof(int), typeof(Paginator), new FrameworkPropertyMetadata(0, OnPageChanged));

    public static readonly DependencyProperty IsFilteredProperty
       = IsFilteredPropertyKey.DependencyProperty;

    public static readonly DependencyProperty CommandsProperty
        = DependencyProperty.Register(
            nameof(Commands),
            typeof(IEnumerable<CommandViewModelBase>),
            typeof(Paginator),
            new FrameworkPropertyMetadata(Array.Empty<CommandViewModelBase>()));

    public Paginator()
    {
        InitializeComponent();
    }

    public int TotalCount
    {
        get => (int)GetValue(TotalCountProperty);
        set => SetValue(TotalCountProperty, value);
    }

    public int? FilteredCount
    {
        get => (int?)GetValue(FilteredCountProperty);
        set => SetValue(FilteredCountProperty, value);
    }

    public int PageSize
    {
        get => (int)GetValue(PageSizeProperty);
        set => SetValue(PageSizeProperty, value);
    }

    public int PageIndex
    {
        get => (int)GetValue(PageIndexProperty);
        set => SetValue(PageIndexProperty, value);
    }

    public bool IsFiltered
    {
        get => (bool)GetValue(IsFilteredProperty);
        private set => SetValue(IsFilteredPropertyKey, value);
    }

    public BulkUpdateableCollection<PaginatorLinkModel> Links { get; }
        = new BulkUpdateableCollection<PaginatorLinkModel>();

    public IEnumerable<CommandViewModelBase> Commands
    {
        get => (IEnumerable<CommandViewModelBase>)GetValue(CommandsProperty);
        set => SetValue(CommandsProperty, value);
    }

    #region MoveToCommand

    private sealed class MoveToCommandImpl : ICommand
    {
        private readonly Paginator _Paginator;

        public MoveToCommandImpl(Paginator paginator)
        {
            _Paginator = paginator;
        }

        event EventHandler ICommand.CanExecuteChanged { add { } remove { } }

        public bool CanExecute(object parameter)
            => parameter is PaginatorLinkModel && _Paginator.DataContext is ISortablePageViewModel;

        public void Execute(object parameter)
        {
            if (parameter is PaginatorLinkModel l && _Paginator.DataContext is ISortablePageViewModel p)
            {
                p.NavigateTo(l.Index);
            }
        }
    }

    private ICommand _MoveToCommand;

    public ICommand MoveToCommand => _MoveToCommand ??= new MoveToCommandImpl(this);

    #endregion MoveToCommand

    private static void OnPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Paginator p)
        {
            p.IsFiltered = 0 <= p.FilteredCount && p.FilteredCount < p.TotalCount;

            var Count = p.FilteredCount ?? p.TotalCount;

            var links = new List<PaginatorLinkModel>();

            if (p.PageSize > 0 && Count > p.PageSize)
            {
                var pMax = (Count - 1) / p.PageSize;
                const int R = 5;

                links.Add(new PaginatorLinkModel(0, PaginatorLinkType.First));
                links.Add(new PaginatorLinkModel(Math.Max(0, p.PageIndex - 1), PaginatorLinkType.Previous));

                for (var di = -R; di <= R; di++)
                {
                    var i = p.PageIndex + di;
                    if (0 <= i && i <= pMax)
                    {
                        links.Add(new PaginatorLinkModel(i, isActive: i == p.PageIndex));
                    }
                }

                links.Add(new PaginatorLinkModel(Math.Min(pMax, p.PageIndex + 1), PaginatorLinkType.Next));
                links.Add(new PaginatorLinkModel(pMax, PaginatorLinkType.Last));
            }

            for (var i = 0; i < links.Count; i++)
            {
                var item = links[i];
                links[i] = p.Links.FirstOrDefault(e => e.Equals(item)) ?? item;
            }

            if (!links.SequenceEqual(p.Links))
            {
                p.Links.Set(links);
            }
        }
    }
}
