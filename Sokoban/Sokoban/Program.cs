namespace Sokoban
{
    internal class Program{
        static void Main(string[] args){
            
            Console.ReadKey();
        }

        public void ResetGame() {
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Title = "Sokoban Game";
            Console.CursorVisible = false;
            Console.Clear();
        }
    }
}
