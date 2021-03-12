using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Shipwreck.ViewModelUtils.Demo.PresentationFramework
{
    public sealed class InteractionServiceWindowViewModel : WindowViewModel
    {
        public InteractionServiceWindowViewModel(InteractionService interaction)
        {
            interaction.RegisterModal(typeof(CalculatorModalViewModel), vm => new CalculatorControl() { DataContext = vm });

            Interaction = interaction;
            Logs = new BulkUpdateableCollection<LogViewModel>();
            BindingOperations.EnableCollectionSynchronization(Logs, ((ICollection)Logs).SyncRoot);
        }

        public BulkUpdateableCollection<LogViewModel> Logs { get; }

        private IEnumerable<CommandViewModelBase> _Commands;

        public IEnumerable<CommandViewModelBase> Commands
            => _Commands ??= CreateCommands().ToList();

        private IEnumerable<CommandViewModelBase> CreateCommands()
        {
            var toastFuncs = new Func<string, string, Task>[]
            {
                ShowSuccessToastAsync,
                ShowInformationToastAsync,
                ShowWarningToastAsync,
                ShowErrorToastAsync,
            };

            foreach (var tf in toastFuncs)
            {
                yield return CreateToastCommand(tf, true);
                yield return CreateToastCommand(tf, false);
            }

            yield return CreateAlertCommand(true);
            yield return CreateAlertCommand(false);

            yield return CreateConfirmCommand(true);
            yield return CreateConfirmCommand(false);

            yield return CreateOpenFileCommand(true);
            yield return CreateOpenFileCommand(false);

            yield return CreateOpenFilesCommand(true);
            yield return CreateOpenFilesCommand(false);

            yield return CreateSaveFileCommand(true);
            yield return CreateSaveFileCommand(false);

            yield return CreateOpenDirectoryCommand(true);
            yield return CreateOpenDirectoryCommand(false);

            yield return CreateSaveDirectoryCommand(true);
            yield return CreateSaveDirectoryCommand(false);

            yield return CreateModalCommand(true);
            yield return CreateModalCommand(false);
        }

        private CommandViewModelBase CreateToastCommand(Func<string, string, Task> toastFunc, bool capture)
        {
            var fn = toastFunc.Method.Name;
            return CommandViewModel.CreateAsync(async () =>
            {
                Log("Begin CreateToastCommand ({0})", capture);
                await Task.Delay(250).ConfigureAwait(capture);
                Log("Calling {0}", fn);
                await toastFunc(fn, nameof(CreateToastCommand)).ConfigureAwait(capture);
                Log("Called {0}", fn);
                Log("End CreateToastCommand ({0})", capture);
            }, title: $"{fn} ({capture})");
        }

        private CommandViewModelBase CreateAlertCommand(bool capture)
            => CommandViewModel.Create(async () =>
            {
                Log("Begin CreateAlertCommand ({0})", capture);
                await Task.Delay(250).ConfigureAwait(capture);
                Log("Calling {0}", nameof(AlertAsync));
                await AlertAsync("Alert", title: nameof(CreateAlertCommand), buttonText: "I got it").ConfigureAwait(capture);
                Log("Called {0}", nameof(AlertAsync));
                Log("End CreateAlertCommand ({0})", capture);
            }, title: $"AlertAsync ({capture})");

        private CommandViewModelBase CreateConfirmCommand(bool capture)
            => CommandViewModel.Create(async () =>
            {
                Log("Begin CreateConfirmCommand ({0})", capture);
                await Task.Delay(250).ConfigureAwait(capture);
                Log("Calling {0}", nameof(ConfirmAsync));
                var r = await ConfirmAsync("Confirm", title: nameof(CreateConfirmCommand), trueText: "Accept", falseText: "decline").ConfigureAwait(capture);
                Log("Called {0}", nameof(ConfirmAsync));
                Log("Result: {0}", r);
                Log("End CreateConfirmCommand ({0})", capture);
            }, title: $"ConfirmAsync ({capture})");

        private CommandViewModelBase CreateOpenFileCommand(bool capture)
            => CommandViewModel.Create(async () =>
            {
                Log("Begin CreateOpenFileCommand ({0})", capture);
                await Task.Delay(250).ConfigureAwait(capture);
                Log("Calling {0}", nameof(OpenFileAsync));
                var r = await OpenFileAsync(
                    filter: "Text|*.txt|CSV|*.csv|All|*",
                    filterIndex: 2,
                    fileName: "filename",
                    initialDirectory: "C:\\").ConfigureAwait(capture);
                Log("Called {0}", nameof(OpenFileAsync));
                Log("Result: {0}", r);
                Log("End CreateOpenFileCommand ({0})", capture);
            }, title: $"OpenFileAsync ({capture})");

        private CommandViewModelBase CreateOpenFilesCommand(bool capture)
            => CommandViewModel.Create(async () =>
            {
                Log("Begin CreateOpenFilesCommand ({0})", capture);
                await Task.Delay(250).ConfigureAwait(capture);
                Log("Calling {0}", nameof(OpenFilesAsync));
                var r = await OpenFilesAsync(
                    filter: "Text|*.txt|CSV|*.csv|All|*",
                    filterIndex: 2,
                    fileName: "filename",
                    initialDirectory: "C:\\",
                    multiSelect: true).ConfigureAwait(capture);
                Log("Called {0}", nameof(OpenFilesAsync));
                Log("Result: {0}", r == null ? null : string.Join(",", r));
                Log("End CreateOpenFilesCommand ({0})", capture);
            }, title: $"OpenFilesAsync ({capture})");

        private CommandViewModelBase CreateSaveFileCommand(bool capture)
            => CommandViewModel.Create(async () =>
            {
                Log("Begin CreateSaveFileCommand ({0})", capture);
                await Task.Delay(250).ConfigureAwait(capture);
                Log("Calling {0}", nameof(SaveFileAsync));
                var r = await SaveFileAsync(
                    filter: "Text|*.txt|CSV|*.csv|All|*",
                    filterIndex: 2,
                    fileName: "filename",
                    initialDirectory: "C:\\").ConfigureAwait(capture);
                Log("Called {0}", nameof(SaveFileAsync));
                Log("Result: {0}", r);
                Log("End CreateSaveFileCommand ({0})", capture);
            }, title: $"SaveFileAsync ({capture})");

        private CommandViewModelBase CreateOpenDirectoryCommand(bool capture)
            => CommandViewModel.Create(async () =>
            {
                Log("Begin CreateOpenDirectoryCommand ({0})", capture);
                await Task.Delay(250).ConfigureAwait(capture);
                Log("Calling {0}", nameof(OpenDirectoryAsync));
                var r = await OpenDirectoryAsync(directoryName: "hoge").ConfigureAwait(capture);
                Log("Called {0}", nameof(OpenDirectoryAsync));
                Log("Result: {0}", r);
                Log("End CreateOpenDirectoryCommand ({0})", capture);
            }, title: $"OpenDirectoryAsync ({capture})");

        private CommandViewModelBase CreateSaveDirectoryCommand(bool capture)
            => CommandViewModel.Create(async () =>
            {
                Log("Begin CreateSaveDirectoryCommand ({0})", capture);
                await Task.Delay(250).ConfigureAwait(capture);
                Log("Calling {0}", nameof(SaveDirectoryAsync));
                var r = await SaveDirectoryAsync("fuga").ConfigureAwait(capture);
                Log("Called {0}", nameof(SaveDirectoryAsync));
                Log("Result: {0}", r);
                Log("End CreateSaveDirectoryCommand ({0})", capture);
            }, title: $"SaveDirectoryAsync ({capture})");

        private CommandViewModelBase CreateModalCommand(bool capture)
            => CommandViewModel.Create(async () =>
            {
                Log("Begin CreateModalCommand ({0})", capture);
                await Task.Delay(250).ConfigureAwait(capture);
                Log("Calling {0}", nameof(OpenModalAsync));
                var vm = new CalculatorModalViewModel();
                await OpenModalAsync(vm).ConfigureAwait(capture);
                Log("Called {0}", nameof(OpenModalAsync));
                Log("Result: {0}", vm.Input);
                Log("End CreateModalCommand ({0})", capture);
            }, title: $"OpenModalAsync ({capture})");

        public void Log(string message)
        {
            lock (((ICollection)Logs).SyncRoot)
            {
                Logs.Insert(0, new LogViewModel(message));
            }
        }

        public void Log(string format, params object[] args)
            => Log(string.Format(format, args));
    }
}
