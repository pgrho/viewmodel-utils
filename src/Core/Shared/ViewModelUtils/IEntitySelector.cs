using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Shipwreck.ViewModelUtils
{
    public interface IEntitySelector : INotifyPropertyChanged, IRequestFocus
    {
        FrameworkPageViewModel Page { get; }
        bool HasModal { get; }

        bool UseList { get; }
        bool IsEnabled { get; }
        bool IsSearching { get; }

        object SelectedItem { get; set; }
        object SelectedId { get; set; }

        string Code { get; set; }

        CommandViewModelBase SelectCommand { get; }
        CommandViewModelBase ClearCommand { get; }
        CommandViewModelBase SelectOrClearCommand { get; }
        CommandViewModelBase ShowModalCommand { get; }

        bool IsValid(object id);

        string GetCode(object item);

        string GetName(object item);

        string GetDisplayText(object item);

        int GetMatchDistance(string code, object item);
        object GetById(object id); 

        void ShowModal();

        void Select(object item);

        Task<bool> SelectByCodeAsync(string code, bool isExactMatch = false);

        void Clear();

        void Focus();

        Task<IEnumerable> SearchAsync(string query, int maxCount, CancellationToken cancellationToken = default);

        Task<IList> GetItemsTask();
    }

    public interface IEntitySelector<TId, TItem> : IEntitySelector
        where TItem : class
    {
        BulkUpdateableCollection<TItem> Items { get; }

        new TItem SelectedItem { get; set; }
        new TId SelectedId { get; set; }

        string GetCode(TItem item);

        string GetName(TItem item);

        string GetDisplayText(TItem item);

        new Task<IEnumerable<TItem>> SearchAsync(string query, int maxCount, CancellationToken cancellationToken = default);

        new Task<IReadOnlyList<TItem>> GetItemsTask();

        int GetMatchDistance(string code, TItem item);

        void Select(TItem item);
    }
}
