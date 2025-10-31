using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CalculatorForTests
{
    public class ExpressionParser : IExpressionParser
    {
        private static readonly Dictionary<string, int> operatorPriority = new Dictionary<string, int>
    {
        {"+", 1}, {"-", 1}, {"*", 2}, {"/", 2}, {"^", 3}, {"(", 0}
    };

        public List<string> ParseExpression(string expression)
        {
            var tokens = TokenizeExpression(expression);
            List<string> output = new List<string>();
            Stack<string> operators = new Stack<string>();

            foreach (var token in tokens)
            {
                if (IsValidNumber(token))
                {
                    output.Add(token);
                }
                else if (token == "(")
                {
                    operators.Push(token);
                }
                else if (token == ")")
                {
                    PopOperatorsUntilOpenRoundBrackets(operators, output);
                }
                else
                {
                    HandleOperator(token, operators, output);
                }
            }

            AppendRemainingOperators(operators, output);

            return output;
        }

        private List<string> TokenizeExpression(string expression)
        {
            var matches = Regex.Matches(expression, @"(\d+\.\d+|\d+|[-+*/()^])");
            return matches.Cast<Match>().Select(m => m.Value).ToList();
        }

        private void HandleOperator(string token, Stack<string> operators, List<string> output)
        {
            while (operators.Count > 0 &&
                   operators.Peek() != "(" &&
                   operatorPriority[operators.Peek()] >= operatorPriority[token])
            {
                output.Add(operators.Pop());
            }
            operators.Push(token);
        }

        private void PopOperatorsUntilOpenRoundBrackets(Stack<string> operators, List<string> output)
        {
            while (operators.Count > 0 && operators.Peek() != "(")
            {
                output.Add(operators.Pop());
            }

            if (operators.Count == 0)
                throw new InvalidOperationException("Несбалансированные скобки");

            operators.Pop(); 
        }

        private void AppendRemainingOperators(Stack<string> operators, List<string> output)
        {
            while (operators.Count > 0)
            {
                if (operators.Peek() == "(")
                    throw new InvalidOperationException("Несбалансированные скобки");
                output.Add(operators.Pop());
            }
        }

        private bool IsValidNumber(string token)
        {
            return Regex.IsMatch(token, @"^\d+(\.\d+)?$");
        }
    }
}
