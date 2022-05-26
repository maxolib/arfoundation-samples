using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Wedo
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleActiveEvent : MonoBehaviour
    {
        private Toggle Toggle;
        public GameObject Target;

        private void Awake()
        {
            Toggle = GetComponent<Toggle>();
        }

        private void Start()
        {
            Toggle.OnValueChangedAsObservable().Subscribe(isOn =>
            {
                Target.SetActive(isOn);
            }).AddTo(this);
        }
    }
}
