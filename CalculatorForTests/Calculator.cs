using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorForTests
{
    public class Calculator
    {
        private static readonly ExpressionParser _parser = new ExpressionParser();
        private static readonly ParsedExpressionEvaluator _evaluator = new ParsedExpressionEvaluator();

        public static double Calculate(string expression)
        {
            var parsedExpression = _parser.ParseExpression(expression);
            return _evaluator.Evaluate(parsedExpression);
        }
    }
}
