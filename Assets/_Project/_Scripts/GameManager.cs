using UnityEngine;

namespace EternalDefenders
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
