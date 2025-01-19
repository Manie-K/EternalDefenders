using System.Collections;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "ProjectileAttack", menuName = "EternalDefenders/Tower/AttackStrategies/ProjectileAttack")]
    public class TowerProjectileAttackStrategy : TowerAttackStrategy
    {
        public ProjectileController projectilePrefab;
        public override void Attack(EnemyController target)
        {
            Debug.Log("Projectile attack!");
            
            var projectile = Instantiate(projectilePrefab, tower.AttackPoint.position, Quaternion.identity);
            //projectile.Init();
        }
    }
}