namespace Shipwreck.ViewModelUtils;

using System.ComponentModel.DataAnnotations;
using D = InlineDataAttribute;

public sealed class EnumMemberFilterTest
{
    private const string ZERO_NAME = "ゼロ";
    private const string ONE_NAME = "ワン";
    private const string TWO_NAME = "ツー";

    public enum TestEnum
    {
        [Display(Name = ZERO_NAME)]
        Zero = 0,

        [Display(Name = ONE_NAME)]
        One = 1,

        [Display(Name = TWO_NAME)]
        Two = 2,
    }

    [Theory]
    [D(null, null, true)]
    [D(TestEnum.Zero, null, true)]
    [D(null, "null", true)]
    [D(TestEnum.Zero, "0", true)]
    [D(TestEnum.Zero, nameof(TestEnum.Zero), true)]
    [D(TestEnum.Zero, ZERO_NAME, true)]
    [D(TestEnum.One, "1", true)]
    [D(TestEnum.One, nameof(TestEnum.One), true)]
    [D(TestEnum.One, ONE_NAME, true)]
    [D(TestEnum.Two, "2", true)]
    [D(TestEnum.Two, nameof(TestEnum.Two), true)]
    [D(TestEnum.Two, TWO_NAME, true)]
    public void Test(TestEnum? testValue, string filter, bool isMatch)
    {
        var f =
        new EnumMemberFilter<TestEnum?, TestEnum>(e => e, _ => { })
        {
            Filter = filter
        };

        Assert.Equal(isMatch, f.IsMatch(testValue));

        if (!string.IsNullOrWhiteSpace(filter)
            && filter.IndexOf(',') < 0
            && isMatch)
        {
            foreach (var ov in new TestEnum?[] { TestEnum.Zero, TestEnum.One, TestEnum.Two, null })
            {
                if (ov != testValue)
                {
                    Assert.False(f.IsMatch(ov));
                }
            }
        }
    }
}
