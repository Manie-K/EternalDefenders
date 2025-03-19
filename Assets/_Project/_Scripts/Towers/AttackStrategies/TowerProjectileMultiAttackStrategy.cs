using MG_Utilities;
using UnityEditor;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "ProjectileMultiAttack", menuName = "EternalDefenders/Tower/AttackStrategies/ProjectileMultiAttack")]
    public class TowerProjectileMultiAttackStrategy : TowerAttackStrategy
    {
        public ProjectileController projectilePrefab;
        public float explosionRadius = 0f;
        public ParticleSystem explosionEffect;
        public LayerMask enemyLayer;
        public override void Attack(TowerController tower, EnemyController target)
        {
            var projectile = Instantiate(projectilePrefab, tower.AttackPoint.position, Quaternion.identity);
            projectile.Launch(target);
            
            projectile.OnTargetHit += (enemy) =>
            { 
                var particleSystem = Instantiate(explosionEffect, enemy.transform.position, Quaternion.identity);
                particleSystem.Play();
                
                //TODO: Refactor to NonAlloc version
                Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, explosionRadius, enemyLayer);
                foreach(var collider in colliders)
                {
                    EnemyController enemyController = collider.gameObject.GetComponent<EnemyController>();
                    if(enemyController != null)
                    {
                        DamageCalculator.Instance.TowerAttackEnemy(tower, enemyController);
                    }
                }
            };
        }
        
    }
}