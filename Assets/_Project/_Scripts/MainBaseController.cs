using System;
using HudElements;
using MG_Utilities;
using UnityEngine;
using UnityEngine.UIElements;

namespace EternalDefenders
{
    public class MainBaseController : Singleton<MainBaseController>, IEnemyTarget
    {
        [SerializeField] MainBaseStats statsConfig;

        [SerializeField] private GameObject deathEffectPrefab;
        [SerializeField] private Vector3 deathEffectOffset = new Vector3(0, 0.5f, 0);

        public event Action OnMainBaseDestroyed;
        public Stats Stats
        {
            get => _stats;
            set => _stats = value;
        }

        Stats _stats;

        void Start()
        {
            _stats = new Stats(statsConfig.GetStats());
        }

        void Update()
        {
            if (_stats.GetStat(StatType.Health) <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            if (deathEffectPrefab != null)
            {
                Vector3 spawnPosition = transform.position + deathEffectOffset;
                GameObject deathEffect = Instantiate(deathEffectPrefab, spawnPosition, Quaternion.identity);

                deathEffect.transform.localScale *= 1f;

                Destroy(deathEffect, 5f);
            }

            OnMainBaseDestroyed?.Invoke();
        }
    }
}