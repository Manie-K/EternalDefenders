using Codice.Client.Common;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace EternalDefenders
{
    public class GunController : MonoBehaviour
    {
        [SerializeField] Gun gun;
        [SerializeField] string enemyTag;
        [SerializeField] float maxFirePower;
        [SerializeField] float firePowerSpeed;
        [SerializeField] float rotateSpeed;
        [SerializeField] float minRotation;
        [SerializeField] float maxRotation;

        private float _firePower;

        private float _mouseY;

        private bool _fire;

        private float _aimingTime;

        void Awake()
        {
            gun.SetEnemyTag(enemyTag);
            gun.Reload();
            _aimingTime = 0f;
        }

        void Start()
        {
            PlayerController playerController = GetComponentInParent<PlayerController>();
            playerController.OnPlayerAiming += ChangePlayerAimingTime;
        }


        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                StartCoroutine(WaitForFightAndFire());
            }

            //if (_fire && _firePower < maxFirePower)
            //{
            //    _firePower += Time.deltaTime * firePowerSpeed;
            //}

            //if (_fire && Input.GetMouseButtonUp(0))
            //{
            //    gun.Fire(_firePower);
            //    _firePower = 0;
            //    _fire = false;
            //}

            //if (_fire)
            //{
            //    firePowerText.text = _firePower.ToString();
            //}
        }

        private IEnumerator WaitForFightAndFire()
        {
            yield return new WaitForSeconds(_aimingTime);

            _firePower = maxFirePower;
            gun.Fire(_firePower);
            _firePower = 0;
            _aimingTime = 0f;
            //Debug.Log("_fire");
        }

        private void ChangePlayerAimingTime(float time)
        {
            _aimingTime = time;
        }
    }
}
