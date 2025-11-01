using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
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

            string? lastTokenType = string.Empty; 

            foreach (var token in tokens)
            {
                if (IsValidNumber(token))
                {
                    ValidateTokenSequence(token, lastTokenType, "number");
                    output.Add(token);
                    lastTokenType = "number";
                }
                else if (token == "(")
                {
                    ValidateTokenSequence(token, lastTokenType, "openParen");
                    operators.Push(token);
                    lastTokenType = "openParen";
                }
                else if (token == ")")
                {
                    ValidateTokenSequence(token, lastTokenType, "closeParen");
                    PopOperatorsUntilOpenRoundBrackets(operators, output);
                    lastTokenType = "closeParen";
                }
                else if (operatorPriority.ContainsKey(token))
                {
                    ValidateTokenSequence(token, lastTokenType, "operator");
                    HandleOperator(token, operators, output);
                    lastTokenType = "operator";
                }
                else
                {
                    throw new InvalidOperationException($"Недопустимый токен: {token}");
                }
            }

            if (lastTokenType == "operator" || lastTokenType == "openParen")
                throw new InvalidOperationException("Конец выражения после оператора или открывающей скобки");

            AppendRemainingOperators(operators, output);

            return output;
        }


        private void ValidateTokenSequence(string token, string lastTokenType, string currentTokenType)
        {
            if (currentTokenType == "number")
            {
                if (lastTokenType == "number" || lastTokenType == "closeParen")
                    throw new InvalidOperationException("Синтаксическая ошибка: два числа или закрытая скобка подряд");
            }
            else if (currentTokenType == "openParen")
            {
                if (lastTokenType == "number" || lastTokenType == "closeParen")
                    throw new InvalidOperationException("Синтаксическая ошибка: пропущен оператор перед '('");
            }
            else if (currentTokenType == "closeParen")
            {
                if (lastTokenType == "operator" || lastTokenType == "openParen" || lastTokenType == null)
                    throw new InvalidOperationException("Синтаксическая ошибка: пустые скобки или оператор перед ')'");
            }
            else if (currentTokenType == "operator")
            {
                if (lastTokenType == string.Empty || lastTokenType == "operator" || lastTokenType == "openParen")
                {
                    if (token != "-")
                        throw new InvalidOperationException("Синтаксическая ошибка: оператор без операнда");
                }
            }
        }

        private List<string> TokenizeExpression(string expression)
        {
            var matches = Regex.Matches(expression, @"(\d+\.\d+|\d+|[-+*/()^])");
            var tokens = matches.Cast<Match>().Select(m => m.Value).ToList();

            tokens = ProcessUnaryMinus(tokens);

            return tokens;
        }

        private List<string> ProcessUnaryMinus(List<string> tokens)
        {
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i] == "-")
                {
                    if (i == 0 ||
                        tokens[i - 1] == "(" ||
                        tokens[i - 1] == "+" || tokens[i - 1] == "-" ||
                        tokens[i - 1] == "*" || tokens[i - 1] == "/" || tokens[i - 1] == "^")
                    {
                        if (i + 1 < tokens.Count && IsValidNumber(tokens[i + 1]))
                        {
                            tokens[i + 1] = "-" + tokens[i + 1];
                            tokens.RemoveAt(i);
                        }
                        else
                        {
                            throw new InvalidOperationException("Некорректное выражение: ожидается число после унарного минуса");
                        }
                    }
                }
            }
            return tokens;
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
            return Regex.IsMatch(token, @"^-?\d+(\.\d+)?$");
        }
    }
}
