using System;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "OverTimeModifier", menuName = "EternalDefenders/Modifiers/OverTimeModifier")]
    public class OverTimeModifier : InstantModifier
    {
        public float tickRate;
        public int tickValue;
        
        CountdownTimer _tickTimer;
        public override void InitModifer()
        {
            base.InitModifer();
            _tickTimer = new CountdownTimer(tickRate);
            _tickTimer.OnTimerStop += OnTick;
            _tickTimer.Start();
        }
        public override void UpdateModifier(float dt)
        {
            base.UpdateModifier(dt);
            if(_tickTimer.IsRunning)
                _tickTimer.Tick(dt);
            else if(_tickTimer.IsFinished)
                _tickTimer.Start();
        }

        void OnTick()
        {
            value += tickValue;
        }
    }
}