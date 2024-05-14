#nullable enable
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Daipan.Enemy.Scripts
{
    [Serializable]
    public sealed class EnemyAttackParameter
    {
        public int attackAmount;
    }

    [Serializable]
    public sealed class EnemyHpParameter
    {
        public int maxHp;
    }

    [Serializable]
    public sealed class EnemyMovementParameter
    {
        public float speed;
    }

    [Serializable]
    public sealed class EnemyAttackCoolTime
    {
        public float MaxCoolTime;
        public float MinCoolTime;
    }

    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyParameter", order = 1)]
    public sealed class EnemyParameter : ScriptableObject
    {
        [SerializeField] EnemyType enemyType = EnemyType.None;
        public EnemyAttackParameter attack = null!;
        public EnemyHpParameter hp = null!;
        public EnemyMovementParameter movement = null!;
        public EnemyAttackCoolTime attackCoolTime = null!;
        public Sprite sprite = null!;

        public EnemyEnum GetEnemyEnum
        {
            get
            {
                EnemyEnumChecker.CheckEnum();
                return EnemyEnum.Values.First(x => x.Name == enemyType.ToString());
            }
        }
    }

    static class EnemyEnumChecker
    {
        static bool _isCheckedEnum;
        public static void CheckEnum()
        {
            if (_isCheckedEnum) return;
            Debug.Log("CheckEnum");
            Debug.Log($"Enum.GetValues(typeof(EnemyType)).Cast<EnemyType>() : {Enum.GetValues(typeof(EnemyType)).Cast<EnemyType>().Count()}");
            foreach (var type in Enum.GetValues(typeof(EnemyType)).Cast<EnemyType>())
            {
                Debug.Log($"type : {type.ToString()}");
                var enemy = EnemyEnum.Values.FirstOrDefault(x => x.Name == type.ToString());
                Debug.Log($"EnemyEnum with name {type.ToString()} found.");
                if (enemy.Equals(default(EnemyEnum)))
                {
                    Debug.LogWarning($"EnemyEnum with name {type.ToString()} not found.");
                }
            }
            _isCheckedEnum = true;
        }
    }
}