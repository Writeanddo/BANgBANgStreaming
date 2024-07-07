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
using Daipan.Tutorial.Scripts;
using UnityEngine;

namespace Daipan.Player.Scripts
{
    public sealed class PlayerAttackEffectBuilderTutorial : IPlayerAttackEffectBuilder
    {
        readonly IPlayerParamDataContainer _playerParamDataContainer;
        readonly EnemyCluster _enemyCluster;
        readonly EnemyTotemOnAttack _enemyTotemOnAttack;
        readonly TutorialFacilitator _tutorialFacilitator; 

        public PlayerAttackEffectBuilderTutorial(
            IPlayerParamDataContainer playerParamDataContainer
            ,EnemyCluster enemyCluster
            ,EnemyTotemOnAttack enemyTotemOnAttack
            ,TutorialFacilitator tutorialFacilitator 
        )
        {
            _playerParamDataContainer = playerParamDataContainer;
            _enemyCluster = enemyCluster;
            _enemyTotemOnAttack = enemyTotemOnAttack;
            _tutorialFacilitator = tutorialFacilitator;
        }

        public PlayerAttackEffectMono Build(PlayerAttackEffectMono effect, PlayerMono playerMono,
            List<AbstractPlayerViewMono?> playerViewMonos, PlayerColor playerColor)
        {
            effect.SetUp(_playerParamDataContainer.GetPlayerParamData(playerColor),
                () => _enemyCluster.NearestEnemy(playerMono.transform.position));
            effect.OnHit += (sender, args) =>
            {
                Debug.Log($"OnHit");
                AttackEnemy(
                    _playerParamDataContainer
                    , playerViewMonos
                    , playerColor
                    , args.EnemyMono
                    ,_enemyTotemOnAttack
                    ,_tutorialFacilitator);
            };
            return effect;
        }



        static void AttackEnemy(IPlayerParamDataContainer playerParamDataContainer
            ,List<AbstractPlayerViewMono?> playerViewMonos
            ,PlayerColor playerColor
            ,EnemyMono? enemyMono
            ,EnemyTotemOnAttack totemOnAttack
            ,TutorialFacilitator tutorialFacilitator 
            )
        {
            Debug.Log($"Attack enemyMono?.EnemyEnum: {enemyMono?.EnemyEnum}");
            if (enemyMono == null) return;

            Debug.Log($"EnemyType: {enemyMono.EnemyEnum}を攻撃");
            if (enemyMono.EnemyEnum == EnemyEnum.Red)
            {
                // チュートリアルごとの処理 
                if (tutorialFacilitator.CurrentStep is RedEnemyTutorial redEnemyTutorial)
                {
                    redEnemyTutorial.SetIsSuccess(playerColor == PlayerColor.Red);
                }
                if(tutorialFacilitator.CurrentStep is SequentialEnemyTutorial sequentialEnemyTutorial)
                {
                    // 本来は全ての敵を倒したかどうかを判定するべきだが、最後の敵がたまたmRedなので、これで判定する
                    if(playerColor == PlayerColor.Red) sequentialEnemyTutorial.MoveNextSpeech();
                }
            }
            if (PlayerAttackModule.GetTargetEnemyEnum(playerColor).Contains(enemyMono.EnemyEnum))
            {
                // 敵を攻撃
                var playerParamData = playerParamDataContainer.GetPlayerParamData(playerColor);
                enemyMono.Hp = enemyMono.EnemyEnum switch 
                {
                    EnemyEnum.Totem => totemOnAttack.OnAttacked(enemyMono.Hp, playerParamData),
                    _ => PlayerAttackModule.Attack(enemyMono.Hp,playerParamData)
                };

            }

            // Animation
            foreach (var playerViewMono in playerViewMonos)
            {
                if (playerViewMono == null) continue;
                if (playerViewMono.playerColor == playerColor)
                    playerViewMono.Attack();
            }
        }
        
     
    }
}