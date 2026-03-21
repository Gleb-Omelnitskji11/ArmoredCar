namespace Core.BusEvents
{
    public class GameResultEvent : IEvent
    {
        public bool IsWin;

        public GameResultEvent(bool isWin)
        {
            IsWin = isWin;
        }
    }
    
    public class RestartEvent : IEvent
    {
        public bool IsFirstGame;

        public RestartEvent(bool isFirstGame = false)
        {
            IsFirstGame = isFirstGame;
        }
    }
    
    public class PauseEvent : IEvent
    {
        public bool IsPause;

        public PauseEvent(bool isPause)
        {
            IsPause = isPause;
        }
    }
}
