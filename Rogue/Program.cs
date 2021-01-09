namespace Rogue
{
    class Program
    {
        static void Main(string[] args)
        {
            var generator = new MapGenerator(30, 20, 30, 3, 6, 3, 6);

            System.Console.ForegroundColor = System.ConsoleColor.DarkGray;
            System.Console.SetWindowSize(100, 50);
            System.Console.CursorVisible = false;
            var map = generator.GenerateMap();

            System.Console.ReadKey();
        }
    }
}
