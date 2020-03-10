using ESFA.DC.Operations.Reports.Service.Providers;
using Xunit;

namespace ESFA.DC.Operations.Reports.Service.Tests
{
    public class OrgProviderServiceTests
    {
        [Theory]
        [InlineData("123456", true)]
        [InlineData("ABCDEF", false)]
        [InlineData("123456A", false)]
        [InlineData("A123456", false)]
        [InlineData("12345", false)]
        [InlineData("A12345", false)]
        [InlineData("12345A", false)]
        [InlineData("1234567", false)]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData(" ", false)]
        public void IsValidUpin(string valueToCheck, bool expectedResult)
        {
            // Arrange
            var sut = new OrgProviderService(null);

            // Act
            var result = sut.IsValidUpin(valueToCheck);

            // Assert
            Assert.Equal(result, expectedResult);
        }
    }
}