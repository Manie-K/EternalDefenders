using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace EternalDefenders
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] float torque;

        private Rigidbody _rigidbody;
        private string _enemyTag;
        private bool _didHit;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void SetEnemyTag(string enemyTag)
        {
            _enemyTag = enemyTag;
        }

        public void Fly(Vector3 force)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(force, ForceMode.Impulse);
            _rigidbody.AddTorque(transform.right * torque);
            transform.SetParent(null);

            StartCoroutine(DestroyAfterDelay(5f));
        }

        void OnTriggerEnter(Collider collider)
        {
            if (_didHit) return;
            _didHit = true;

            if (collider != null && !string.IsNullOrEmpty(collider.tag) && !string.IsNullOrEmpty(_enemyTag))
            {
                if (collider.CompareTag(_enemyTag))
                {
                    //Enemy take damage
                    EnemyController enemyController = collider.GetComponent<EnemyController>();
                    if (enemyController != null)
                    {
                        DamageCalculator.Instance.BulletHitEnemy(enemyController);
                        //Debug.Log("Enemy hit");
                    }
                    else
                    {
                        Debug.LogWarning("Enemy without EnemyController");
                    }

                }
                
            }
            else
            {
                //Debug.Log("No enemy hit");
            }

            transform.SetParent(collider.transform);
            _rigidbody.isKinematic = true;
            //_rigidbody.linearVelocity = Vector3.zero;
            //_rigidbody.angularVelocity = Vector3.zero;

            StartCoroutine(DestroyAfterDelay(1f));
        }

        IEnumerator DestroyAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }
    }
}
