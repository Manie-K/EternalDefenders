using UnityEngine;
using UnityEngine.UI;

namespace EternalDefendersPrototype
{
    public class CrossbowController : MonoBehaviour
    {
        [SerializeField]
        private Crossbow crossbow;

        [SerializeField]
        private string enemyTag;

        [SerializeField]
        private float maxFirePower;

        [SerializeField]
        private float firePowerSpeed;

        private float firePower;

        [SerializeField]
        private float rotateSpeed;

        [SerializeField]
        private float minRotation;

        [SerializeField]
        private float maxRotation;

        private float mouseY;

        private bool fire;

        void Start()
        {
            crossbow.SetEnemyTag(enemyTag);
            crossbow.Reload();

            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            //mouseY -= Input.GetAxis("Mouse Y") * rotateSpeed;
            //mouseY = Mathf.Clamp(mouseY, minRotation, maxRotation);
            //crossbow.transform.localRotation = Quaternion.Euler(mouseY, crossbow.transform.localEulerAngles.y, crossbow.transform.localEulerAngles.z);

            if (Input.GetMouseButtonDown(0))
            {
                fire = true;
            }

            if (fire && firePower < maxFirePower)
            {
                firePower += Time.deltaTime * firePowerSpeed;
            }

            if (fire && Input.GetMouseButtonUp(0))
            {
                crossbow.Fire(firePower);
                firePower = 0;
                fire = false;
            }

            if (fire)
            {
                //firePowerText.text = firePower.ToString();
            }
        }
    }
}
