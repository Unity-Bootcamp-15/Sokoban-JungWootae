using System;

namespace Sokoban {
    public class Game {
        private Map _map;
        private Player _player;
        private Renderer _renderer;
        private int _maxScore;
        private int _curScore;
        private bool _canUndo;

        public Game() {
            InitializeGameState();
        }

        private void InitializeGameState() {
            _map = new Map();
            _player = new Player(3, 3); // Initial position
            _renderer = new Renderer();
            
            _curScore = 0;
            _canUndo = false;
            _maxScore = CalculateMaxScore();
        }

        private int CalculateMaxScore() {
            int maxScore = 0;
            foreach (int tile in _map.Data) {
                if (tile == (int)TileType.Box) {
                    maxScore++;
                }
            }
            return maxScore;
        }

        public void Run() {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.CursorVisible = false;
            Console.Title = "(  {==[= S.o.k.o.b.a.n =]==}  )";

            _renderer.DrawMap(_map, _curScore, _maxScore, _player.IsMagnetActive);
            _renderer.DrawPlayer(_player);

            while (true) {
                if (_curScore == _maxScore) {
                    Console.Clear();
                    Console.WriteLine("\n\n=== You Clear Game!!! ===\n\n");
                    Console.WriteLine("Press ESC to Exit.");
                    if (Console.ReadKey(true).Key == ConsoleKey.Escape) {
                        Environment.Exit(0);
                    }
                }

                var key = Console.ReadKey(true).Key;
                HandleInput(key);
            }
        }

        private void HandleInput(ConsoleKey key) {
            switch (key) {
                case ConsoleKey.UpArrow:
                    MovePlayer(Direction.Up);
                    break;

                case ConsoleKey.DownArrow:
                    MovePlayer(Direction.Down);
                    break;

                case ConsoleKey.LeftArrow:
                    MovePlayer(Direction.Left);
                    break;

                case ConsoleKey.RightArrow:
                    MovePlayer(Direction.Right);
                    break;

                case ConsoleKey.Z:
                    TryUndo();
                    break;

                case ConsoleKey.M:
                    ToggleMagnet();
                    break;
            }
        }

        private void TryUndo() {
            if (_canUndo) {
                _map.RestoreState();
                _player.RestoreState();
                _renderer.DrawMap(_map, _curScore, _maxScore, _player.IsMagnetActive);
                _renderer.DrawPlayer(_player);
            }
        }

        private void ToggleMagnet() {
            int nearBoxes = 0;
            if (_map.Data[_player.Y - 1, _player.X] == (int)TileType.Box) nearBoxes++;
            if (_map.Data[_player.Y + 1, _player.X] == (int)TileType.Box) nearBoxes++;
            if (_map.Data[_player.Y, _player.X - 1] == (int)TileType.Box) nearBoxes++;
            if (_map.Data[_player.Y, _player.X + 1] == (int)TileType.Box) nearBoxes++;

            if (nearBoxes == 1) _player.IsMagnetActive = !_player.IsMagnetActive;

            _renderer.DrawMap(_map, _curScore, _maxScore, _player.IsMagnetActive);
            _renderer.DrawPlayer(_player);
        }

