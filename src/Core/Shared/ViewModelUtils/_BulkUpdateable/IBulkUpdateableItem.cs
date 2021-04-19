using System.Threading.Tasks;

namespace Shipwreck.ViewModelUtils
{
    public interface IBulkUpdateableItem
    {
        bool IsNew { get; }
        bool ShouldSave();
        void CancelEdit();
        Task<bool> SaveAsync(bool forceUpdate = false);
    }
}
