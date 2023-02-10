using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shipwreck.ViewModelUtils.Demo.XamarinForms;

public sealed class SelectableEntryPageViewModel : FrameworkPageViewModel
{
    #region Text

    private string _Text = "abcdefg";

    public string Text
    {
        get => _Text;
        set => SetProperty(ref _Text, value);
    }

    #endregion
    #region Placeholder

    private string _Placeholder = "placeholder";

    public string Placeholder
    {
        get => _Placeholder;
        set => SetProperty(ref _Placeholder, value);
    }

    #endregion

    #region IsKeyboardEnabled

    private bool _IsKeyboardEnabled = true;

    public bool IsKeyboardEnabled
    {
        get => _IsKeyboardEnabled;
        set => SetProperty(ref _IsKeyboardEnabled, value);
    }

    #endregion

    #region SelectAllOnFocus

    private bool _SelectAllOnFocus;

    public bool SelectAllOnFocus
    {
        get => _SelectAllOnFocus;
        set => SetProperty(ref _SelectAllOnFocus, value);
    }

    #endregion

    private CommandViewModelBase _FocusCommand;
    public CommandViewModelBase FocusCommand
        => _FocusCommand ??= CommandViewModel.Create(() => Focus(nameof(Text)), title: "Focus");

    private CommandViewModelBase _ExitCommand;
    public CommandViewModelBase ExitCommand
        => _ExitCommand ??= CommandViewModel.CreateAsync(() => Application.Current.MainPage.Navigation.PushAsync(new ContentPage()));

}

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SelectableEntryPage : FrameworkContentPage
{
    public SelectableEntryPage()
    {
        InitializeComponent();
        BindingContext = new SelectableEntryPageViewModel();
    }

    protected override void OnFocusRequested(string propertyName)
        => target.Focus();
}
