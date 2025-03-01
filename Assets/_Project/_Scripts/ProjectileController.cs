using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace EternalDefenders
{
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] float speed = 2f;
        [SerializeField] float hitDistance = 0.1f;

        public event Action<EnemyController> OnTargetHit;
        
        EnemyController _target;
        
        public void Launch(EnemyController target)
        {
            _target = target;
            StartCoroutine(MoveTowardsTarget());
        }
        
        IEnumerator MoveTowardsTarget()
        {
            while (_target != null)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position, _target.transform.position, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, _target.transform.position) < hitDistance)
                {
                    OnTargetHit?.Invoke(_target);
                    Destroy(gameObject);                    
                }
                yield return null;
            }
            
            Destroy(gameObject);
        }
        
    }
}