#nullable enable
using System;

namespace Daipan.Enemy.LevelDesign.Scripts
{
    public sealed class EnemyLevelDesignParamData
    {
        public Func<int> GetSpawnBossAmount { get; init; } = () => 10;
        public Func<int> GetIncreaseIrritatedValueByBoss { get; init; } = () => 10;
        public Func<int> GetIncreaseViewerOnEnemyKill { get; init; } = () => 5; 
        public Func<int> GetCurrentKillAmount { get; init; } = () => 0;
        public Action<int> SetCurrentKillAmount { get; init; } = _ => { }; 
    }
}