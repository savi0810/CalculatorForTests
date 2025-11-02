using Serilog;

namespace CalculatorForTests.Tests
{
    internal static class LogInitializerTests
    {
        public static void Init()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("test_log.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}
