namespace CalculatorForTests.Tests.Tests;

using Xunit;
using CalculatorForTests;
using System.Collections.Generic;

public class ParsedExpressionEvaluatorTests
{
    private readonly ParsedExpressionEvaluator evaluator = new ParsedExpressionEvaluator();

    [Theory]
    [InlineData(new[] { "2", "3", "+" }, 5)]
    [InlineData(new[] { "5", "2", "-" }, 3)]
    [InlineData(new[] { "4", "2", "*" }, 8)]
    [InlineData(new[] { "8", "4", "/" }, 2)]
    public void Evaluate_BasicOperations_ReturnsCorrectResults(string[] tokensArray, double expected)
    {
        var tokens = new List<string>(tokensArray);
        var result = evaluator.Evaluate(tokens);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Evaluate_NegativeNumbers_ReturnsCorrectResult()
    {
        var tokens = new List<string> { "-2", "3", "+" };
        var result = evaluator.Evaluate(tokens);
        Assert.Equal(1, result);
    }

    [Fact]
    public void Evaluate_ComplexExpression_ReturnsCorrectResult()
    {
        var tokens = new List<string> { "2", "3", "4", "*", "+", "5", "-" };
        var result = evaluator.Evaluate(tokens);
        Assert.Equal(9, result);
    }

    [Fact]
    public void Evaluate_ZeroOperands_ReturnsZero()
    {
        var tokens = new List<string> { "0", "0", "+" };
        var result = evaluator.Evaluate(tokens);
        Assert.Equal(0, result);
    }

    [Fact]
    public void Evaluate_LargeNumbers_ReturnsCorrectResult()
    {
        var tokens = new List<string> { "999999999", "1", "+" };
        var result = evaluator.Evaluate(tokens);
        Assert.Equal(1000000000, result);
    }

    [Theory]
    [InlineData(new[] { "+" }, "Недостаточно операндов")]
    [InlineData(new[] { "2", "+" }, "Недостаточно операндов")]
    [InlineData(new[] { "2", "3", "*", "+" }, "Недостаточно операндов")]
    public void Evaluate_InvalidTokens_ThrowsException(string[] tokensArray, string expectedMessagePart)
    {
        var tokens = new List<string>(tokensArray);
        var exception = Assert.Throws<InvalidOperationException>(() => evaluator.Evaluate(tokens));
        Assert.Contains(expectedMessagePart, exception.Message);
    }

    [Fact]
    public void Evaluate_DivideByZero_ThrowException()
    {
        var tokens = new List<string> { "1", "0", "/" };
        Assert.Throws<DivideByZeroException>(() => evaluator.Evaluate(tokens));
    }

    [Fact]
    public void Evaluate_InvalidToken_ThrowsException()
    {
        var tokens = new List<string> { "2", "a", "+" };
        Assert.Throws<InvalidOperationException>(() => evaluator.Evaluate(tokens));
    }

    [Fact]
    public void Evaluate_EmptyList_ThrowsException()
    {
        var tokens = new List<string>();
        Assert.Throws<InvalidOperationException>(() => evaluator.Evaluate(tokens));
    }
}