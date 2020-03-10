using ESFA.DC.Operations.Reports.Service.Providers;
using Xunit;

namespace ESFA.DC.Operations.Reports.Service.Tests
{
    public class UpinValidatorTests
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
        public void UpinValidator_IsValid(string valueToCheck, bool expectedresult)
        {
            // Act
            var result = UpinValidator.IsValid(valueToCheck);

            // Assert
            Assert.Equal(result, expectedresult);
        }
    }
}