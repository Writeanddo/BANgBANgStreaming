using System.Collections.Generic;
using System.Linq;
using Daipan.Core.Interfaces;
using Daipan.Stream.Scripts;
using UnityEngine;

namespace Daipan.Comment.Scripts
{
    public sealed class AntiCommentRelocate : IUpdate
    {
        readonly AntiCommentCluster _antiCommentCluster;
        readonly StreamStatus _streamStatus;

        // todo : あとで調整する必要あり 必ずしもパラメータでもらう必要はない
        readonly float _verticalSpacing = 1.0f; // オブジェクト間の垂直間隔


        public AntiCommentRelocate(
            AntiCommentCluster antiCommentCluster,
            StreamStatus streamStatus
        )
        {
            _antiCommentCluster = antiCommentCluster;
            _streamStatus = streamStatus;
        }

        void IUpdate.Update()
        {
            var objectsToAlign = _antiCommentCluster.CommentMonos.Select(x => x.gameObject).ToList();
            _streamStatus.IsIrritated = objectsToAlign.Count > 0;
            AlignVertically(objectsToAlign);
        }

        void AlignVertically(IEnumerable<GameObject> objectsToAlign)
        {
            var objectsToAlignList = objectsToAlign.ToList();
            var totalHeight = (objectsToAlignList.Count() - 1) * _verticalSpacing;
            var currentY = totalHeight / 2.0f; // 最初のオブジェクトのY位置

            foreach (var obj in objectsToAlignList)
            {
                // オブジェクトを中央に配置
                obj.transform.position = new Vector3(0, currentY, 0);
                currentY -= _verticalSpacing; // 次のオブジェクトの位置
            }
        }
    } 
}

