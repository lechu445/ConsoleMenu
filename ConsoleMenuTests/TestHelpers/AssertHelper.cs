using Xunit;

namespace ConsoleMenuTests.TestHelpers
{
  public static class AssertHelper
  {
    public static void Equal(string expected, string actual)
    {
      try
      {
        Assert.Equal(expected, actual, ignoreLineEndingDifferences: true);
      }
      catch (Xunit.Sdk.EqualException ex)
      {
        throw new Xunit.Sdk.XunitException("Expected was not equal to actual", ex);
      }
    }
  }
}
