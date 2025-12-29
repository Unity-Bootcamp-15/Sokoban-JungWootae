namespace Sokoban {
    public class Player {
        public int X { get; set; }
        public int Y { get; set; }
        public int PastX { get; private set; }
        public int PastY { get; private set; }
        public bool IsMagnetActive { get; set; }

        public Player(int startX, int startY) {
            X = startX;
            Y = startY;
            PastX = startX;
            PastY = startY;
            IsMagnetActive = false;
        }

        public void SaveState() {
            PastX = X;
            PastY = Y;
        }

        public void RestoreState() {
            X = PastX;
            Y = PastY;
        }
    }
}