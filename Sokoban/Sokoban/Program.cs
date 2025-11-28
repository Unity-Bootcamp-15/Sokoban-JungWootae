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
                { 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 0, 3, 0, 1},
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 1, 0, 0, 0, 1},
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

                int wall = 1;
                int box = 3;
                int goal = 4;

                int playerUp = mapData[playerY - 1, playerX];
                int playerDown = mapData[playerY + 1, playerX];
                int playerLeft = mapData[playerY, playerX - 1];
                int playerRight = mapData[playerY, playerX + 1];



                switch (keyInfo.Key) {
                    case ConsoleKey.UpArrow:
                        if (playerUp != wall)
                        if (playerY > 0)
                            moveY = playerY - 1;
                        break;

                    case ConsoleKey.DownArrow:
                        if (playerDown != wall)
                        if (playerY < Console.BufferHeight - 1)
                            moveY = playerY + 1;
                        break;

                    case ConsoleKey.LeftArrow:
                        if (playerLeft != wall)
                        if (playerX > 0)
                            moveX = playerX - 1;
                        break;

                    case ConsoleKey.RightArrow:
                        if (playerRight != wall)
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
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("■");
                                break;

                            case 2:
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.Write("◈");
                                break;

                            case 3:
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                Console.Write("□");
                                break;

                            case 4:
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write("※");
                                break;

                        }
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}