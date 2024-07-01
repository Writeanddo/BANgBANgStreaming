#nullable enable
using Daipan.Battle.interfaces;
using Daipan.Enemy.Interfaces;
using Daipan.Enemy.MonoScripts;
using Daipan.LevelDesign.Enemy.Scripts;
using Daipan.Player.MonoScripts;
using Daipan.Player.Scripts;
using UnityEngine;

namespace Daipan.Enemy.Scripts
{
    public sealed class EnemyAttackDecider
    {
        float Timer { get; set; }

        public PlayerHpNew AttackUpdate(EnemyMono enemyMono,IEnemyParamData enemyParamData, PlayerMono playerMono, AbstractEnemyViewMono? enemyViewMono)
        {
            Timer += Time.deltaTime;
            if (Timer >= enemyParamData.GetAttackDelayDec())
            {
                Timer = 0;
                return Attack(enemyMono, enemyParamData,  playerMono, enemyViewMono);
            }
            return playerMono.PlayerHpNew;
        }

        static PlayerHpNew  Attack(EnemyMono enemyMono,IEnemyParamData enemyParamData, PlayerMono playerMono, AbstractEnemyViewMono? enemyViewMono)
        {
            if (!CanAttack(enemyMono,enemyParamData, playerMono)) return playerMono.PlayerHpNew;
            if (enemyViewMono != null) enemyViewMono.Attack();
            return EnemyAttackModule.Attack(enemyParamData,playerMono.PlayerHpNew);
        }
        
        static bool CanAttack(EnemyMono enemyMono,IEnemyParamData enemyParamData,  PlayerMono playerMono)
        {
            if (playerMono.CurrentHp <= 0) return false;
            if (enemyMono.transform.position.x - playerMono.transform.position.x > enemyParamData.GetAttackRange()) return false;
            return true;
        }
        
    }
}