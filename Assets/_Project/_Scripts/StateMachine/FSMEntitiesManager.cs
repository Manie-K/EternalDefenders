﻿using System;
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
            if(entity && _managedEntities.Contains(entity)) return;
            _managedEntities.Add(entity);
        }
        
        public void UnregisterEntity(StateMachineBrain entity)
        {
            if (entity && !_managedEntities.Contains(entity)) return;
            _managedEntities.Remove(entity);
        }
        
        void Update()
        {
            /* In the future, we can use Parallel.ForEach to optimize the update process
            Parallel.ForEach(_managedEntities, entity =>
            {
                entity.OnUpdate();
            });*/

            for(int i = 0; i < _managedEntities.Count; i++)
            {
                _managedEntities[i].OnUpdate();
            }
        }
        
        void FixedUpdate()
        {
            //In the future, we can use Parallel.ForEach to optimize the fixed update process
            for(int i = 0; i < _managedEntities.Count; i++)
            {
                _managedEntities[i].OnFixedUpdate();
            }
        }
    }
}