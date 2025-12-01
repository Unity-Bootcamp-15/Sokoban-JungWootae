namespace Sokoban {
    public enum TileType {
        Empty = 0,
        Wall = 1,
        Player = 2,
        Box = 3,
        Goal = 4
    }
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

            GameStart();

            while (true) {
                CheckGameClear();
            }

            void CheckGameClear() {
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

                int playerUp = mapData[playerY - 1, playerX];
                int playerDown = mapData[playerY + 1, playerX];
                int playerLeft = mapData[playerY, playerX - 1];
                int playerRight = mapData[playerY, playerX + 1];

                switch (keyInfo.Key) {
                    case ConsoleKey.UpArrow:
                        if (playerUp == (int)TileType.Box) {
                            if ((mapData[playerY - 2, playerX] == 0 || mapData[playerY - 2, playerX] == (int)TileType.Goal) && playerY > 1) {
                                if (mapData[playerY - 2, playerX] == (int)TileType.Goal) {
                                    mapData[playerY - 2, playerX] = (int)TileType.Goal;
                                    curScore++;
                                }
                                else {
                                    mapData[playerY - 2, playerX] = (int)TileType.Box;
                                }
                                mapData[playerY - 1, playerX] = 0;
                                moveY = playerY - 1;
                                RedrawMap();
                            }
                        }
                        else if (playerUp != (int)TileType.Wall) {
                            if (playerY > 0)
                                moveY = playerY - 1;
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (playerDown == (int)TileType.Box) {
                            if ((mapData[playerY + 2, playerX] == 0 || mapData[playerY + 2, playerX] == (int)TileType.Goal) && playerY < mapData.GetLength(0) - 2) {
                                if (mapData[playerY + 2, playerX] == (int)TileType.Goal) {
                                    mapData[playerY + 2, playerX] = (int)TileType.Goal;
                                    curScore++;
                                }
                                else {
                                    mapData[playerY + 2, playerX] = (int)TileType.Box;
                                }
                                mapData[playerY + 1, playerX] = 0;
                                moveY = playerY + 1;
                                RedrawMap();
                            }
                        }
                        else if (playerDown != (int)TileType.Wall) {
                            if (playerY < mapData.GetLength(0) - 1)
                                moveY = playerY + 1;
                        }
                        break;

                    case ConsoleKey.LeftArrow:
                        if (playerLeft == (int)TileType.Box) {
                            if ((mapData[playerY, playerX - 2] == 0 || mapData[playerY, playerX - 2] == (int)TileType.Goal) && playerX > 1) {
                                if (mapData[playerY, playerX - 2] == (int)TileType.Goal) {
                                    mapData[playerY, playerX - 2] = (int)TileType.Goal;
                                    curScore++;
                                }
                                else {
                                    mapData[playerY, playerX - 2] = (int)TileType.Box;
                                }
                                mapData[playerY, playerX - 1] = 0;
                                moveX = playerX - 1;
                                RedrawMap();
                            }
                        }
                        else if (playerLeft != (int)TileType.Wall) {
                            if (playerX > 0)
                                moveX = playerX - 1;
                        }
                        break;

                    case ConsoleKey.RightArrow:
                        if (playerRight == (int)TileType.Box) {
                            if ((mapData[playerY, playerX + 2] == 0 || mapData[playerY, playerX + 2] == (int)TileType.Goal) && playerX < mapData.GetLength(1) - 2) {
                                if (mapData[playerY, playerX + 2] == (int)TileType.Goal) {
                                    mapData[playerY, playerX + 2] = (int)TileType.Goal;
                                    curScore++;
                                }
                                else {
                                    mapData[playerY, playerX + 2] = (int)TileType.Box;
                                }
                                mapData[playerY, playerX + 1] = 0;
                                moveX = playerX + 1;
                                RedrawMap();
                            }
                        }
                        else if (playerRight != (int)TileType.Wall) {
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
                            case (int)TileType.Empty:
                                Console.Write(" ");
                                break;
                            case (int)TileType.Wall:
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("■");
                                break;
                            case (int)TileType.Player:
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.Write("◈");
                                break;
                            case (int)TileType.Box:
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                Console.Write("□");
                                break;
                            case (int)TileType.Goal:
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