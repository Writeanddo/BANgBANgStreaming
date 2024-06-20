#nullable enable
using System;
using Daipan.Battle.interfaces;
using Daipan.Enemy.Interfaces;
using Daipan.Enemy.LevelDesign.Scripts;
using Daipan.Enemy.Scripts;
using Daipan.LevelDesign.Enemy.Scripts;
using Daipan.Player.Scripts;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace Daipan.Enemy.MonoScripts
{
    public sealed class EnemyMono : MonoBehaviour, IHpSetter
    {
        [SerializeField] AbstractEnemyViewMono? enemyViewMono;
        EnemyAttackDecider _enemyAttackDecider = null!;
        EnemyCluster _enemyCluster = null!;
        EnemyHp _enemyHp = null!;
        EnemySpawnPointData _enemySpawnPointData = null!;
        EnemyParamWarpContainer _enemyParamWarpContainer = null!;
        PlayerHolder _playerHolder = null!;
        public EnemyEnum EnemyEnum { get; private set; } = EnemyEnum.None;

        void Update()
        {
            _enemyAttackDecider.AttackUpdate(_playerHolder.PlayerMono, enemyViewMono);

            // 攻撃範囲よりプレイヤーとの距離が大きいときだけ動く
            if (transform.position.x - _playerHolder.PlayerMono.transform.position.x >=
                _enemyParamWarpContainer.GetEnemyParamData(EnemyEnum).GetAttackRange())
            {
                float moveSpeed = (float)_enemyParamWarpContainer.GetEnemyParamData(EnemyEnum).GetMoveSpeedPreSec();
                transform.position += Time.deltaTime * moveSpeed * Vector3.left;
            }

            if (transform.position.x < _enemySpawnPointData.GetEnemyDespawnedPoint().x)
                _enemyCluster.Remove(this, false); // Destroy when out of screen

            enemyViewMono?.SetHpGauge(CurrentHp, _enemyParamWarpContainer.GetEnemyParamData(EnemyEnum).GetCurrentHp());
        }

        public int CurrentHp
        {
            set => _enemyHp.CurrentHp = value;
            get => _enemyHp.CurrentHp;
        }

        public event EventHandler<DiedEventArgs>? OnDied;


        [Inject]
        public void Initialize(
            EnemyCluster enemyCluster,
            PlayerHolder playerHolder,
            EnemySpawnPointData enemySpawnPointData,
            EnemyParamWarpContainer enemyParamWarpContainer
        )
        {
            _enemyCluster = enemyCluster;
            _playerHolder = playerHolder;
            _enemySpawnPointData = enemySpawnPointData;
            _enemyParamWarpContainer = enemyParamWarpContainer;
        }

        public void SetDomain(
            EnemyEnum enemyEnum,
            EnemyHp enemyHp,
            EnemyAttackDecider enemyAttackDecider
        )
        {
            EnemyEnum = enemyEnum;
            _enemyHp = enemyHp;
            _enemyAttackDecider = enemyAttackDecider;

            enemyViewMono?.SetDomain(_enemyParamWarpContainer);
            enemyViewMono?.SetView(enemyEnum);
        }

        public void Died(bool isDaipaned, bool isTriggerCallback)
        {
            // Callback
            if (isTriggerCallback)
            {
                var args = new DiedEventArgs(EnemyEnum);
                OnDied?.Invoke(this, args);
            }

            OnDiedProcess(this, isDaipaned, enemyViewMono);
        }

        static void OnDiedProcess(EnemyMono enemyMono, bool isDaipaned, AbstractEnemyViewMono? enemyViewMono)
        {
            if (enemyViewMono == null)
            {
                Destroy(enemyMono.gameObject);
                return;
            }

            if (isDaipaned)
                enemyMono.transform
                    .DOMoveY(-1.7f, 0.3f)
                    .SetEase(Ease.InQuint)
                    .OnStart(() => { enemyViewMono.Daipaned(() => Destroy(enemyMono.gameObject)); });
            else
                enemyViewMono.Died(() => Destroy(enemyMono.gameObject));
        }
    }

    public record DiedEventArgs(EnemyEnum enemyEnum, bool IsTrigger = false);
}