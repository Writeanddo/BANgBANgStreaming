using System;
using System.Collections.Generic;
using System.Linq;
using Daipan.Enemy.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Daipan.LevelDesign.Enemy.Scripts;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Enemy/ManagerParameters", order = 1)]
public sealed class EnemyParamsManager : ScriptableObject
{
    [Header("エネミー全体のレベルデザインはこちら")]
    [Space(30)]
    
    [Header("エネミー生成のクールタイム")]
    [Min(0)]
    public float spawnDelaySec;

    [Header("ボスの生成周期 (n回通常敵を倒したら生成)")]
    [Min(0)]
    public int spawnBossAmount;

    [Header("現在通常敵討伐数（自動更新されます）")]
    public int currentKillAmount;


    [Header("使用するエネミーを設定してください。")]
    public List<EnemyParam> enemyParams = null!;
}

[Serializable]
public sealed class EnemySpawnParam
{
    [Header("エネミーの生成割合 (相対的に指定可)")]
    [Min(0)]
    public float spawnRatio;

    [Header("台パンの影響を受けるイライラ度の閾値")]
    [Min(0)]
    public int daipanThreshold;

    [Header("台パンされたときに死ぬ確率 (0～1)")]
    [Range(0.0f, 1.0f)]
    [Tooltip("最終的には別途、イライラ度に応じた確率シートを作成する")]
    public float daipanProbability; 
}
[Serializable]
public sealed class EnemyAttackParam
{
    [Header("エネミーの攻撃力")] public int attackAmount;

    [Header("攻撃間隔")] [Min(0)] public float attackDelaySec;

    [Header("攻撃範囲")] [Min(0)] public float attackRange;
}

[Serializable]
public sealed class EnemyHpParam
{
    [Header("エネミーのHP")] [Min(0)] public int hpAmount;
}

[Serializable]
public sealed class EnemyMoveParam
{
    [Header("移動速度 [unit/s]")] [Min(0)] 
    public float moveSpeedPerSec;
}

[Serializable]
public sealed class EnemyParam
{
    [Header("エネミーのレベルデザインはこちら！！")]
    [Space(30)]
    [Header("エネミーのタイプ")]
    [Tooltip("None      : 設定しないでください-_-\n" +
             "W         : Wキーで倒せる敵\n" +
             "A         : Aキーで倒せる敵\n" +
             "S         : Sキーで倒せる敵\n" +
             "Boss      ; ボス")]
    [SerializeField]
    EnemyType enemyType = EnemyType.None;
    


    [Header("このエネミーを倒したときの視聴者数の変化")] public float diffViewer;


    [Header("スプライト")] public Sprite sprite = null!;

    public EnemySpawnParam EnemySpawnParam = null!;
    public EnemyAttackParam EnemyAttackParam = null!;
    public EnemyHpParam EnemyHpParam = null!;
    public EnemyMoveParam EnemyMoveParam = null!;

    public EnemyEnum GetEnemyEnum
    {
        get
        {
            EnemyEnumChecker.CheckEnum();
            return EnemyEnum.Values.First(x => x.Name == enemyType.ToString());
        }
    }

    static class EnemyEnumChecker
    {
        static bool _isCheckedEnum;

        public static void CheckEnum()
        {
            if (_isCheckedEnum) return;
            foreach (var type in Enum.GetValues(typeof(EnemyType)).Cast<EnemyType>())
            {
                var enemy = EnemyEnum.Values.FirstOrDefault(x => x.Name == type.ToString());
                if (enemy.Equals(default(EnemyEnum)))
                    Debug.LogWarning($"EnemyEnum with name {type.ToString()} not found.");
            }

            _isCheckedEnum = true;
        }
    }

    enum EnemyType
    {
        None,
        W,
        A,
        S,
        Boss
    }
}