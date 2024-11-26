using System;

namespace Match3
{
    public class EventManager 
    {
        public Action Start;
        public Action Stop;

        public Action<int> AddScore;
        public Action<int> ChangeScore;
        public Action<int> ChangeBestScore;
    }
}
