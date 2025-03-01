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
            projectile.Launch(target);
            
            //I'm not sure if this works when second attack is performed before first projectile hits,
            //but let's leave it for now
            projectile.OnTargetHit += (enemy) =>
            {
                DamageCalculator.PerformAttack(tower, enemy);
            };
        }
        
    }
}