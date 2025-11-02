namespace CalculatorForTests.Tests.Tests;

using CalculatorForTests;
using Serilog;
using System.Collections.Generic;
using Xunit;

public class ExpressionParserTests : BaseTest
{
    private readonly ExpressionParser parser = new ExpressionParser();

    [Fact]
    public void ParseExpression_SimpleAddition_ReturnsCorrectTokens()
    {
        Log.Information("Тест выполняется");
        var expression = "2+3";
        var result = parser.ParseExpression(expression);
        Assert.Equal(new List<string> { "2", "3", "+" }, result);
    }

    [Fact]
    public void ParseExpression_WithRoundBrackets_ReturnsCorrectTokens()
    {
        Log.Information("Тест выполняется");
        var expression = "(1+2)*3";
        var result = parser.ParseExpression(expression);
        Assert.Equal(new List<string> { "1", "2", "+", "3", "*" }, result);
    }

    [Fact]
    public void ParseExpression_UnbalancedRoundBrackets_ThrowsException()
    {
        Log.Information("Тест выполняется");
        var expression = "(1+2";
        Assert.Throws<InvalidOperationException>(() => parser.ParseExpression(expression));
    }

    [Fact]
    public void ParseExpression_ComplexExpression_ReturnsCorrectTokens()
    {
        Log.Information("Тест выполняется");
        var expression = "3 + 4 * (2 - 1) ^ 2 / 5.5";
        var expectedTokens = new List<string> { "3", "4", "2", "1", "-", "2", "^", "*", "5.5", "/", "+" };
        var result = parser.ParseExpression(expression);
        Assert.Equal(expectedTokens, result);
    }

    [Fact]
    public void ParseExpression_NumberWithDecimal_ReturnsCorrectToken()
    {
        Log.Information("Тест выполняется");
        var expression = "3.1415 + 2.718";
        var expectedTokens = new List<string> { "3.1415", "2.718", "+" };
        var result = parser.ParseExpression(expression);
        Assert.Equal(expectedTokens, result);
    }

    [Fact]
    public void ParseExpression_OperatorsPrecedence_ReturnsCorrectOrder()
    {
        Log.Information("Тест выполняется");
        var expression = "2 + 3 * 4 ^ 2";
        var expectedTokens = new List<string> { "2", "3", "4", "2","^", "*", "+"};
        var result = parser.ParseExpression(expression);
        Assert.Equal(expectedTokens, result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    public void ParseExpression_EmptyOrWhitespace_ReturnsEmptyList(string expression)
    {
        Log.Information("Тест выполняется");
        var result = parser.ParseExpression(expression);
        Assert.Empty(result);
    }

    [Theory]
    [InlineData("ab + c")]
    [InlineData("2 + a")]
    [InlineData("!2 + @#$")]
    public void ParseExpression_InvalidCharacters_ThrowsException(string expression)
    {
        Log.Information("Тест выполняется");
        Assert.Throws<InvalidOperationException>(() => parser.ParseExpression(expression));
    }

    [Theory]
    [InlineData("  12  +  24 ", new[] { "12", "24", "+" })]
    [InlineData("\t5+6\n", new[] { "5", "6" , "+"})]
    public void ParseExpression_WithSpaces_ReturnsCorrectTokens(string expression, string[] expectedTokens)
    {
        Log.Information("Тест выполняется");
        var result = parser.ParseExpression(expression);
        Assert.Equal(expectedTokens, result);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("0.0")]
    [InlineData("000123")]
    [InlineData("0.0001")]
    public void ParseExpression_NumericEdgeCases_ReturnsCorrectTokens(string number)
    {
        Log.Information("Тест выполняется");
        var result = parser.ParseExpression(number);
        Assert.Single(result);
        Assert.Equal(number, result[0]);
    }

    [Fact]
    public void ParseExpression_LargeNumber_ReturnsCorrectToken()
    {
        var largeNumber = "99999999999997777777777777";
        var result = parser.ParseExpression(largeNumber);
        Assert.Single(result);
        Assert.Equal(largeNumber, result[0]);
    }

    [Theory]
    [InlineData("(1+2")]
    [InlineData("1+2)")]
    [InlineData("(()")]
    [InlineData("())")]
    public void ParseExpression_UnbalancedParentheses_Throws(string expression)
    {
        Log.Information("Тест выполняется");
        Assert.Throws<InvalidOperationException>(() => parser.ParseExpression(expression));
    }

    [Theory]
    [InlineData("2 ++ 2")]
    [InlineData("3 ^^ 3")]
    [InlineData("4 ** 4")]
    public void ParseExpression_InvalidOperatorSequences_Throws(string expression)
    {
        Log.Information("Тест выполняется");
        Assert.Throws<InvalidOperationException>(() => parser.ParseExpression(expression));
    }

    [Theory]
    [InlineData("2 + * 3")]
    [InlineData("+ 2 3")]
    [InlineData("2 3")]
    public void ParseExpression_SyntaxErrors_Throws(string expression)
    {
        Log.Information("Тест выполняется");
        Assert.Throws<InvalidOperationException>(() => parser.ParseExpression(expression));
    }
}