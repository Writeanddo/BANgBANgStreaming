#nullable enable
using System.Collections.Generic;
using Daipan.Stream.Scripts;
using TMPro;
using UnityEngine;
using VContainer;

namespace Daipan.LevelDesign.Enemy.Scripts
{
    public class EnemyTimeLineParamDataBuilder
    {
        public EnemyTimeLineParamDataBuilder(
            IContainerBuilder builder,
            EnemyParamManager enemyParamManager
        )
        {
            // [Precondition]
            if(enemyParamManager.enemyTimeLineParams.Count == 0)
            {
               Debug.LogWarning("EnemyTimeLineParams.Count is 0");
               enemyParamManager.enemyTimeLineParams.Add(new EnemyTimeLineParam());
            }
            
            
            var enemyTimeLineParams = new List<EnemyTimeLineParamData>();
            foreach(var enemyTimeLineParam in enemyParamManager.enemyTimeLineParams)
            {
                enemyTimeLineParams.Add(new EnemyTimeLineParamData()
                {
                    GetStartTime = () => enemyTimeLineParam.startTime,
                    GetSpawnDelaySec = () => enemyTimeLineParam.spawnDelaySec,
                    GetMoveSpeedRate = () => enemyTimeLineParam.moveSpeedRate,
                    GetSpawnBossPercent = () => enemyTimeLineParam.spawnBossPercent
                });
            }
            var enemyTimeLineParamContainer = new EnemyTimeLineParamDataContainer(enemyTimeLineParams);
            builder.RegisterInstance(enemyTimeLineParamContainer);
        }
    
    } 
}
