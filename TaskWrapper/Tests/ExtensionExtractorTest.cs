using Xunit;

namespace TaskWrapper.Tests;

public class ExtensionExtractorTest
{
    [Theory]
    [InlineData("file.sh", "bash ")]
    [InlineData("file.cs", "dotnet run ")]
    [InlineData("script.py", "python3 ")]
    [InlineData("program.go", "go run ")]
    public void Extract_ShouldReturnCorrectCommand_ForSupportedExtensions(string path, string expectedCommand)
    {
        // Arrange
        var extractor = new ExtensionExtractor(path);

        // Act
        var result = extractor.Extract();

        // Assert
        Assert.Equal(expectedCommand, result);
    }

    [Fact]
    public void Extract_ShouldThrowException_ForUnsupportedExtension()
    {
        // Arrange
        var extractor = new ExtensionExtractor("file.txt");

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => extractor.Extract());
        Assert.Equal("Extension txt is not supported", exception.Message);
    }
}