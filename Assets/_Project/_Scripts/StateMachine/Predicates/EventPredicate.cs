using System;
using System.Reflection;
using UnityEngine;
using Object = System.Object;

namespace EternalDefenders
{
    public class EventPredicate : IPredicate, IDisposable
    {
        readonly EventInfo _eventInfo;
        readonly Object _eventObject;
        readonly Delegate _eventMethod;
        bool _flag;
        bool _hasBeenEvaluated;
        
        public EventPredicate(string eventName, Object eventOwnerObject)
        {
            _eventObject = eventOwnerObject;
            _eventInfo = _eventObject.GetType().GetEvent(eventName);
            Type eventHandlerType = _eventInfo.EventHandlerType;

            if (eventHandlerType == typeof(Action)) {
                _eventMethod = new Action(SetFlag);
            }else if (eventHandlerType == typeof(EventHandler)){
                _eventMethod = new EventHandler(SetFlag);
            }
            else {
                Debug.LogError("Unsupported event type!");
            }
            
            _eventInfo.AddEventHandler(_eventObject, _eventMethod);
        }
        
        //<summary>
        //Not sure if it works, but it should remove the event handler from the object when deleting
        //</summary>
        public void Dispose()
        {
            _eventInfo.RemoveEventHandler(_eventObject, _eventMethod);
        }

        void SetFlag()
        {
            _flag = true;
        }
        
        void SetFlag(object sender, EventArgs e)
        {
            if (sender == _eventObject) _flag = true;
        }

        public bool Evaluate()
        {
            bool result = _flag && _hasBeenEvaluated;
            _hasBeenEvaluated = true;
            _flag = false;
            if (result)
            {
                _hasBeenEvaluated = false;
            }
            return result;
        }
    }
}