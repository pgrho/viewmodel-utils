using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Shipwreck.ViewModelUtils.Controls;
using System.Net.Http;
using Microsoft.WindowsAPICodePack.Shell;
using System.IO;

#if NET5_0
using Notifications.Wpf.Core;
using Notifications.Wpf.Core.Controls;
#else

using Notifications.Wpf;
using Notifications.Wpf.Controls;

#endif

namespace Shipwreck.ViewModelUtils
{
    public class FrameworkInteractionService : InteractionService, IInteractionService
    {
        public static string NotificationAreaName { get; } = "NotificationArea";

        private NotificationManager _NotificationManager;

        protected NotificationManager NotificationManager
            => _NotificationManager ??= new NotificationManager();

        public TimeSpan? ToastExpirationTime { get; } = TimeSpan.FromSeconds(3);

        protected override void ShowToast(object context, string message, string title, BorderStyle style)
        {
            var w = GetWindow(context);
            if (w != null)
            {
                var na = (w as FrameworkWindow)?.GetNotificationArea()
                    ?? w.FindName(NotificationAreaName) as NotificationArea;

                if (na != null)
                {
                    var content = new NotificationContent()
                    {
                        Type = (style & BorderStyle.Success) != 0 ? NotificationType.Success
                            : (style & BorderStyle.Danger) != 0 ? NotificationType.Error
                            : (style & BorderStyle.Warning) != 0 ? NotificationType.Warning
                            : NotificationType.Information,
                        Title = title,
                        Message = message
                    };

#if NET5_0
                    NotificationManager.ShowAsync(content, expirationTime: ToastExpirationTime, areaName: na.BindableName.EmptyToNull() ?? na.Name).GetHashCode();
#else
                    NotificationManager.Show(content, expirationTime: ToastExpirationTime, areaName: na.Name);
#endif

                    return;
                }
            }

            base.ShowToast(context, message, title, style);
        }

        public override Task<MessageBoxResult> ShowMessageBoxAsync(object context, string message, string title, string trueText, BorderStyle? trueStyle, string falseText, BorderStyle? falseStyle, MessageBoxButton button, MessageBoxImage icon)
        {
            if (GetWindow(context) is MetroWindow mw)
            {
                Task<MessageBoxResult> showCore()
                {
                    if (DialogParticipation.GetRegister(mw) != null)
                    {
                        switch (button)
                        {
                            case MessageBoxButton.YesNo:
                                return mw.ShowMessageAsync(title, message, MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
                                {
                                    AffirmativeButtonText = !string.IsNullOrEmpty(trueText) ? trueText : "はい",
                                    NegativeButtonText = !string.IsNullOrEmpty(falseText) ? falseText : "いいえ",
                                    OwnerCanCloseWithDialog = true
                                }).ContinueWith(t => t.Status != TaskStatus.RanToCompletion ? MessageBoxResult.None : t.Result == MessageDialogResult.Affirmative ? MessageBoxResult.Yes : MessageBoxResult.No);

                            case MessageBoxButton.OK:
                                return mw.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative, new MetroDialogSettings
                                {
                                    AffirmativeButtonText = !string.IsNullOrEmpty(trueText) ? trueText : "OK",
                                    OwnerCanCloseWithDialog = true
                                }).ContinueWith(t => t.Status != TaskStatus.RanToCompletion ? MessageBoxResult.None : MessageBoxResult.OK);
                        }
                    }

                    return base.ShowMessageBoxAsync(context, message, title, trueText, trueStyle, falseText, falseStyle, button, icon);
                }

                if (mw.Dispatcher.Thread != Thread.CurrentThread)
                {
                    var tcs = new TaskCompletionSource<MessageBoxResult>();
                    InvokeAsync(context, () => showCore().ContinueWith(t =>
                    {
                        if (t.Status == TaskStatus.RanToCompletion)
                        {
                            tcs.TrySetResult(t.Result);
                        }
                        else if (t.Exception != null)
                        {
                            tcs.TrySetException(t.Exception);
                        }
                        else
                        {
                            tcs.TrySetCanceled();
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext()));

                    return tcs.Task;
                }
                return showCore();
            }

            return base.ShowMessageBoxAsync(context, message, title, trueText, trueStyle, falseText, falseStyle, button, icon);
        }

        protected override Task ShowModalAsync(object context, FrameworkElement frameworkElement)
        {
            if (GetWindow(context) is MetroWindow mw)
            {
                Task showCore()
                {
                    if (DialogParticipation.GetRegister(mw) != null
                        && !(frameworkElement is Window))
                    {
                        ConfigureViewModel(frameworkElement.DataContext);

                        var tcs = new TaskCompletionSource<object>();

                        if (frameworkElement is BaseMetroDialog d)
                        {
                            d.Unloaded += (s, e) => tcs.TrySetResult(null);
                            mw.ShowMetroDialogAsync(d);
                        }
                        else
                        {
                            var gd = new GenericMetroDialog()
                            {
                                DataContext = frameworkElement.DataContext,
                                Content = frameworkElement,
                            };
                            gd.Unloaded += (s, e) => tcs.TrySetResult(null);
                            mw.ShowMetroDialogAsync(gd);
                        }
                        return tcs.Task;
                    }
                    return base.ShowModalAsync(context, frameworkElement);
                }

                if (mw.Dispatcher.Thread != Thread.CurrentThread)
                {
                    return InvokeAsync(context, showCore);
                }
                return showCore();
            }
            return base.ShowModalAsync(context, frameworkElement);
        }

        protected override bool CloseModal(object context, object viewModel)
        {
            var app = Application.Current;
            if (app != null)
            {
                foreach (var mw in app.Windows.OfType<MetroWindow>().ToList())
                {
                    if (DialogParticipation.GetRegister(mw) != null)
                    {
                        var dc = mw.FindChild<Panel>("PART_MetroActiveDialogContainer");
                        if (dc != null)
                        {
                            foreach (var d in dc.Children.OfType<BaseMetroDialog>().Where(e => e.DataContext == viewModel).ToList())
                            {
                                mw.HideMetroDialogAsync(d).GetHashCode();
                            }
                        }
                    }
                }
            }
            return base.CloseModal(context, viewModel);
        }

        protected override Task<string> GetDownloadPathAsync(HttpResponseMessage response, string coreFileName, bool openFile)
        {
            if (!string.IsNullOrEmpty(coreFileName))
            {
                var dp = KnownFolders.Downloads?.Path;
                if (dp != null)
                {
                    return Task.FromResult(Path.Combine(dp, coreFileName));
                }
            }

            return base.GetDownloadPathAsync(response, coreFileName, openFile);
        }
    }
}
