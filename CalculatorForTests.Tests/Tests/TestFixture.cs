using Serilog;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace CalculatorForTests.Tests.Tests
{
    [CollectionDefinition("TestCollection")]
    public class TestCollection : ICollectionFixture<TestFixture> { }

    public class TestFixture : IDisposable
    {
        private static readonly object _lockObject = new object();
        private static int _currentTestNumber = 0;

        public TestFixture()
        {
            LogInitializerTests.Init();
        }

        public (int testNumber, DateTime startTime) StartTest(string testName)
        {
            lock (_lockObject)
            {
                _currentTestNumber++;
                var startTime = DateTime.Now;
                Log.Information("[Test #{TestNumber}] Запуск теста: {TestName} в {StartTime}",
                    _currentTestNumber, testName, startTime.ToString("HH:mm:ss.fff"));
                return (_currentTestNumber, startTime);
            }
        }

        public void EndTest(int testNumber, string testName, DateTime startTime)
        {
            lock (_lockObject)
            {
                var endTime = DateTime.Now;
                var duration = endTime - startTime;
                Log.Information("[Test #{TestNumber}] Завершение теста: {TestName} за {Duration}ms",
                    testNumber, testName, duration.TotalMilliseconds);
            }
        }

        public void Dispose()
        {
            Log.CloseAndFlush();
        }
    }
}