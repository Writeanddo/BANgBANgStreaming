#nullable enable
using System.Collections.Generic;
using System.Linq;
using Daipan.Battle.scripts;
using Daipan.Comment.Scripts;
using Daipan.Enemy.MonoScripts;
using Daipan.Enemy.Scripts;
using Daipan.LevelDesign.Comment.Scripts;
using Daipan.Player.Interfaces;
using Daipan.Player.LevelDesign.Interfaces;
using Daipan.Player.LevelDesign.Scripts;
using Daipan.Player.MonoScripts;
using Daipan.Stream.Scripts;
using UnityEngine;

namespace Daipan.Player.Scripts
{
    public sealed class PlayerAttackEffectBuilder : IPlayerAttackEffectBuilder
    {
        readonly IPlayerParamDataContainer _playerParamDataContainer;
        readonly ComboCounter _comboCounter;
        readonly EnemyCluster _enemyCluster;
        readonly CommentSpawner _commentSpawner;
        readonly WaveState _waveState;
        readonly IPlayerAntiCommentParamData _playerAntiCommentParamData;
        readonly ThresholdResetCounter _playerMissedAttackCounter;

        public PlayerAttackEffectBuilder(
            IPlayerParamDataContainer playerParamDataContainer
            ,ComboCounter comboCounter
            ,EnemyCluster enemyCluster
            ,CommentSpawner commentSpawner
            ,WaveState waveState
            ,IPlayerAntiCommentParamData playerAntiCommentParamData
        )
        {
            _playerParamDataContainer = playerParamDataContainer;
            _comboCounter = comboCounter;
            _enemyCluster = enemyCluster;
            _commentSpawner = commentSpawner;
            _waveState = waveState;
            _playerAntiCommentParamData = playerAntiCommentParamData;
            _playerMissedAttackCounter =
                new ThresholdResetCounter(playerAntiCommentParamData.GetMissedAttackCountForAntiComment());
        }

        public PlayerAttackEffectMono Build
        (
            PlayerAttackEffectMono effect
            , PlayerMono playerMono
            , List<AbstractPlayerViewMono?> playerViewMonos
            , PlayerColor playerColor
            )
        {
            effect.SetUp(_playerParamDataContainer.GetPlayerParamData(playerColor),
                () => _enemyCluster.NearestEnemy(playerMono.transform.position));
            effect.OnHit += (sender, args) =>
            {
                Debug.Log($"OnHit");
                AttackEnemy(_playerParamDataContainer, playerViewMonos, playerColor, args,_comboCounter, _playerMissedAttackCounter, _commentSpawner );
                SpawnAntiComment(args, _commentSpawner, _playerAntiCommentParamData,_waveState);
            };
            return effect;
        }


        static void AttackEnemy(
            IPlayerParamDataContainer playerParamDataContainer
            , List<AbstractPlayerViewMono?> playerViewMonos
            , PlayerColor playerColor
            , OnHitEventArgs args
            , ComboCounter comboCounter
            , ThresholdResetCounter playerMissedAttackCounter
            , CommentSpawner commentSpawner
        )
        {
            if (args.IsTargetEnemy && args.EnemyMono != null)
            {
                Debug.Log($"EnemyType: {args.EnemyMono.EnemyEnum}を攻撃");
                // 敵を攻撃
                var playerParamData = playerParamDataContainer.GetPlayerParamData(playerColor);
                var beforeHp = args.EnemyMono.Hp.Value; 
                PlayerAttackModule.Attack(args.EnemyMono, playerParamData);
                var afterHp = args.EnemyMono.Hp.Value;
                
                //  HPに変化があれば、コンボ増加
                if (beforeHp != afterHp) comboCounter.IncreaseCombo();
                else comboCounter.ResetCombo();

            }
            else
            {
                Debug.Log(
                    $"攻撃対象が{PlayerAttackModule.GetTargetEnemyEnum(playerColor)}ではないです args.EnemyMono?.EnemyEnum: {args.EnemyMono?.EnemyEnum}");
                comboCounter.ResetCombo();
                playerMissedAttackCounter.CountUp();
                if (playerMissedAttackCounter.IsOverThreshold) commentSpawner.SpawnCommentByType(CommentEnum.Spiky); 

                return;
            }


            // Animation
            foreach (var playerViewMono in playerViewMonos)
            {
                if (playerViewMono == null) continue;
                if (playerViewMono.playerColor == playerColor)
                    playerViewMono.Attack();
            }
        }

        static void SpawnAntiComment(
            OnHitEventArgs args
            ,CommentSpawner commentSpawner
            ,IPlayerAntiCommentParamData playerAntiCommentParamData
            ,WaveState waveState
            )
        {
            if (args.IsTargetEnemy) return;
            
            var spawnPercent = playerAntiCommentParamData.GetAntiCommentPercentOnMissAttacks(waveState.CurrentWaveIndex);
            
            if (spawnPercent / 100f > Random.value)
            {
                commentSpawner.SpawnCommentByType(CommentEnum.Spiky);
            }
           
        }
    }
}