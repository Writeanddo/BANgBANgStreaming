﻿#nullable enable
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Daipan.Stream.Scripts
{
    [Serializable]
    public sealed class ViewerParameter
    {
        [Header("一秒間に増える視聴者の数")]
        public int increaseNumberPerSecond;
        [Header("理不尽な状態の時の一秒間に減る視聴者の数")]
        public int decreaseNumberWhenIrradiated;
    }

    [Serializable]
    public sealed class DaipanParameter
    {
        [Header("台パンによって増える視聴者の数")]
        public int increaseNumberByDaipan;
        [Header("盛り上がっているときに増える視聴者の数")]
        public int increaseNumberWhenExciting;
    }

    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StreamParameter", order = 1)]
    public sealed class StreamParameter : ScriptableObject
    {
        [FormerlySerializedAs("viewerNumberParameter")] [SerializeField] public ViewerParameter viewerParameter = null!;
        [SerializeField] public DaipanParameter daipanParameter = null!;
    }
}