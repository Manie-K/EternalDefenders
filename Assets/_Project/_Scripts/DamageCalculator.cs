using System.Collections;
using System.Collections.Generic;
using EternalDefenders.Helpers;
using MG_Utilities;
using UnityEngine;

namespace EternalDefenders
{
    public class DamageCalculator : Singleton<DamageCalculator>
    {
        void PlayParticleSystem(ParticleSystem system, Vector3 position)
        {
            if(system == null) return;
            ParticleSystem particles = Instantiate(system, position, Quaternion.identity);
            particles.Play();
        }
        public void TowerAttackEnemy(TowerController attacker, EnemyController target) 
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
            
            PlayParticleSystem(effect.particleSystem, target.transform.position);
            DamagePopupText.Create(target.transform.position.With(y:1f), towerStats.GetStat(StatType.Damage));
            
            if(enemyStats.GetStat(StatType.Health) <= 0)
            {
                target.Die();
            }
        }
        
        public void EnemyAttackTower(EnemyController attacker, TowerController target) 
        {
            Stats towerStats = target.Stats;
            Stats enemyStats = attacker.Stats;
            Effect effect = attacker.Effect;

            towerStats.ChangeStat(StatType.Health, -enemyStats.GetStat(StatType.Damage));
            
            foreach(var modifier in effect.modifiers)
            {
                towerStats.ApplyModifier(modifier);
            }

            DamagePopupText.Create(target.transform.position.With(y: 3f), enemyStats.GetStat(StatType.Damage));
        }
        
        public void EnemyAttackMainBase(EnemyController attacker, MainBaseController target) 
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

        public void EnemyAttackPlayer(EnemyController attacker, PlayerController player)
        {
            Stats playerStats = player.Stats;
            Stats enemyStats = attacker.Stats;
            Effect effect = attacker.Effect;
            
            playerStats.ChangeStat(StatType.Health, -enemyStats.GetStat(StatType.Damage));
            foreach(var modifier in effect.modifiers)
            {
                playerStats.ApplyModifier(modifier);
            }
            
            DamagePopupText.Create(player.transform.position.With(y: 1.75f), enemyStats.GetStat(StatType.Damage));
            
            if (player.GetState() != PlayerState.Dead)
            {
                player.OnDamage();
            }
        }

        public void BulletHitEnemy(EnemyController enemy)
        {
            Stats playerStats = PlayerController.Instance.Stats;
            Stats enemyStats = enemy.Stats;

            enemyStats.ChangeStat(StatType.Health, -playerStats.GetStat(StatType.Damage));
            DamagePopupText.Create(enemy.transform.position.With(y: 1.75f), playerStats.GetStat(StatType.Damage));
        }
    }
}