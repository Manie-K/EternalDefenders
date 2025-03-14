using UnityEngine;
using System.Collections;

namespace EternalDefendersPrototype
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private float damage;

        [SerializeField]
        private float torque;

        private Rigidbody _rigidbody;

        private string _enemyTag;

        private bool _didHit;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
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
            //StartCoroutine(DestroyAdterDelay(5f));
        }

        void OnTriggerEnter(Collider collider)
        {
            if (_didHit) return;
            _didHit = true;

            if (collider.CompareTag(_enemyTag))
            {
                //take damage - enemy
            }

            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.isKinematic = true;
            transform.SetParent(collider.transform);
            Debug.Log("New parent");

            //StartCoroutine(DestroyAdterDelay(1f));
        }

        IEnumerator DestroyAdterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }
    }
}
