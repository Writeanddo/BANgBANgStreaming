#nullable enable
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Daipan.Tutorial.MonoScripts
{
    public class BlackScreenViewMono : MonoBehaviour
    {
        [SerializeField] CanvasGroup canvasGroup = null!;
        
        public void FadeIn(float time, Action onComplete)
        {
            canvasGroup.DOFade(1, time).OnComplete(() =>
            {
                onComplete();
            });
        }
       
        public void FadeOut(float time, Action onComplete)
        {
            canvasGroup.DOFade(0, time).OnComplete(() =>
            {
                onComplete();
            });
        }

   
    } 
}

