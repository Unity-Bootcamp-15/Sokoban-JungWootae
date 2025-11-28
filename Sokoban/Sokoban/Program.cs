namespace Sokoban {
    internal class Program {
        static void Main(string[] args) {
            int playerX = 3;
            int playerY = 3;

            ConsoleKeyInfo keyInfo;
            int[,] mapData = new int[,]{       
                // 0 : blank, 1 : wall, 2 : player, 3 : box, 4 : goal
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                { 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1},
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
                { 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
            };

            GameStart();
            while (true) {
                keyInfo = Console.ReadKey(true);
                PlayerReset();
                PlayerMove();
            }

            void GameStart() {
                Console.Clear();
                CreateMap();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.CursorVisible = false;
                Console.Title = "(   {=+=[=Sokoban=]=+=}   )";
                Console.SetCursorPosition(3, 3);
                Console.Write("◈");
            }

            void PlayerMove() {
                int moveX = playerX;
                int moveY = playerY;

                switch (keyInfo.Key) {
                    case ConsoleKey.UpArrow:
                        Console.SetCursorPosition(playerX, playerY);


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

                if (moveX != playerX || moveY != playerY) {
                    Console.SetCursorPosition(playerX, playerY);
                    Console.Write(" ");
                    playerX = moveX;
                    playerY = moveY;
                    Console.SetCursorPosition(playerX, playerY);
                    Console.Write("◈");
                }
            }

            void PlayerReset() {
                if (keyInfo.Key != ConsoleKey.R)
                    return;
                GameStart();
            }
            
            void CreateMap() {
                // 0 : blank, 1 : wall, 2 : player, 3 : box, 4 : goal
                for (int i = 0; i < mapData.GetLength(0); i++) {
                    for (int j = 0; j < mapData.GetLength(1); j++) {
                        switch (mapData[i, j]) {
                            case 0:
                                Console.Write(" ");
                                break;

                            case 1:
                                Console.Write("■");
                                break;

                            case 2:
                                Console.Write("◈");
                                break;
                        }
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}