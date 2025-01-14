#nullable enable
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Daipan.LevelDesign.EndScene
{
    [CreateAssetMenu(fileName = "EndSceneTransitionParam",
        menuName = "ScriptableObjects/EndScene/EndSceneTransitionParam",
        order = 1)]
    public sealed class EndSceneTransitionParam : ScriptableObject
    {
        [Header("箱の中END：視聴者がx人以下の時")] [Min(0)] public int viewerCountThresholdForInsideTheBoxEnd = 500;

        [Header("配信者ちゃん感謝祭END：視聴者がx人以上の時")] [Min(0)]
        public int viewerCountThresholdForThanksgivingEnd = 1000;

        [Header("ゲーム下手配信者END：HPがxパーセント以下の時")] [Min(0)]
        public int hpPercentThresholdForNoobGamerEnd = 0;

        [Header("プロゲーマーEND：最高コンボ数がx以上の時")] [Min(0)]
        public int maxComboCountThresholdForProGamerEnd = 100;

        [Header("清らか悟りの境地END：台パンx回以下の時")] [Min(0)]
        public int daipanCountThresholdForSacredLadyEnd = 10;

        [Header("炎上END：ゲーム終了時に台パンx回以上の時")] [Min(0)]
        public int daipanCountThresholdForBacklashEnd = 10;
        
        [Header("平凡END:視聴者の最小値") ] [Min(0)]
        public int viewerCountThresholdForHeibonEndMin = 0;
        [Header("平凡END:視聴者の最大値") ] [Min(0)]
        public int viewerCountThresholdForHeibonEndMax = 1000;
    }
}