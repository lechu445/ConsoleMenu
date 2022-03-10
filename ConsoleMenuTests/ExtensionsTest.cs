using System.Collections.Generic;
using ConsoleTools;
using Xunit;

namespace ConsoleMenuTests
{
  public class ExtensionsTest
  {
    [Theory]
    [InlineData("")]
    [InlineData(".", "")]
    [InlineData("''", "''")]
    [InlineData("1.2.3", "1", "2", "3")]
    [InlineData("1.'somet.hing'.3", "1", "'somet.hing'", "3")]
    [InlineData("1.''.3", "1", "''", "3")]
    public void SplitItems_Test(string input, params string[] expected)
    {
      List<string> actual = Extensions.SplitItems(input, '.', '\'');
      Assert.Equal(expected, actual);
    }
  }
}
