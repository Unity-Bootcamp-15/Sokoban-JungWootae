using System;

namespace Sokoban {
    public class Renderer {
        public void DrawMap(Map map, int curScore, int maxScore, bool isMagnetActive) {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < map.GetHeight(); i++) {
                for (int j = 0; j < map.GetWidth(); j++) {
                    switch (map.Data[i, j]) {
                        case (int)TileType.Empty:
                            Console.Write(" ");
                            break;
                        case (int)TileType.Wall:
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("█");
                            break;
                        case (int)TileType.Player:
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write("◈");
                            break;
                        case (int)TileType.Box:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write("▣");
                            break;
                        case (int)TileType.Goal:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("※");
                            break;
                        case (int)TileType.PlayerPortal:
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.Write("O");
                            break;
                        case (int)TileType.BoxPortal:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("0");
                            break;
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine($"Your Goal : {curScore} / {maxScore}");
            PrintInfos(map.GetHeight(), isMagnetActive);
        }

        public void DrawPlayer(Player player) {
            Console.SetCursorPosition(player.X, player.Y);
            Console.ForegroundColor = player.IsMagnetActive ? ConsoleColor.DarkMagenta : ConsoleColor.Magenta;
            Console.Write("◈");
        }
        public void ClearPlayer(int x, int y) {
            Console.SetCursorPosition(x, y);
            Console.Write(" ");
        }

        private void PrintInfos(int mapHeight, bool isMagnetActive) {
            Console.SetCursorPosition(0, mapHeight + 2);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("""
                Keys :
                Arrow Keys - Move Player
                         Z - Undo
                         M - Toggle Magnet Mode
                """);

            if (isMagnetActive) {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("        [ Magnet Mode ON ]");
            }
            else {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("        [ Magnet Mode OFF]");
            }
        }
    }
}