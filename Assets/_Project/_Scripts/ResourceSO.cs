using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "ResourceSO", menuName = "EternalDefenders/Resource/ResourceSO")]
    public class ResourceSO : ScriptableObject
    {
        public string Name;
        public Sprite Sprite;
    }
}