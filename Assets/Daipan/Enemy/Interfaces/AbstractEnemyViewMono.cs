#nullable enable
using Daipan.Enemy.Scripts;
using Daipan.LevelDesign.Enemy.Scripts;
using UnityEngine;

namespace Daipan.Enemy.Interfaces
{
    public abstract class AbstractEnemyViewMono : MonoBehaviour
    {
        public abstract void SetDomain(EnemyParamDataContainer enemyParamDataContainer);
        public abstract void SetView(EnemyEnum enemyEnum);
        public abstract void SetHpGauge(int currentHp, int maxHp);
        public abstract void Move();
        public abstract void Attack();
        public abstract void Died();
        public abstract void Daipaned();

    }
}