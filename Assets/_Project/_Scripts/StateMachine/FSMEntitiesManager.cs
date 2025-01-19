using System.Collections.Generic;
using MG_Utilities;
using UnityEngine;

namespace EternalDefenders
{
    public class FSMEntitiesManager : Singleton<FSMEntitiesManager>
    {
        readonly List<StateMachineBrain> _managedEntities = new();
        
        public void RegisterEntity(StateMachineBrain entity)
        {
            if(_managedEntities.Contains(entity)) return;
            _managedEntities.Add(entity);
        }
        
        public void UnregisterEntity(StateMachineBrain entity)
        {
            if (!_managedEntities.Contains(entity)) return;
            _managedEntities.Remove(entity);
        }
        
        void Update()
        {
            /* In the future, we can use Parallel.ForEach to optimize the update process
            Parallel.ForEach(_managedEntities, entity =>
            {
                entity.OnUpdate();
            });*/
            
            foreach (var entity in _managedEntities)
            {
                entity.OnUpdate();
            }
        }
        
        void FixedUpdate()
        {
            //In the future, we can use Parallel.ForEach to optimize the fixed update process
            foreach (var entity in _managedEntities)
            {
                entity.OnFixedUpdate();
            }
        }
    }
}