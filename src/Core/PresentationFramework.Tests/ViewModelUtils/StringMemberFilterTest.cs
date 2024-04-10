namespace Shipwreck.ViewModelUtils;

public class StringMemberFilterTest
{
    [Theory]
    [InlineData(null, null, true)]
    [InlineData("abc", null, true)]
    [InlineData(null, "a", false)]
    [InlineData(null, "!=a", true)]
    [InlineData("abc", "a", true)]
    [InlineData("ABC", "a", true)]
    [InlineData("def", "a", false)]
    [InlineData("abc", "*=a", true)]
    [InlineData("ABC", "*=a", true)]
    [InlineData("def", "*=a", false)]
    [InlineData("a", "=a", true)]
    [InlineData("A", "=a", true)]
    [InlineData("abc", "=a", false)]
    [InlineData("a", "!=a", false)]
    [InlineData("A", "!=a", false)]
    [InlineData("abc", "!=a", true)]
    [InlineData("ccb", "<ccc", true)]
    [InlineData("ccc", "<ccc", false)]
    [InlineData("ccb", "<=ccc", true)]
    [InlineData("ccc", "<=ccc", true)]
    [InlineData("ccd", "<=ccc", false)]
    [InlineData("ccc", ">ccc", false)]
    [InlineData("ccd", ">ccc", true)]
    [InlineData("ccb", ">=ccc", false)]
    [InlineData("ccc", ">=ccc", true)]
    [InlineData("ccd", ">=ccc", true)]
    [InlineData("abcd", "^=ab", true)]
    [InlineData("abcd", "^=bc", false)]
    [InlineData("abcd", "^=cd", false)]
    [InlineData("abcd", "$=ab", false)]
    [InlineData("abcd", "$=bc", false)]
    [InlineData("abcd", "$=cd", true)]
    [InlineData("a", "|=b,c", false)]
    [InlineData("b", "|=b,c", true)]
    [InlineData("c", "|=b,c", true)]
    [InlineData("d", "|=b,c", false)]
    public void Test(string testValue, string filter, bool isMatch)
        => Assert.Equal(
            isMatch,
            new StringMemberFilter<string>(e => e, _ => { })
            {
                Filter = filter
            }.IsMatch(testValue));
}
