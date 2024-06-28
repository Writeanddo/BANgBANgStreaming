#nullable enable
using UnityEngine;
using VContainer;
using Daipan.Stream.Scripts;
using R3;
using UnityEngine.UI;

namespace Daipan.Stream.MonoScripts
{
    public class IrritatedViewMono : MonoBehaviour
    {
        [SerializeField] Image IrritatedGuage = null!;

        [Inject]
        public void Initialize(IrritatedValue irritatedValue)
        {
            Observable.EveryValueChanged(irritatedValue, x => irritatedValue.Ratio)
                .Subscribe(_ => UpdateIrritatedGauge(irritatedValue.Ratio))
                .AddTo(this);
        }

        void UpdateIrritatedGauge(float ratio)
        {
            IrritatedGuage.fillAmount = ratio;
        }
    }
}