using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorForTests
{
    public static class Calculator
    {
        private static readonly IExpressionParser _parser;
        private static readonly IEvaluator _evaluator;

        static Calculator()
        {
            _parser = new ExpressionParser();
            _evaluator = new ParsedExpressionEvaluator();
        }

        public static double Calculate(string expression)
        {
            var parsedExpression = _parser.ParseExpression(expression);
            return _evaluator.Evaluate(parsedExpression);
        }
    }
}
