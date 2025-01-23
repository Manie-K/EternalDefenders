using MG_Utilities;
using UnityEngine;

namespace EternalDefenders
{
    public class MainBaseController : Singleton<MainBaseController>, IEnemyTarget
    {
        public Stats Stats { get; }
    }
}
