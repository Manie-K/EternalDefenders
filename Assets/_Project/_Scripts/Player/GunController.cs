using UnityEngine;
using UnityEngine.UI;

namespace EternalDefendersPrototype
{
    public class GunController : MonoBehaviour
    {
        [SerializeField]
        private Gun gun;

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
            gun.SetEnemyTag(enemyTag);
            gun.Reload();
        }

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                //fire = true;
                firePower = maxFirePower;
                gun.Fire(firePower);
                firePower = 0;
            }

            //if (fire && firePower < maxFirePower)
            //{
            //    firePower += Time.deltaTime * firePowerSpeed;
            //}

            //if (fire && Input.GetMouseButtonUp(0))
            //{
            //    gun.Fire(firePower);
            //    firePower = 0;
            //    fire = false;
            //}

            //if (fire)
            //{
            //    firePowerText.text = firePower.ToString();
            //}
        }
    }
}
