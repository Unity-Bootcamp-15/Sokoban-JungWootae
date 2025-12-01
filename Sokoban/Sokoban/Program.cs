namespace Sokoban {

    /*
        레벨      |    의미                          | 사용 시점                          | 예시                                             
    =======================================================================================================================================
        TRACE         가장 상세한 추적 정보             함수 진입/종료, 루프 반복 추적        "Entered ProcessData()"
        DEBUG         디버깅용 상세 정보                변수 값, 상태 변화, 조건 분기         "Counter value: 42"
        INFO          일반 정보성 메시지                프로그램 정상 동작 확인               "Server started on port 8080"
        WARN          잠재적 문제 경고                  복구 가능한 이상 상황                 "Configuration file not found, using defaults"
        ERROR         기능에 영향을 주는 에러            예외 상황, 기능 실패                 "Failed to connect to database"
        FATAL         프로그램 종료 필요한 치명적 에러   더이상 실행 불가능한 상황             "Out of memory, terminating"
    */
    public enum TileType {
        Empty = 0,
        Wall = 1,
        Player = 2,
        Box = 3,
        Goal = 4,
        PlayerPortal = 5,
        BoxPortal = 6
    }
    internal class Program {
        static void Main(string[] args) {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            int playerX = 3;
            int playerY = 3;
            int maxScore = 0;
            int curScore = 0;
            bool isClear = false;
            bool isMagnetActive = false;

            //record portal pos
            (int, int)[] playerPortals = new (int, int)[2];
            (int, int)[] boxPortals = new (int, int)[2];

            ConsoleKeyInfo keyInfo;
            int[,] mapData = new int[,]
            {
                    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                    { 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                    { 1, 0, 6, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 1},
                    { 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1},
                    { 1, 0, 0, 0, 0, 0, 0, 0, 0 ,0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                    { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                    { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 1},
                    { 1, 1, 1, 1, 1, 1, 0, 0, 0, 3, 0, 0, 1, 0, 3, 0, 1, 1, 1, 1},
                    { 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 1},
                    { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 1, 0, 0, 0, 1, 1, 1, 1},
                    { 1, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1},
                    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 6, 0, 0, 0, 1, 1, 1, 1},
                    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1},
                    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
            };
            int[,] pastMap = new int[mapData.GetLength(0), mapData.GetLength(1)];

            int pastPlayerX = playerX;
            int pastPlayerY = playerY;
            bool canUndo = false;

            GameStart();
            while (true) {
                CheckGameClear();
            }

            void printInfos() {
                Console.SetCursorPosition(0, mapData.GetLength(0) + 2);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("""
                        Arrow Keys - Move Player
                                Z - Undo
                                M - Toggle Magnet Mode
                        """);

                //check magnet mode
                if (isMagnetActive) {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("        [ Magnet Mode ON ]");
                }
                else {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("        [ Magnet Mode OFF]");
                }

            }
            
            void SavePastState() {
                for (int i = 0; i < mapData.GetLength(0); i++)
                    for (int j = 0; j < mapData.GetLength(1); j++)
                        pastMap[i, j] = mapData[i, j];

                pastPlayerX = playerX;
                pastPlayerY = playerY;
                canUndo = true;
            }

            void RestorePastState() {
                for (int i = 0; i < mapData.GetLength(0); i++)
                    for (int j = 0; j < mapData.GetLength(1); j++)
                        mapData[i, j] = pastMap[i, j];

                playerX = pastPlayerX;
                playerY = pastPlayerY;

                RedrawMap();
                RedrawPlayer();
            }

            //record portal pos
            void FindPortals() {
                int pIdx = 0, bIdx = 0;
                for (int i = 0; i < mapData.GetLength(0); i++) {
                    for (int j = 0; j < mapData.GetLength(1); j++) {
                        if (mapData[i, j] == (int)TileType.PlayerPortal && pIdx < 2)
                            playerPortals[pIdx++] = (i, j);
                        else if (mapData[i, j] == (int)TileType.BoxPortal && bIdx < 2)
                            boxPortals[bIdx++] = (i, j);
                    }
                }
            }

            void CheckGameClear() {
                isClear = (curScore == maxScore);
                if (isClear) {
                    Console.Clear();
                    Console.WriteLine("\n\n=== You Clear Game!!! ===\n\n");
                    Console.WriteLine("Press ESC to Exit.");
                    keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Escape) {
                        Environment.Exit(0);
                    }
                }
                keyInfo = Console.ReadKey(true);
                PlayerMove();
            }

            void GameStart() {
                Console.OutputEncoding = System.Text.Encoding.UTF8;

                playerX = 3;
                playerY = 3;
                maxScore = 0;
                curScore = 0;
                isClear = false;
                canUndo = false;
                playerPortals = new (int, int)[2];
                boxPortals = new (int, int)[2];
                pastPlayerX = playerX;
                pastPlayerY = playerY;

                for (int i = 0; i < mapData.GetLength(0); i++)
                    for (int j = 0; j < mapData.GetLength(1); j++)
                        pastMap[i, j] = mapData[i, j];
                FindPortals();

                foreach (int map in mapData)
                    if (map == (int)TileType.Box)
                        maxScore++;

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.CursorVisible = false;
                Console.Title = "(  {==[= S.o.k.o.b.a.n =]==}  )";
                RedrawMap();
                RedrawPlayer();
            }

            void PlayerMove() {
                int moveX = playerX;
                int moveY = playerY;
                int nextX = playerX, nextY = playerY;

                if (keyInfo.Key == ConsoleKey.Z && mapData != pastMap) {
                    if (canUndo)
                        RestorePastState();
                    return;
                }

                SavePastState();
                switch (keyInfo.Key) {
                    case ConsoleKey.UpArrow:
                        nextY = playerY - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        nextY = playerY + 1;
                        break;
                    case ConsoleKey.LeftArrow:
                        nextX = playerX - 1;
                        break;
                    case ConsoleKey.RightArrow:
                        nextX = playerX + 1;
                        break;

                    case ConsoleKey.M:
                        int nearBoxes = 0;

                        if (mapData[playerY - 1, playerX] == (int)TileType.Box)
                            nearBoxes++;

                        if (mapData[playerY + 1, playerX] == (int)TileType.Box)
                            nearBoxes++;

                        if (mapData[playerY, playerX - 1] == (int)TileType.Box)
                            nearBoxes++;

                        if (mapData[playerY, playerX + 1] == (int)TileType.Box)
                            nearBoxes++;

                        if (nearBoxes == 1)
                            isMagnetActive = !isMagnetActive;

                        RedrawMap();
                        RedrawPlayer();
                        break;

                    default:
                        return;
                }

                int dx = nextX - playerX;
                int dy = nextY - playerY;

                if (isMagnetActive && (dx != 0 || dy != 0)) {
                    int boxX = -1, boxY = -1;
                    if (mapData[playerY - 1, playerX] == (int)TileType.Box) {
                        boxX = playerX;
                        boxY = playerY - 1;
                    }

                    else if (mapData[playerY + 1, playerX] == (int)TileType.Box) {
                        boxX = playerX;
                        boxY = playerY + 1;
                    }

                    else if (mapData[playerY, playerX - 1] == (int)TileType.Box) {
                        boxX = playerX - 1;
                        boxY = playerY;
                    }

                    else if (mapData[playerY, playerX + 1] == (int)TileType.Box) {
                        boxX = playerX + 1;
                        boxY = playerY;
                    }

                    if (boxX != -1) {
                        int nextBoxX = boxX + dx;
                        int nextBoxY = boxY + dy;

                        int targetTileForBox = mapData[nextBoxY, nextBoxX];
                        bool boxBlocked = (targetTileForBox == (int)TileType.Wall || targetTileForBox == (int)TileType.Box ||
                                        targetTileForBox == (int)TileType.BoxPortal || targetTileForBox == (int)TileType.PlayerPortal);

                        if (nextBoxX == playerX && nextBoxY == playerY)
                            boxBlocked = false;

                        int targetTileForPlayer = mapData[nextY, nextX];
                        bool playerBlocked = (targetTileForPlayer == (int)TileType.Wall || targetTileForPlayer == (int)TileType.BoxPortal ||
                                            targetTileForPlayer == (int)TileType.PlayerPortal);

                        if (targetTileForPlayer == (int)TileType.Box && (nextX != boxX || nextY != boxY))
                            playerBlocked = true;

                        else if (targetTileForPlayer == (int)TileType.Box)
                            playerBlocked = false;

                        if (!boxBlocked && !playerBlocked) {
                            mapData[boxY, boxX] = 0;
                            if (mapData[nextBoxY, nextBoxX] == (int)TileType.Goal) {
                                mapData[nextBoxY, nextBoxX] = (int)TileType.Goal;
                                curScore++;
                            }
                            else {
                                mapData[nextBoxY, nextBoxX] = (int)TileType.Box;
                            }

                            playerX = nextX;
                            playerY = nextY;
                            RedrawMap();
                            RedrawPlayer();
                            return;
                        }
                    }
                    else {
                        isMagnetActive = false;
                        RedrawMap();
                        RedrawPlayer();
                    }
                }

                int nextTile = mapData[nextY, nextX];


                if (nextTile == (int)TileType.Box) {
                    int boxNextX = nextX + (nextX - playerX);
                    int boxNextY = nextY + (nextY - playerY);
                    int boxNextTile = mapData[boxNextY, boxNextX];

                    // box in box portal
                    if (mapData[boxNextY, boxNextX] == (int)TileType.BoxPortal) {
                        // find destination
                        int idx = (boxPortals[0] == (boxNextY, boxNextX)) ? 1 : 0;
                        var (destY, destX) = boxPortals[idx];
                        // check accessible
                        if (mapData[destY, destX] == (int)TileType.BoxPortal) {
                            mapData[boxNextY, boxNextX] = 0;
                            mapData[destY, destX] = 0;
                            mapData[nextY, nextX] = 0;
                            mapData[destY, destX] = (int)TileType.Box;
                            moveX = nextX;
                            moveY = nextY;
                            FindPortals();
                            RedrawMap();
                        }
                        return;
                    }
                    // if type not equal = wall
                    if (mapData[boxNextY, boxNextX] == (int)TileType.PlayerPortal || boxNextTile == (int)TileType.Wall || boxNextTile == (int)TileType.Box)
                        return;

                    // box in goal
                    if (boxNextTile == (int)TileType.Goal) {
                        mapData[boxNextY, boxNextX] = (int)TileType.Goal;
                        curScore++;
                    }
                    else {
                        mapData[boxNextY, boxNextX] = (int)TileType.Box;
                    }
                    mapData[nextY, nextX] = 0;
                    moveX = nextX;
                    moveY = nextY;
                    RedrawMap();
                }
                // p in p portal
                else if (nextTile == (int)TileType.PlayerPortal) {
                    int idx = (playerPortals[0] == (nextY, nextX)) ? 1 : 0;
                    var (destY, destX) = playerPortals[idx];
                    // check accessible
                    if (mapData[destY, destX] == (int)TileType.PlayerPortal) {
                        mapData[nextY, nextX] = 0;
                        mapData[destY, destX] = 0;
                        moveX = destX;
                        moveY = destY;
                        FindPortals();
                        RedrawMap();
                    }
                }
                // type not equal = wall
                else if (nextTile == (int)TileType.BoxPortal || nextTile == (int)TileType.Wall) {
                    return;
                }
                // normal move
                else {
                    moveX = nextX;
                    moveY = nextY;
                }

                if (moveX != playerX || moveY != playerY) {
                    Console.SetCursorPosition(playerX, playerY);
                    Console.Write(" ");
                    playerX = moveX;
                    playerY = moveY;
                    RedrawPlayer();
                }
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
                printInfos();
            }

            void RedrawMap() {
                Console.SetCursorPosition(0, 0);
                CreateMap();
            }

            void RedrawPlayer() {
                Console.SetCursorPosition(playerX, playerY);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("◈");
            }
        }
    }
}