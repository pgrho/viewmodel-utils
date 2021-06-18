namespace Shipwreck.ViewModelUtils.Demo.PresentationFramework
{
    public sealed class ButtonsWindowViewModel : WindowViewModel
    {
        #region DropDownButtons

        #region SingleCommandCollection

        private CommandViewModelCollection _SingleCommandCollection;

        public CommandViewModelCollection SingleCommandCollection
            => _SingleCommandCollection ??= new CommandViewModelCollection(new[]
            {
                CommandViewModel.Create(() => ShowSuccessToastAsync("Single"), "Single", style: BorderStyle.Primary)
            });

        #endregion SingleCommandCollection

        #region MultipleCommandCollection

        private CommandViewModelCollection _MultipleCommandCollection;

        public CommandViewModelCollection MultipleCommandCollection
            => _MultipleCommandCollection ??= new CommandViewModelCollection(new[]
            {
                CommandViewModel.Create(() => ShowSuccessToastAsync("Primary"), "Primary", style: BorderStyle.Primary),
                CommandViewModel.Create(() => ShowSuccessToastAsync("Secondary"), "Secondary", style: BorderStyle.Secondary)
            });

        #endregion MultipleCommandCollection

        #endregion DropDownButtons
    }
}
