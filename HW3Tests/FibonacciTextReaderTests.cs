using HW3.Models;

namespace HW3Tests;

public class FibonacciTextReaderTests
{
    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public void ReadLine_Returns_First_Two_Numbers()
    {
        // Arrange
        var reader = new FibonacciTextReader(2);

        // Act
        var firstNumber = reader.ReadLine();
        var secondNumber = reader.ReadLine();
        Assert.Multiple(() =>
        {

            // Assert
            Assert.That(firstNumber, Is.EqualTo("0"));
            Assert.That(secondNumber, Is.EqualTo("1"));
        });
    }

    [Test]
    public void ReadLine_Returns_Correct_Fibonacci_Sequence()
    {
        // Arrange
        const int maxLines = 10; // Assuming we want to generate the first 10 Fibonacci numbers
        var reader = new FibonacciTextReader(maxLines);
        string[] expectedNumbers = ["0", "1", "1", "2", "3", "5", "8", "13", "21", "34"];

        // Act
        var actualNumbers = new string[maxLines];
        for (var i = 0; i < maxLines; i++)
        {
            actualNumbers[i] = reader.ReadLine() ?? throw new InvalidOperationException();
        }

        // Assert
        Assert.That(actualNumbers, Is.EqualTo(expectedNumbers));
    }
    
    [Test]
    public void ReadLine_Returns_Null_If_Reader_Is_Empty()
    {
        // Arrange
        var reader = new FibonacciTextReader(0); // Empty reader

        // Act
        var result = reader.ReadLine();

        // Assert
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public void ReadLine_Returns_Null_After_MaxLines_Calls()
    {
        // Arrange
        const int maxLines = 5; // Assuming we want to generate the first 5 Fibonacci numbers
        var reader = new FibonacciTextReader(maxLines);

        // Act
        for (var i = 0; i < maxLines; i++)
        {
            reader.ReadLine();
        }
        var result = reader.ReadLine();

        // Assert
        Assert.That(result, Is.Null);
    }
}