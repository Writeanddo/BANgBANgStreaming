#nullable enable
using System;
using Daipan.Enemy.Interfaces;
using Daipan.Enemy.LevelDesign.Interfaces;
using Daipan.Enemy.LevelDesign.Scripts;
using Daipan.Enemy.Scripts;
using Daipan.LevelDesign.Enemy.Scripts;
using Daipan.Player.LevelDesign.Interfaces;
using Daipan.Player.MonoScripts;
using Daipan.Player.Scripts;
using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace Daipan.Enemy.MonoScripts
{
    public sealed class FinalBossMono : AbstractEnemyMono 
    {
        public FinalBossViewMono? FinalBossViewMono => finalBossViewMono;
        [SerializeField] FinalBossViewMono? finalBossViewMono;
        EnemyCluster _enemyCluster = null!;
        FinalBossActionDecider _finalBossActionDecider = null!;
        EnemyDie _enemyDie = null!;
        IEnemySpawnPoint _enemySpawnPoint = null!;
        FinalBossParamData _finalBossParamData = null!;
        IEnemyOnAttacked _enemyOnAttacked = null!;
        PlayerHolder _playerHolder = null!;
        public override EnemyEnum EnemyEnum { get; protected set; } = EnemyEnum.None;
        public override bool IsReachedPlayer { get; protected set; }
        Hp _hp = null!;

        public override Hp Hp
        {
            get => _hp;
            protected set
            {
                _hp = value;
                if (_hp.Value <= 0)
                {
                    Die(this, isDaipaned: false);
                }
            }
        }

        void Update()
        {
            // _enemyAttackDecider.AttackUpdate(this, finalBossViewMono,
            //     _enemyParamContainer.GetEnemyParamData(EnemyEnum), _playerHolder.PlayerMono);
            
            // 攻撃範囲よりプレイヤーとの距離が大きいときだけ動く
            if (transform.position.x - _playerHolder.PlayerMono.transform.position.x >=
                _finalBossParamData.GetAttackRange())
            {
                var moveSpeed = (float)_finalBossParamData.GetMoveSpeedPerSec();
                transform.position += Time.deltaTime * moveSpeed * Vector3.left;
                IsReachedPlayer = false;
            }
            else
            {
                IsReachedPlayer = true;
            }

            if (transform.position.x < _enemySpawnPoint.GetEnemyDespawnedPoint().x)
                Die(this, isDaipaned: false);

            finalBossViewMono?.SetHpGauge(Hp.Value, _finalBossParamData.GetMaxHp());
        }

        [Inject]
        public void Initialize(
            PlayerHolder playerHolder
            , IEnemySpawnPoint enemySpawnPointData
            , FinalBossParamData finalBossParamData
        )
        {
            _playerHolder = playerHolder;
            _enemySpawnPoint = enemySpawnPointData;
            _finalBossParamData = finalBossParamData;
        }

        public void SetDomain(
            EnemyEnum enemyEnum
            , EnemyCluster enemyCluster
            , FinalBossActionDecider finalBossActionDecider
            , EnemyDie enemyDie
            , IEnemyOnAttacked enemyOnAttacked
        )
        {
            EnemyEnum = enemyEnum;
            _enemyCluster = enemyCluster;
            _finalBossActionDecider = finalBossActionDecider;
            _finalBossActionDecider.SetDomain(this, finalBossViewMono, _finalBossParamData, _playerHolder.PlayerMono);
            _enemyDie = enemyDie;
            _enemyOnAttacked = enemyOnAttacked;
            finalBossViewMono?.SetDomain(_finalBossParamData);
            Hp = new Hp(_finalBossParamData.GetMaxHp());
        }

        public event EventHandler<DiedEventArgs>? OnDied
        {
            add => _enemyDie.OnDied += value;
            remove => _enemyDie.OnDied -= value;
        }

        public override void Die(AbstractEnemyMono enemyMono, bool isDaipaned = false)
        {
            // todo: EnemyClusterとFinalBossを繋ぐ
            _enemyCluster.Remove(this);
           // _enemyDie.Died(finalBossViewMono, isDaipaned); // finalBossViewMono
        }

        public override void OnAttacked(IPlayerParamData playerParamData)
        {
            Hp = _enemyOnAttacked.OnAttacked(Hp, playerParamData);
        }

        public override void Highlight(bool isHighlighted)
        {
            finalBossViewMono?.Highlight(isHighlighted);
        }
    }

}