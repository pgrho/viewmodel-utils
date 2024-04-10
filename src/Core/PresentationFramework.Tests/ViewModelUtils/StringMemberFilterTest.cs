namespace Shipwreck.ViewModelUtils;

using D = InlineDataAttribute;

public class StringMemberFilterTest
{
    [Theory]
    [D(null, null, true)]
    [D("abc", null, true)]
    [D(null, "a", false)]
    [D(null, "!=a", true)]
    [D("abc", "a", true)]
    [D("ABC", "a", true)]
    [D("def", "a", false)]
    [D("abc", "*=a", true)]
    [D("ABC", "*=a", true)]
    [D("def", "*=a", false)]
    [D("a", "=a", true)]
    [D("A", "=a", true)]
    [D("abc", "=a", false)]
    [D("a", "!=a", false)]
    [D("A", "!=a", false)]
    [D("abc", "!=a", true)]
    [D("ccb", "<ccc", true)]
    [D("ccc", "<ccc", false)]
    [D("ccb", "<=ccc", true)]
    [D("ccc", "<=ccc", true)]
    [D("ccd", "<=ccc", false)]
    [D("ccc", ">ccc", false)]
    [D("ccd", ">ccc", true)]
    [D("ccb", ">=ccc", false)]
    [D("ccc", ">=ccc", true)]
    [D("ccd", ">=ccc", true)]
    [D("abcd", "^=ab", true)]
    [D("abcd", "^=bc", false)]
    [D("abcd", "^=cd", false)]
    [D("abcd", "$=ab", false)]
    [D("abcd", "$=bc", false)]
    [D("abcd", "$=cd", true)]
    [D("a", "|=b,c", false)]
    [D("b", "|=b,c", true)]
    [D("c", "|=b,c", true)]
    [D("d", "|=b,c", false)]
    public void Test(string testValue, string filter, bool isMatch)
        => Assert.Equal(
            isMatch,
            new StringMemberFilter<string>(e => e, _ => { })
            {
                Filter = filter
            }.IsMatch(testValue));
}
