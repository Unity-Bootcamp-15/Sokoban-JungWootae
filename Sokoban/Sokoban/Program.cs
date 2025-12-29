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
        public enum Direction {
            None,
            Up,
            Down,
            Left,
            Right
        }

        internal class Program {
            static void Main(string[] args) {
                Game game = new Game();
                game.Run();
            }
        }
    }