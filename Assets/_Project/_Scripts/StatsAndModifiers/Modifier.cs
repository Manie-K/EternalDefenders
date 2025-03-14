using System;
using UnityEngine;

namespace EternalDefenders
{
    public abstract class Modifier : ScriptableObject
    {
        public StatType statType;
        public ModifierType modifierType;
        public bool persistAfterFinish = false;
        public float limitedDurationTime = 0;
        public int value;
        public bool IsFinished => _isFinished;
        public bool IsDirty => _isDirty;
        
        CountdownTimer _durationTimer;
        bool _isFinished = false;
        bool _isDirty = true;

        protected virtual void InitOnCreation(StatType statType, ModifierType modifierType, bool persistAfterFinish, 
            float limitedDurationTime, int value)
        {
            this.statType = statType;
            this.modifierType = modifierType;
            this.persistAfterFinish = persistAfterFinish;
            this.limitedDurationTime = limitedDurationTime;
            this.value = value;
            _isFinished = false;
            _isDirty = true;
            _durationTimer = null;
        }
        
        public abstract Modifier CreateCopy();
        
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