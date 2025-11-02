using Serilog;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorForTests.Tests.Tests
{
    public class BaseTest : IDisposable
    {
        private readonly TestFixture _fixture;
        private readonly int _testNumber;
        private readonly DateTime _startTime;
        private readonly string _testName;

        public BaseTest(TestFixture fixture)
        {
            _fixture = fixture;
            _testName = GetType().Name;
            (_testNumber, _startTime) = _fixture.StartTest(_testName);
        }

        public void Dispose()
        {
            _fixture.EndTest(_testNumber, _testName, _startTime);
        }
        protected void LogTestStep(string message, [CallerMemberName] string methodName = "")
        {
            Log.Information("[Test #{TestNumber}] {TestName}: {Message}",
                _testNumber, _testName, message);
        }

        protected string GetCurrentTestMethodName([CallerMemberName] string methodName = "")
        {
            return methodName;
        }
    }
}
