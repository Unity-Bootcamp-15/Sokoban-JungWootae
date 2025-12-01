namespace Sokoban {
    internal class Program {
        static void Main(string[] args) {
            int playerX = 3;
            int playerY = 3;
            int maxScore = 0;
            int curScore = 0;
            bool isClear = false;

            ConsoleKeyInfo keyInfo;
            int[,] mapData = new int[,]{
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                { 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1},
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
                { 1, 0, 0, 1, 1, 1, 0, 0, 0, 3, 0, 0, 1, 0, 3, 0, 1},
                { 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 1, 0, 0, 0, 1},
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 1, 0, 0, 0, 1},
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
            };
            CheckGameClear();
            GameStart();



            void CheckGameClear() {
                while (true) {
                    isClear = (curScore == maxScore);
                    if (isClear) {
                        Console.Clear();
                        Console.WriteLine("""

                    === You Clear Game!!! ===

                    """);
                        Console.WriteLine("Press ESC to Exit.");
                        keyInfo = Console.ReadKey(true);
                        if (keyInfo.Key == ConsoleKey.Escape) {
                            Environment.Exit(0);
                        }
                    }
                    keyInfo = Console.ReadKey(true);
                    PlayerReset();
                    PlayerMove();
                }
            }

            void GameStart() {
                foreach (int map in mapData) {
                    if (map == 3)
                        maxScore++;
                }
                Console.Clear();
                CreateMap();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.CursorVisible = false;
                Console.Title = "(   {=+=[=Sokoban=]=+=}   )";
                Console.SetCursorPosition(playerX, playerY);
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
                        if (playerUp == box) {
                            if ((mapData[playerY - 2, playerX] == 0 || mapData[playerY - 2, playerX] == goal) && playerY > 1) {
                                if (mapData[playerY - 2, playerX] == goal) {
                                    mapData[playerY - 2, playerX] = goal;
                                    curScore++;
                                }
                                else {
                                    mapData[playerY - 2, playerX] = box;
                                }
                                mapData[playerY - 1, playerX] = 0;
                                moveY = playerY - 1;
                                RedrawMap();
                            }
                        }
                        else if (playerUp != wall) {
                            if (playerY > 0)
                                moveY = playerY - 1;
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (playerDown == box) {
                            if ((mapData[playerY + 2, playerX] == 0 || mapData[playerY + 2, playerX] == goal) && playerY < mapData.GetLength(0) - 2) {
                                if (mapData[playerY + 2, playerX] == goal) {
                                    mapData[playerY + 2, playerX] = goal;
                                    curScore++;
                                }
                                else {
                                    mapData[playerY + 2, playerX] = box;
                                }
                                mapData[playerY + 1, playerX] = 0;
                                moveY = playerY + 1;
                                RedrawMap();
                            }
                        }
                        else if (playerDown != wall) {
                            if (playerY < mapData.GetLength(0) - 1)
                                moveY = playerY + 1;
                        }
                        break;

                    case ConsoleKey.LeftArrow:
                        if (playerLeft == box) {
                            if ((mapData[playerY, playerX - 2] == 0 || mapData[playerY, playerX - 2] == goal) && playerX > 1) {
                                if (mapData[playerY, playerX - 2] == goal) {
                                    mapData[playerY, playerX - 2] = goal;
                                    curScore++;
                                }
                                else {
                                    mapData[playerY, playerX - 2] = box;
                                }
                                mapData[playerY, playerX - 1] = 0;
                                moveX = playerX - 1;
                                RedrawMap();
                            }
                        }
                        else if (playerLeft != wall) {
                            if (playerX > 0)
                                moveX = playerX - 1;
                        }
                        break;

                    case ConsoleKey.RightArrow:
                        if (playerRight == box) {
                            if ((mapData[playerY, playerX + 2] == 0 || mapData[playerY, playerX + 2] == goal) && playerX < mapData.GetLength(1) - 2) {
                                if (mapData[playerY, playerX + 2] == goal) {
                                    mapData[playerY, playerX + 2] = goal;
                                    curScore++;
                                }
                                else {
                                    mapData[playerY, playerX + 2] = box;
                                }
                                mapData[playerY, playerX + 1] = 0;
                                moveX = playerX + 1;
                                RedrawMap();
                            }
                        }
                        else if (playerRight != wall) {
                            if (playerX < mapData.GetLength(1) - 1)
                                moveX = playerX + 1;
                        }
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
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("◈");
                }
            }

            void PlayerReset() {
                if (keyInfo.Key != ConsoleKey.R)
                    return;
                GameStart();
            }

            void CreateMap() {
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
                Console.WriteLine($"Your Goal : {curScore} / {maxScore}");
            }

            void RedrawMap() {
                Console.SetCursorPosition(0, 0);
                CreateMap();
            }
        }
    }
}