namespace Shipwreck.ViewModelUtils;

public static partial class TaskHelper
{
    public static ConfiguredTaskAwaitable ConfigureAwait(this Task task)
        => task.ConfigureAwait(FrameworkPageViewModel.ShouldCaptureContext);

    public static ConfiguredTaskAwaitable<TResult> ConfigureAwait<TResult>(this Task<TResult> task)
        => task.ConfigureAwait(FrameworkPageViewModel.ShouldCaptureContext);

#if NET9_0_OR_GREATER
    public static ConfiguredValueTaskAwaitable ConfigureAwait(this ValueTask task)
        => task.ConfigureAwait(FrameworkPageViewModel.ShouldCaptureContext);

    public static ConfiguredValueTaskAwaitable<TResult> ConfigureAwait<TResult>(this ValueTask<TResult> task)
        => task.ConfigureAwait(FrameworkPageViewModel.ShouldCaptureContext);
#endif
}
