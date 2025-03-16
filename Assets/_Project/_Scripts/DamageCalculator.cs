using MG_Utilities;
using UnityEngine;

namespace EternalDefenders
{
    public static class DamageCalculator
    {
        //TODO: Refactor this mess completely
        //later we will overload this method to accept different parameters
        public static void TowerAttackEnemy(TowerController attacker, EnemyController target) 
        {
            Stats towerStats = attacker.Stats;
            Stats enemyStats = target.Stats;
            Effect effect = attacker.Effect;

            enemyStats.ChangeStat(StatType.Health, -towerStats.GetStat(StatType.Damage));
            towerStats.ChangeStat(StatType.Health, -enemyStats.GetStat(StatType.ReturnDamage));
            
            foreach(var modifier in effect.modifiers)
            {
                enemyStats.ApplyModifier(modifier);
            }

            if(effect.particleSystem != null)
            {
                var particles = Object.Instantiate(effect.particleSystem, target.transform);
                particles?.Play();
            }
            DamagePopupText.Create(target.transform.position.With(y:1f), towerStats.GetStat(StatType.Damage));
            
            if(enemyStats.GetStat(StatType.Health) <= 0)
            {
                target.Die();
            }
        }
        
        public static void EnemyAttackTower(EnemyController attacker, TowerController target) 
        {
            Stats towerStats = target.Stats;
            Stats enemyStats = attacker.Stats;
            Effect effect = attacker.Effect;

            towerStats.ChangeStat(StatType.Health, -enemyStats.GetStat(StatType.Damage));
            
            foreach(var modifier in effect.modifiers)
            {
                towerStats.ApplyModifier(modifier);
            }
            DamagePopupText.Create(target.transform.position.With(y:3f), enemyStats.GetStat(StatType.Damage));
            
            //check if tower is dead
        }
        
        public static void EnemyAttackMainBase(EnemyController attacker, MainBaseController target) 
        {
            Stats mainBaseStats = target.Stats;
            Stats enemyStats = attacker.Stats;
            Effect effect = attacker.Effect;
            
            mainBaseStats.ChangeStat(StatType.Health, -enemyStats.GetStat(StatType.Damage));
            foreach(var modifier in effect.modifiers)
            {
                mainBaseStats.ApplyModifier(modifier);
            }
        }

        public static void EnemyAttackPlayer(EnemyController attacker, PlayerController player)
        {
            Stats playerStats = player.Stats;
            Stats enemyStats = attacker.Stats;
            Effect effect = attacker.Effect;
            
            playerStats.ChangeStat(StatType.Health, -enemyStats.GetStat(StatType.Damage));
            foreach(var modifier in effect.modifiers)
            {
                playerStats.ApplyModifier(modifier);
            }
            
            
        }

        public static void BulletHitEnemy(Bullet bullet, EnemyController enemy)
        {
            Stats bulletStats = bullet.Stats;
            Stats enemyStats = enemy.Stats;

            enemyStats.ChangeStat(StatType.Health, -bulletStats.GetStat(StatType.Damage));
        }
    }
}