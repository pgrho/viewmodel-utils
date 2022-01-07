using Shipwreck.ViewModelUtils.Searching;

namespace Shipwreck.ViewModelUtils;

public static class ConditionViewModelExtensions
{
    public static void SetYear(this ConditionViewModel c, DateTime v)
    {
        if (c is DateTimeConditionViewModel d)
        {
            d.Operator = DateTimeConditionViewModel.YearOperator.Token;
            d.Value = new DateTime(v.Year, 1, 1);
        }
    }
    public static void SetYear(this ConditionViewModel c, int v)
    {
        if (c is DateTimeConditionViewModel d)
        {
            d.Operator = DateTimeConditionViewModel.YearOperator.Token;
            d.Value = new DateTime(v, 1, 1);
        }
    }
    public static void SetMonth(this ConditionViewModel c, DateTime v)
    {
        if (c is DateTimeConditionViewModel d)
        {
            d.Operator = DateTimeConditionViewModel.MonthOperator.Token;
            d.Value = new DateTime(v.Year, v.Month, 1);
        }
    }
    public static void SetDate(this ConditionViewModel c, DateTime v)
    {
        if (c is DateTimeConditionViewModel d)
        {
            d.Operator = DateTimeConditionViewModel.DateOperator.Token;
            d.Value = v.Date;
        }
    }
}
