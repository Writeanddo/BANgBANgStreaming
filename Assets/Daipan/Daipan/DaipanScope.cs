#nullable enable
using Daipan.Core.Interfaces;
using Daipan.Daipan;
using Daipan.Enemy.Interfaces;
using Daipan.Enemy.MonoScripts;
using Daipan.Enemy.Scripts;
using Daipan.Stream.MonoScripts;
using Daipan.Stream.Scripts;
using Daipan.Stream.Scripts.Utility;
using Daipan.Stream.Scripts.Viewer.Tests;
using Enemy;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public sealed class DaipanScope : LifetimeScope
{
    [SerializeField] StreamParameter streamParameter = null!;
    [SerializeField] PlayerParameter playerParameter = null!;
    [SerializeField] EnemyAttributeParameters enemyAttributeParameters = null!;

    protected override void Configure(IContainerBuilder builder)
    {
        // Domain
        // Stream
        builder.RegisterInstance(streamParameter.viewer);
        builder.RegisterInstance(streamParameter.daipan);
        builder.Register<StreamPrefabLoader>(Lifetime.Scoped).As<IPrefabLoader<StreamMono>>();
        builder.Register<IrritatedValue>(Lifetime.Scoped).WithParameter(100);
        builder.Register<ViewerNumber>(Lifetime.Scoped);
        builder.Register<StreamStatus>(Lifetime.Scoped);
        builder.Register<IStart, StreamSpawner>(Lifetime.Scoped).AsSelf();

        // Comment
        builder.Register<IStart, CommentSpawnPointContainer>(Lifetime.Scoped).AsSelf();
        builder.RegisterComponentInHierarchy<CommentSpawner>(); // とりあえずMonoで実装


        builder.Register<DaipanExecutor>(Lifetime.Scoped);

        // Player
        builder.RegisterInstance(playerParameter.attackParameter);
        builder.Register<PlayerPrefabLoader>(Lifetime.Scoped).As<IPrefabLoader<PlayerMono>>();
        builder.Register<PlayerAttack>(Lifetime.Scoped);
        builder.Register<IStart, PlayerSpawner>(Lifetime.Scoped);

        // Enemy
        builder.RegisterInstance(enemyAttributeParameters);
        builder.Register<EnemyPrefabLoader>(Lifetime.Scoped).As<IPrefabLoader<EnemyMono>>();

        builder.Register<EnemyAttack>(Lifetime.Scoped);
        builder.Register<EnemyOnHit>(Lifetime.Scoped).As<IEnemyOnHit>();
        builder.Register<EnemySpecificBuilder>(Lifetime.Scoped).AsImplementedInterfaces().WithParameter(EnemyEnum.A);
        builder.Register<IStart, EnemySpawner>(Lifetime.Scoped).AsSelf();
        builder.Register<EnemyCluster>(Lifetime.Scoped);


        // View
        builder.RegisterComponentInHierarchy<StreamViewMono>();

        // Test
        builder.RegisterComponentInHierarchy<PlayerTestInput>();

        // Initializer
        builder.RegisterEntryPoint<DaipanInitializer>();

        builder.UseEntryPoints(Lifetime.Scoped, entryPoints => { entryPoints.Add<EnemySpawner>(); });
    }
}