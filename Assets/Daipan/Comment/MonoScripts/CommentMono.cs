#nullable enable
using System;
using Daipan.Comment.Scripts;
using UnityEngine;
using VContainer;

namespace Daipan.Comment.MonoScripts
{
    public sealed class CommentMono : MonoBehaviour
    {
        [SerializeField] float speed = 0.01f;
        CommentCluster _commentCluster = null!;
        CommentSpawnPointContainer _spawnPointContainer = null!;
        public CommentParameter Parameter { get; private set; } = null!;

        void Update()
        {
            transform.position += Vector3.up * speed;
            if (transform.position.y > _spawnPointContainer.DespawnPosition.y) _commentCluster.Remove(this);
        }

        public event EventHandler<DespawnEventArgs>? OnDespawn;


        [Inject]
        public void Initialize(
            CommentSpawnPointContainer spawnPointContainer,
            CommentCluster commentCluster
        )
        {
            _spawnPointContainer = spawnPointContainer;
            _commentCluster = commentCluster;
        }

        public void SetParameter(CommentParameter parameter)
        {
            Parameter = parameter;
        }

        public void Despawn()
        {
            var args = new DespawnEventArgs(Parameter.CommentType == CommentType.Super);
            OnDespawn?.Invoke(this, args);
            Destroy(gameObject);
        }

        public void BlownAway()
        {
            _commentCluster.Remove(this);
        }
    }

    public record DespawnEventArgs(bool IsSuperComment);
}