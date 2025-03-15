using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace EternalDefenders
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private int damage;

        [SerializeField]
        private float torque;

        private Rigidbody _rigidbody;
        private string _enemyTag;
        private bool _didHit;

        public Stats Stats { get; private set; }

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            var initialStats = new Dictionary<StatType, Stats.Stat>
            {
                { StatType.Damage, new Stats.Stat(this.damage) }
            };

            Stats = new Stats(initialStats);
        }

        public void SetEnemyTag(string enemyTag)
        {
            this._enemyTag = enemyTag;
        }

        public void Fly(Vector3 force)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(force, ForceMode.Impulse);
            _rigidbody.AddTorque(transform.right * torque);
            transform.SetParent(null);
            //Debug.Log("No parent");
            StartCoroutine(DestroyAfterDelay(5f));
        }

        void OnTriggerEnter(Collider collider)
        {
            if (_didHit) return;
            _didHit = true;

            if (collider.CompareTag(_enemyTag))
            {
                //take damage - enemy
                EnemyController enemyController = collider.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    Bullet bullet = gameObject.GetComponent<Bullet>();
                    DamageCalculator.BulletHitEnemy(bullet, enemyController);
                    Debug.Log("Enemy hit");
                }
                else
                {
                    Debug.LogWarning("Enemy without EnemyController");
                }
                
            }

            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.isKinematic = true;
            transform.SetParent(collider.transform);
            //Debug.Log("New parent");

            StartCoroutine(DestroyAfterDelay(1f));
        }

        IEnumerator DestroyAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }
    }
}
