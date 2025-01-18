namespace EternalDefenders
{
    public class CountdownTimer : Timer //Implement Disposing of the timer
    {
        public override bool IsFinished => currentTime <= 0;
        public CountdownTimer(float value) : base(value)
        {
        }
        
        public override void Tick(float deltaTime)
        {
            if(IsRunning && currentTime > 0)
            {
                currentTime -= deltaTime;
            }
            if(IsRunning && currentTime <= 0)
            {
                Stop();
            }
        }
    }
}