using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Shipwreck.ViewModelUtils
{
    public static partial class TaskHelper
    {
        public static ConfiguredTaskAwaitable ConfigureAwait(this Task task)
            => task.ConfigureAwait(SHOULD_CAPTURE_CONTEXT);

        public static ConfiguredTaskAwaitable<TResult> ConfigureAwait<TResult>(this Task<TResult> task)
            => task.ConfigureAwait(SHOULD_CAPTURE_CONTEXT);
    }
}
