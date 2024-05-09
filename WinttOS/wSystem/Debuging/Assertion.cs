namespace WinttOS.wSystem.Debug
{
    public static class Assertion
    {
        public static void Assert(bool condition, string errorMessage)
        {
            if (!condition) { throw new AssertFailedException(errorMessage); }
        }

        public static void Assert(bool condition) => Assert(condition, "Assert: Condition not met!");

        public static void Expect<T>(T var, T value) => Expect(var, value, $"Assert: Expected \"{value}\", got \"{var}\"");

        public static void Expect<T>(T var, T value, string errorMessage) => Assert(var.Equals(value), errorMessage);
    }
}
