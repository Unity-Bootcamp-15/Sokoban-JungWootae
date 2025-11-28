namespace Sokoban
{
    internal class Program{

        static void Main(string[] args){
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.CursorVisible = false;

            int playerX = 0;
            int playerY = 0;

            Console.SetCursorPosition(0, 0);
            Console.Write("◈");

            while (true) {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                int curPosX = Console.GetCursorPosition().Left;
                int curPosY = Console.GetCursorPosition().Top;

                int moveX = playerX;
                int moveY = playerY;

                switch (keyInfo.Key) {
                    case ConsoleKey.UpArrow:
                        if (playerY > 0)
                            moveY = playerY - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        if (playerY < Console.BufferHeight - 1)
                            moveY = playerY + 1;
                        break;

                    case ConsoleKey.LeftArrow:
                        if (playerX > 0)
                            moveX = playerX - 1;
                        break;
                    case ConsoleKey.RightArrow:
                        if (playerX < Console.BufferWidth - 1)
                            moveX = playerX + 1;
                        break;

                    case ConsoleKey.Escape:
                        return;
                }
                Console.SetCursorPosition(playerX, playerY);
                Console.Write(" ");

                playerX = moveX;
                playerY = moveY;

                Console.SetCursorPosition(playerX, playerY);
                Console.Write("◈");
            }
        }
    }
}
