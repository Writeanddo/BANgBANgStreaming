#nullable enable
using System;
using Daipan.Battle.scripts;
using Daipan.Enemy.LevelDesign.Scripts;
using Daipan.Enemy.Scripts;
using Daipan.Stream.Scripts;
using UnityEngine;
using VContainer;

namespace Daipan.DebugInput.MonoScripts
{
    public sealed class DebugWaveInputMono : MonoBehaviour
    {
        WaveState _waveState = null!;
        EnemyWaveSpawnerCounter _enemyWaveSpawnerCounter = null!;

        [Inject]
        public void Initialize(
            WaveState waveState
            , EnemyWaveSpawnerCounter enemyWaveSpawnerCounter
        )
        {
            _waveState = waveState;
            _enemyWaveSpawnerCounter = enemyWaveSpawnerCounter;
        }

        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ForceNextWave(_waveState, 0); 
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ForceNextWave (_waveState, 1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ForceNextWave(_waveState, 2); 
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ForceNextWave(_waveState, 3);  
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
               //  SetTime(_streamTimer, _enemyParamsManager, 4);
            }
#endif
        }

        void ForceNextWave(WaveState waveState, int index)
        {
            while (waveState.CurrentWave < index)
            {
                waveState.NextWave();
                if(waveState.CurrentWave > 100) break;
            }
            _enemyWaveSpawnerCounter.ResetCounter();
            Debug.Log($"ForceNextWave waveState.CurrentWave: {waveState.CurrentWave}");
        }

        static void SetNearLastTime(StreamTimer streamTimer, StreamData streamData)
        {
            var nearLastTime = streamData.GetMaxTime() - 3;
            Debug.Log($"SetNearLastTime nearLastTime: {nearLastTime}");
            streamTimer.SetTime(nearLastTime);
        }
    }
}