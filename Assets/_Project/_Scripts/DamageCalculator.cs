namespace EternalDefenders
{
    public static class DamageCalculator
    {
        //later we will overload this method to accept different parameters
        public static void CalculateDamage(TowerController attacker, EnemyController target) 
        {
            Stats towerStats = attacker.Stats;
            Stats enemyStats = target.Stats;
            EffectSO effect = attacker.Effect;

            enemyStats.SetStat(StatType.Health, -towerStats.GetStat(StatType.Damage));
            foreach(var modifier in effect.modifiers)
            {
                enemyStats.ApplyModifier(modifier);
            }
            
            //Example usage ^ shown above
        }
    }
}