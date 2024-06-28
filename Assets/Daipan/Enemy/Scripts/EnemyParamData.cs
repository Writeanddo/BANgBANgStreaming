#nullable enable
using System;
using Daipan.Enemy.Scripts;
using Daipan.LevelDesign.Enemy.Scripts;
using UnityEngine;

namespace Daipan.Enemy.Scripts
{
    public class EnemyParamData
    {
        // Enum
        public Func<EnemyEnum> GetEnemyEnum { get; init; } = () => EnemyEnum.None;

        // Attack
        public Func<int> GetAttackAmount { get; init; } = () => 10;
        public Func<double> GetAttackDelayDec { get; init; } = () => 1.0;
        public Func<double> GetAttackRange { get; init; } = () => 5.0;

        // Hp
        public Func<int> GetCurrentHp { get; init; } = () => 100;

        // Move
        public Func<double> GetMoveSpeedPreSec { get; init; } = () => 1.0;

        // Spawn
        public Func<double> GetSpawnRatio { get; init; } = () => 1.0;

        // Irritated value
        public Func<int> GetIrritationAfterKill { get; init; } = () => 10;

        // Animator 
        public Func<Color> GetBodyColor { get; init; } = () => Color.white;
        public Func<Color> GetEyeColor { get; init; } = () => Color.white;
        public Func<Color> GetEyeBallColor { get; init; } = () => Color.white;
        public Func<Color> GetLineColor { get; init; } = () => Color.white;
    }
}