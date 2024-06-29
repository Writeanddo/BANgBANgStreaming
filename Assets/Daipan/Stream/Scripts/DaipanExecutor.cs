#nullable enable
using Daipan.Comment.Scripts;
using Daipan.Enemy.Scripts;
using Daipan.Streamer.Scripts;
using Daipan.LevelDesign.Enemy.Scripts;
using UnityEngine;
using Daipan.Stream.MonoScripts;

namespace Daipan.Stream.Scripts
{
    public sealed class DaipanExecutor
    {
        readonly CommentCluster _commentCluster;
        readonly AntiCommentCluster _antiCommentCluster;
        readonly DaipanParam _daipanParam;
        readonly EnemyCluster _enemyCluster;
        readonly IrritatedValue _irritatedValue;
        readonly StreamStatus _streamStatus;
        readonly ViewerNumber _viewerNumber;
        readonly StreamerViewMono _streamerViewMono;
        readonly ShakeDisplayMono _shakeDisplayMono;
        public int DaipanNum = 0;

        public DaipanExecutor(
            DaipanParam daipanParam,
            ViewerNumber viewerNumber,
            StreamStatus streamStatus,
            IrritatedValue irritatedValue,
            EnemyCluster enemyCluster,
            CommentCluster commentCluster,
            AntiCommentCluster antiCommentCluster,
            StreamerViewMono streamerViewMono,
            ShakeDisplayMono shakeDisplayMono
        )
        {
            _daipanParam = daipanParam;
            _viewerNumber = viewerNumber;
            _streamStatus = streamStatus;
            _irritatedValue = irritatedValue;
            _enemyCluster = enemyCluster;
            _commentCluster = commentCluster;
            _antiCommentCluster = antiCommentCluster;
            _streamerViewMono = streamerViewMono;
            _shakeDisplayMono = shakeDisplayMono;
        }
        public void DaiPan()
        {
            var canDaipan = _irritatedValue.IsFull;
            if (canDaipan)
            {
                Debug.Log($"Daipan!");
                _enemyCluster.Daipaned();
                _antiCommentCluster.BlownAway();
                _streamerViewMono.Daipan();
                _shakeDisplayMono.Daipan();
                DaipanNum++;
                
                // 台パンしたら怒りゲージは0になる
                _irritatedValue.DecreaseValue(_irritatedValue.Value);
            }
            else
            {
                // 何もしない
                // 台パンをスカした時のアニメーションを再生するかもしれない
            }

   
        }
    }
}