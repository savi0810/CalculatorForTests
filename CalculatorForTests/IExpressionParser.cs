using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorForTests
{
    internal interface IExpressionParser
    {
        List<string> ParseExpression(string expression);
    }
}
