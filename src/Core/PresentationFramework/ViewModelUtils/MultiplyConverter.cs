namespace Shipwreck.ViewModelUtils;

public sealed class MultiplyConverter : ArithmeticConverter
{
    [DefaultValue(1.0)]
    public override double Operand { get; set; } = 1;

    protected override double Calculate(double x, double y)
        => x * y;
}
