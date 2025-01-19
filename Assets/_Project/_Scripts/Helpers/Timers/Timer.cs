using System;

namespace EternalDefenders
{
    public abstract class Timer
    {
        public abstract bool IsFinished { get; }
        public bool IsRunning { get; protected set; }
        public float Progress => currentTime / initialTime;
        
        public event Action OnTimerStart;
        public event Action OnTimerStop;
        
        protected float initialTime;
        protected float currentTime;
        
        protected Timer(float defaultInitialTime) {
            initialTime = defaultInitialTime;
            IsRunning = false;
        }

        /// <summary>
        /// Starts the timer with the specified time. If the time parameter is omitted, the default initial time will be used.
        /// </summary>
        /// <param name="time">The time to start the timer with. If 0, the default initial time will be used.</param>
        public void Start(float time = 0) {
            initialTime = time == 0 ? initialTime : time;
            currentTime = initialTime;
            if(!IsRunning) {
                IsRunning = true;
                OnTimerStart?.Invoke();
            }
        }

        public void Stop() {
            if(IsRunning) {
                IsRunning = false;
                OnTimerStop?.Invoke();
            }
        }
        
        public void Resume() => IsRunning = true;
        public void Pause() => IsRunning = false;
        
        public abstract void Tick(float deltaTime);
    }
}