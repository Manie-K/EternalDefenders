using MG_Utilities;
using UnityEngine;

namespace EternalDefenders
{
    public class MouseGroundPointer : MonoBehaviour
    {
        void Update()
        {
            transform.position = CameraController.Instance.GetWorldMousePosition().With(y: 0);
        }
    }
}
