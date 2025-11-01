using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CalculatorForTests
{
    public class ParsedExpressionEvaluator : IEvaluator
    {
        public double Evaluate(List<string> parsedExpression)
        {
            Stack<double> stack = new Stack<double>();

            foreach (var token in parsedExpression)
            {
                if (IsValidNumber(token))
                {
                    ProcessNumber(token, stack);
                }
                else
                {
                    ProcessOperator(token, stack);
                }
            }

            if (stack.Count != 1)
                throw new InvalidOperationException("Некорректное выражение");

            return stack.Pop();
        }

        private void ProcessNumber(string token, Stack<double> stack)
        {
            double num = double.Parse(token, CultureInfo.InvariantCulture);
            stack.Push(num);
        }

        private void ProcessOperator(string token, Stack<double> stack)
        {
            if (stack.Count < 2)
                throw new InvalidOperationException($"Недостаточно операндов для оператора {token}");

            double b = stack.Pop();
            double a = stack.Pop();
            double result = token switch
            {
                "+" => a + b,
                "-" => a - b,
                "*" => a * b,
                "/" => b == 0 ? throw new DivideByZeroException("Деление на ноль") : a / b,
                "^" => Math.Pow(a, b),
                _ => throw new InvalidOperationException($"Неизвестный оператор: {token}")
            };
            stack.Push(result);
        }

        private bool IsValidNumber(string token)
        {
            return Regex.IsMatch(token, @"^-?\d+(\.\d+)?$");
        }
    }
}
