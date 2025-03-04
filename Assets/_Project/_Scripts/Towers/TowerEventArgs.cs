using System;
using UnityEngine;

namespace EternalDefenders
{
    public class TowerEventArgs : EventArgs
    {
        public bool ShouldPreventDestruction { get; set; }
    }
}
