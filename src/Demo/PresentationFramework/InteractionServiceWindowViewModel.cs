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
