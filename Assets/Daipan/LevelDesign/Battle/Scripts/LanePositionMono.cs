#nullable enable
using System;
using System.Collections.Generic;
using Daipan.LevelDesign.Enemy.Scripts;
using Daipan.Player.MonoScripts;
using UnityEngine;

namespace Daipan.LevelDesign.Battle.Scripts
{
    public class LanePositionMono : MonoBehaviour
    {
       [Header("レーンの座標に関する設定はここです")]
       [Space]
       public List<LanePosition> lanePositions = null!;

       [Header("プレイヤーの生成位置のx座標を決めるゲームオブジェクト")]
       public PlayerSpawnedPosition playerSpawnedPosition = null!;

       [Header("敵の消滅位置")]
       public Transform enemyDespawnedPoint = null!;

       [Header("攻撃エフェクトの消滅位置")]
       public Transform attackEffectDespawnedPoint = null!;
    }

    [Serializable]
    public class LanePosition
    {
        [Header("レーンのy座標を決めるゲームオブジェクト")]
        public Transform laneYTransform = null!;

        public EnemySpawnedPosition enemySpawnedPosition = null!;
        // このクラスがWaveごとにあって、ListでそのWaveにおけるレーンを表してもいいかも。

    }
    

}