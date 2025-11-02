using Serilog;

namespace CalculatorForTests.Tests
{
    internal static class LogInitializerTests
    {
        private static bool _isInitialized = false;
        private static readonly object _lockObject = new object();

        public static void Init()
        {
            if (!_isInitialized)
            {
                lock (_lockObject)
                {
                    if (!_isInitialized)
                    {
                        Log.Logger = new LoggerConfiguration()
                            .WriteTo.File(
                                $"../../../LogTests/test_log.log",
                                rollingInterval: RollingInterval.Day,
                                shared: true) 
                            .CreateLogger();
                        _isInitialized = true;
                    }
                }
            }
        }
    }
}
