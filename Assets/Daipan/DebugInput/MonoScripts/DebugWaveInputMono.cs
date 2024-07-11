#nullable enable
using System;
using Daipan.Enemy.LevelDesign.Scripts;
using Daipan.Enemy.Scripts;
using Daipan.Stream.Scripts;
using UnityEngine;
using VContainer;

namespace Daipan.DebugInput.MonoScripts
{
    public sealed class DebugWaveInputMono : MonoBehaviour
    {
        StreamTimer _streamTimer = null!;
        StreamData _streamData = null!;
        EnemyParamsManager _enemyParamsManager = null!;

        [Inject]
        public void Initialize(
            StreamTimer streamTimer
            , StreamData streamData
            , EnemyParamsManager enemyParamsManager
        )
        {
            _streamTimer = streamTimer;
            _streamData = streamData;
            _enemyParamsManager = enemyParamsManager;
        }

        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetTime(_streamTimer, _enemyParamsManager, 0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetTime(_streamTimer, _enemyParamsManager, 1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetTime(_streamTimer, _enemyParamsManager, 2);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SetNearLastTime(_streamTimer, _streamData);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SetTime(_streamTimer, _enemyParamsManager, 4);
            }
#endif
        }

        static void SetTime(StreamTimer streamTimer, EnemyParamsManager enemyParamsManager, int index)
        {
            // if (index < 0 || index >= enemyParamsManager.enemyTimeLineParams.Count)
            // {
            //     Debug.LogWarning($" index is out of range. index: {index}");
            //     return;
            // }
            //
            // Debug.Log($"SetTime index: {index}");
            // streamTimer.SetTime(enemyParamsManager.enemyTimeLineParams[index].startTime);
        }

        static void SetNearLastTime(StreamTimer streamTimer, StreamData streamData)
        {
            var nearLastTime = streamData.GetMaxTime() - 3;
            Debug.Log($"SetNearLastTime nearLastTime: {nearLastTime}");
            streamTimer.SetTime(nearLastTime);
        }
    }
}