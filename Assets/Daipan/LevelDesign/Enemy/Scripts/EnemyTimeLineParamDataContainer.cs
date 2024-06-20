#nullable enable
using System.Collections.Generic;
using System.Linq;
using Daipan.Enemy.Scripts;
using Daipan.Stream.Scripts;

namespace Daipan.LevelDesign.Enemy.Scripts
{
    public class EnemyTimeLineParamDataContainer
    {
        readonly IList<EnemyTimeLineParamWarp> _enemyTimeLineParamDatas;
        
        public EnemyTimeLineParamDataContainer(
            IList<EnemyTimeLineParamWarp> enemyTimeLineParamDatas
            )
        {
            _enemyTimeLineParamDatas = enemyTimeLineParamDatas;
          
        }

        public EnemyTimeLineParamWarp GetEnemyTimeLineParamData(StreamTimer streamTimer)
        {
            return _enemyTimeLineParamDatas
                .Where(e => e.GetStartTime() <= streamTimer.CurrentTime)
                .OrderByDescending(e => e.GetStartTime()).First();
        }
    }
}