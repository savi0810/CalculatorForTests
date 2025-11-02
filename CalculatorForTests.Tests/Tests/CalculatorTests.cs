namespace CalculatorForTests.Tests.Tests;

using CalculatorForTests;
using Serilog;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using Xunit;

[Collection("TestCollection")]
public class CalculatorTests : BaseTest
{
    public CalculatorTests(TestFixture fixture) : base(fixture) { }

    [Theory]
    [InlineData("2+3", 5)]
    [InlineData("10-4", 6)]
    [InlineData("4*5", 20)]
    [InlineData("20/4", 5)]
    public void Calculate_BasicOperations_ReturnsExpectedResults(string expression, double expected)
    {
        string testMethodName = GetCurrentTestMethodName();
        LogTestStep($"Тест '{testMethodName}' выполняется с выражением '{expression}'"); 
        var result = Calculator.Calculate(expression);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("2+3*4", 14)]
    [InlineData("(2+3)*4", 20)]
    [InlineData("2+ (3*4)", 14)]
    [InlineData(" ( 2 + 3 ) * 4 ", 20)]
    public void Calculate_OperatorPrecedence_ReturnsCorrectResult(string expression, double expected)
    {
        string testMethodName = GetCurrentTestMethodName();
        LogTestStep($"Тест '{testMethodName}' выполняется с выражением '{expression}'");
        var result = Calculator.Calculate(expression);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("-2+3", 1)]
    [InlineData("-3^2", 9)]
    [InlineData("-3*3", -9)]
    public void Calculate_NegativeNumbers_ReturnsCorrectResult(string expression, double expected)
    {
        string testMethodName = GetCurrentTestMethodName();
        LogTestStep($"Тест '{testMethodName}' выполняется с выражением '{expression}'"); 
        var result = Calculator.Calculate(expression);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("0+0", 0)]
    [InlineData("0-0", 0)]
    [InlineData("0*0", 0)]
    [InlineData("0/1", 0)]
    [InlineData("1/1", 1)]
    [InlineData("999999999+1", 1000000000)]
    [InlineData("1-999999999", -999999998)]
    public void Calculate_BorderlineCases_ReturnsExpectedResults(string expression, double expected)
    {
        string testMethodName = GetCurrentTestMethodName();
        LogTestStep($"Тест '{testMethodName}' выполняется с выражением '{expression}'"); 
        var result = Calculator.Calculate(expression);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("2++2")]
    [InlineData("5//2")]
    [InlineData("4**2")]
    [InlineData("(2+3")]
    [InlineData("2+3)")]
    [InlineData("abc")]
    [InlineData("2 + a")]
    public void Calculate_InvalidExpressions_ThrowsException(string expression)
    {
        string testMethodName = GetCurrentTestMethodName();
        LogTestStep($"Тест '{testMethodName}' выполняется с выражением '{expression}'"); 
        Assert.Throws<InvalidOperationException>(() => Calculator.Calculate(expression));
    }

    [Fact]
    public void Calculate_DivisionByZero_ThrowsException()
    {
        var expression = "1/0";
        string testMethodName = GetCurrentTestMethodName();
        LogTestStep($"Тест '{testMethodName}' выполняется с выражением '{expression}'");
        Assert.Throws<DivideByZeroException>(() => Calculator.Calculate(expression));
    }
}