namespace Glekcraft;

/// <summary>
/// The class that contains the entry point of the program.
/// </summary>
public static class Program {
    public const int ExitSuccess = 0;

    public const int ExitFailure = 1;

    /// <summary>
    /// The entry point of the program.
    /// </summary>
    public static int Main() {
        using var game = new Game();
        try {
            game.Initialize();
            game.Run();
        } catch (Exception ex) {
            Console.Error.WriteLine("Fatal error: {0}", ex);
            return ExitFailure;
        } finally {
            game.Dispose();
        }
        return ExitSuccess;
    }
}
