using System.Collections.Generic;
using System.Linq;

namespace Shipwreck.ViewModelUtils.Demo.PresentationFramework
{
    public sealed class MainWindowViewModel : WindowViewModel
    {
        private IEnumerable<CommandViewModelBase> _Commands;

        public IEnumerable<CommandViewModelBase> Commands
            => _Commands ??= CreateCommands().ToArray();

        private IEnumerable<CommandViewModelBase> CreateCommands()
        {
            yield return CommandViewModel.Create(
                () => new InteractionServiceWindow(new InteractionServiceWindowViewModel(new InteractionService()))
                {
                    Owner = Window
                }.Show(),
                title: "InteractionService",
                mnemonic: "A");

            yield return CommandViewModel.Create(
                () => new InteractionServiceWindow(new InteractionServiceWindowViewModel(new FrameworkInteractionService()))
                {
                    Owner = Window
                }.Show(),
                title: "FrameworkInteractionService",
                mnemonic: "F");

            yield return CommandViewModel.Create(
                () => new SearchPageWindow()
                {
                    Owner = Window,
                    DataContext = new SearchPageWindowViewModel()
                }.Show(),
                title: "SearchPage",
                mnemonic: "S");

            yield return CommandViewModel.Create(
                () => new ButtonsWindow()
                {
                    Owner = Window,
                    DataContext = new ButtonsWindowViewModel()
                }.Show(),
                title: "Buttons",
                mnemonic: "B");

            //yield return CommandViewModel.Create(
            //    async () =>
            //    {
            //        await ShowInformationToastAsync(typeof(QueryPropertyInfo).GetCustomAttribute<JsonConverterAttribute>()?.ConverterType?.ToString());

            //        var props = new QueryPropertyInfo[]
            //        {
            //            new DateTimeQueryPropertyInfo
            //            {
            //                TypeName = "DateTime",
            //                IsDate=true,
            //            },
            //            new EnumQueryPropertyInfo
            //            {
            //                TypeName = nameof(TypeCode),
            //                Fields=
            //                {
            //                    new EnumFieldInfo { Value = 0 },
            //                    new EnumFieldInfo { Value = 1 },
            //                }
            //            },
            //            new BooleanQueryPropertyInfo
            //            {
            //                TypeName = nameof(Boolean),
            //                TrueString = "T",
            //                FalseString = "F",
            //            },
            //            new QueryPropertyInfo
            //            {
            //                TypeName = nameof(Int32)
            //            }
            //        };

            //        await ShowInformationToastAsync(JsonConvert.SerializeObject(props));
            //    },
            //    title: "Client.Newtonsoft"
            //    );
        }
    }
}
