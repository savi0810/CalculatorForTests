using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorForTests
{
    internal interface IEvaluator
    {
        double Evaluate(List<string> parsedExpression);
    }
}
