using Codice.Client.Common;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace EternalDefenders
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

        private float aimingTime;

        void Start()
        {
            gun.SetEnemyTag(enemyTag);
            gun.Reload();
            aimingTime = 0f;
            PlayerController playerController = GetComponentInParent<PlayerController>();
            playerController.OnPlayerAiming += ChangePlayerAimingTime;
        }


        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                StartCoroutine(WaitForFightAndFire());
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

        private IEnumerator WaitForFightAndFire()
        {
            yield return new WaitForSeconds(aimingTime);

            firePower = maxFirePower;
            gun.Fire(firePower);
            firePower = 0;
            aimingTime = 0f;
            //Debug.Log("fire");
        }

        private void ChangePlayerAimingTime(float time)
        {
            aimingTime = time;
        }
    }
}
