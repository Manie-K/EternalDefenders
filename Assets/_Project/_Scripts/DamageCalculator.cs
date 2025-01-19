namespace EternalDefenders
{
    public static class DamageCalculator
    {
        //later we will overload this method to accept different parameters
        public static void PerformAttack(TowerController attacker, EnemyController target) 
        {
            Stats towerStats = attacker.Stats;
            Stats enemyStats = target.Stats;
            Effect effect = attacker.Effect;

            enemyStats.SetStat(StatType.Health, -towerStats.GetStat(StatType.Damage));
            towerStats.SetStat(StatType.Health, -enemyStats.GetStat(StatType.ReturnDamage));
            
            foreach(var modifier in effect.modifiers)
            {
                enemyStats.ApplyModifier(modifier);
            }
            
            //effect.particleSystem.Play();
            //Example usage ^ shown above
        }
    }
}