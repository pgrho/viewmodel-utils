namespace Shipwreck.ViewModelUtils.Components;

public class FormGroupTheme
{
    public static readonly FormGroupTheme Default = new FormGroupTheme();
    public string FormGroupClass { get; set; } = "form-group";
    public string LabelClass { get; set; } = "form-label";
    public string ControlClass { get; set; } = "form-control";
    public string SelectControlClass { get; set; } = "form-control custom-select";
    public string ErrorMessageClass { get; set; } = "field-validation-valid text-danger";
}