        private void MovePlayer(Direction dir) {
            _map.SaveState();
            _player.SaveState();
            _canUndo = true;

            int dx = 0, dy = 0;
            switch (dir) {
                case Direction.Up:
                    dy = -1;
                    break;

                case Direction.Down:
                    dy = 1;
                    break;

                case Direction.Left:
                    dx = -1;
                    break;

                case Direction.Right:
                    dx = 1;
                    break;

                default: return;
            }

            int nextX = _player.X + dx;
            int nextY = _player.Y + dy;

            // Magnet Mode Logic
            if (_player.IsMagnetActive && (dx != 0 || dy != 0)) {
                int boxX = -1, boxY = -1;
                // Find the attached box
                if (_map.Data[_player.Y - 1, _player.X] == (int)TileType.Box) {
                    boxX = _player.X;
                    boxY = _player.Y - 1;
                }

                else if (_map.Data[_player.Y + 1, _player.X] == (int)TileType.Box) {
                    boxX = _player.X;
                    boxY = _player.Y + 1;
                }

                else if (_map.Data[_player.Y, _player.X - 1] == (int)TileType.Box) {
                    boxX = _player.X - 1;
                    boxY = _player.Y;
                }

                else if (_map.Data[_player.Y, _player.X + 1] == (int)TileType.Box) {
                    boxX = _player.X + 1;
                    boxY = _player.Y;
                }

                if (boxX != -1) {
                    int nextBoxX = boxX + dx;
                    int nextBoxY = boxY + dy;

                    int targetTileForBox = _map.Data[nextBoxY, nextBoxX];
                    bool boxBlocked = (targetTileForBox == (int)TileType.Wall || targetTileForBox == (int)TileType.Box ||
                                       targetTileForBox == (int)TileType.BoxPortal || targetTileForBox == (int)TileType.PlayerPortal);

                    if (nextBoxX == _player.X && nextBoxY == _player.Y)
                        boxBlocked = false;

                    int targetTileForPlayer = _map.Data[nextY, nextX];
                    bool playerBlocked = (targetTileForPlayer == (int)TileType.Wall || targetTileForPlayer == (int)TileType.BoxPortal ||
                                          targetTileForPlayer == (int)TileType.PlayerPortal);

                    if (targetTileForPlayer == (int)TileType.Box && (nextX != boxX || nextY != boxY))
                        playerBlocked = true;

                    else if (targetTileForPlayer == (int)TileType.Box)
                        playerBlocked = false;

                    if (!boxBlocked && !playerBlocked) {
                        _map.Data[boxY, boxX] = 0;
                        if (_map.Data[nextBoxY, nextBoxX] == (int)TileType.Goal) {
                            _map.Data[nextBoxY, nextBoxX] = (int)TileType.Goal;
                            _curScore++;
                        }
                        else {
                            _map.Data[nextBoxY, nextBoxX] = (int)TileType.Box;
                        }

                        _renderer.ClearPlayer(_player.X, _player.Y);
                        _player.X = nextX;
                        _player.Y = nextY;
                        _renderer.DrawMap(_map, _curScore, _maxScore, _player.IsMagnetActive);
                        _renderer.DrawPlayer(_player);
                        return;
                    }
                }
                else {
                    _player.IsMagnetActive = false;
                    _renderer.DrawMap(_map, _curScore, _maxScore, _player.IsMagnetActive);
                    _renderer.DrawPlayer(_player);
                }
            }

            int nextTile = _map.Data[nextY, nextX];

            if (nextTile == (int)TileType.Box) {
                int boxNextX = nextX + dx;
                int boxNextY = nextY + dy;
                int boxNextTile = _map.Data[boxNextY, boxNextX];

                // box in box portal
                if (_map.Data[boxNextY, boxNextX] == (int)TileType.BoxPortal) {
                    int idx = (_map.BoxPortals[0] == (boxNextY, boxNextX)) ? 1 : 0;
                    var (destY, destX) = _map.BoxPortals[idx];
                    
                    if (_map.Data[destY, destX] == (int)TileType.BoxPortal) {
                        _map.Data[boxNextY, boxNextX] = 0;
                        _map.Data[destY, destX] = 0;
                        _map.Data[nextY, nextX] = 0;
                        _map.Data[destY, destX] = (int)TileType.Box;
                        
                        _renderer.ClearPlayer(_player.X, _player.Y);
                        _player.X = nextX;
                        _player.Y = nextY;
                        _map.FindPortals();
                        _renderer.DrawMap(_map, _curScore, _maxScore, _player.IsMagnetActive);
                        _renderer.DrawPlayer(_player);
                    }
                    return;
                }

                if (_map.Data[boxNextY, boxNextX] == (int)TileType.PlayerPortal || boxNextTile == (int)TileType.Wall || boxNextTile == (int)TileType.Box)
                    return;

                if (boxNextTile == (int)TileType.Goal) {
                    _map.Data[boxNextY, boxNextX] = (int)TileType.Goal;
                    _curScore++;
                }
                else {
                    _map.Data[boxNextY, boxNextX] = (int)TileType.Box;
                }
                _map.Data[nextY, nextX] = 0;
                
                _renderer.ClearPlayer(_player.X, _player.Y);
                _player.X = nextX;
                _player.Y = nextY;
                _renderer.DrawMap(_map, _curScore, _maxScore, _player.IsMagnetActive);
                _renderer.DrawPlayer(_player);
            }
            else if (nextTile == (int)TileType.PlayerPortal) {
                int idx = (_map.PlayerPortals[0] == (nextY, nextX)) ? 1 : 0;
                var (destY, destX) = _map.PlayerPortals[idx];
                
                if (_map.Data[destY, destX] == (int)TileType.PlayerPortal) {
                    _map.Data[nextY, nextX] = 0;
                    _map.Data[destY, destX] = 0;
                    
                    _renderer.ClearPlayer(_player.X, _player.Y);
                    _player.X = destX;
                    _player.Y = destY;
                    _map.FindPortals();
                    _renderer.DrawMap(_map, _curScore, _maxScore, _player.IsMagnetActive);
                    _renderer.DrawPlayer(_player);
                }
            }
            else if (nextTile == (int)TileType.BoxPortal || nextTile == (int)TileType.Wall) {
                return;
            }
            else {
                _renderer.ClearPlayer(_player.X, _player.Y);
                _player.X = nextX;
                _player.Y = nextY;
                _renderer.DrawPlayer(_player);
            }
        }
    }
}