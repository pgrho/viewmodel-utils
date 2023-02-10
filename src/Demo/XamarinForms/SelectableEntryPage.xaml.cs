using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shipwreck.ViewModelUtils.Demo.XamarinForms;

public sealed class SelectableEntryPageViewModel : ObservableModel
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

    private bool _IsKeyboardEnabled =true;

    public bool  IsKeyboardEnabled
    {
        get => _IsKeyboardEnabled;
        set => SetProperty(ref _IsKeyboardEnabled, value);
    }

    #endregion
}

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SelectableEntryPage : ContentPage
{
    public SelectableEntryPage()
    {
        InitializeComponent();
        BindingContext = new SelectableEntryPageViewModel();
    }
}
