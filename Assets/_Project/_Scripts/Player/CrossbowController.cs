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
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                fire = true;
            }

            if (fire && firePower < maxFirePower)
            {
                firePower += Time.deltaTime * firePowerSpeed;
            }

            if (fire && Input.GetMouseButtonUp(0) && false)
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
