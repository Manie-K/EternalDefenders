using System;
using UnityEngine;

namespace EternalDefenders
{
    public abstract class Modifier : ScriptableObject
    {
        public StatType statType;
        public ModifierType modifierType;
        public float limitedDurationTime = 0;
        public int value;
        public bool IsFinished => _isFinished;
        public bool IsDirty => _isDirty;
        
        CountdownTimer _durationTimer;
        bool _isFinished = false;
        bool _isDirty = true;
        
        public virtual void InitModifer()
        {
            if(limitedDurationTime > 0)
            {
                _durationTimer = new CountdownTimer(limitedDurationTime);
                _durationTimer.OnTimerStop += Finish;
                _durationTimer.Start();
            }
        }
        
        public virtual void UpdateModifier(float dt)
        {
            _durationTimer?.Tick(dt);
        }
        public void SetClean() => _isDirty = false;
        void Finish() => _isFinished = true;
    }
}