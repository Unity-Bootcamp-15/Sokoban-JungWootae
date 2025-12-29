namespace Sokoban {
    public class Map {
        public int[,] Data { get; private set; }
        public int[,] PastData { get; private set; }
        public (int, int)[] PlayerPortals { get; private set; }
        public (int, int)[] BoxPortals { get; private set; }

        public Map() {
            Data = new int[,]
            {
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                { 1, 0, 6, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 1},
                { 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1},
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

            PastData = new int[Data.GetLength(0), Data.GetLength(1)];
            PlayerPortals = new (int, int)[2];
            BoxPortals = new (int, int)[2];
            
            // Initialize PastData
            for (int i = 0; i < Data.GetLength(0); i++)
                for (int j = 0; j < Data.GetLength(1); j++)
                    PastData[i, j] = Data[i, j];
            
            FindPortals();
        }

        public void FindPortals() {
            int pIdx = 0, bIdx = 0;
            for (int i = 0; i < Data.GetLength(0); i++) {
                for (int j = 0; j < Data.GetLength(1); j++) {
                    if (Data[i, j] == (int)TileType.PlayerPortal && pIdx < 2)
                        PlayerPortals[pIdx++] = (i, j);
                    else if (Data[i, j] == (int)TileType.BoxPortal && bIdx < 2)
                        BoxPortals[bIdx++] = (i, j);
                }
            }
        }

        public void SaveState() {
            for (int i = 0; i < Data.GetLength(0); i++)
                for (int j = 0; j < Data.GetLength(1); j++)
                    PastData[i, j] = Data[i, j];
        }

        public void RestoreState() {
            for (int i = 0; i < Data.GetLength(0); i++)
                for (int j = 0; j < Data.GetLength(1); j++)
                    Data[i, j] = PastData[i, j];
            FindPortals(); // Re-find portals as map changed
        }

        public int GetHeight() => Data.GetLength(0);
        public int GetWidth() => Data.GetLength(1);
    }
}