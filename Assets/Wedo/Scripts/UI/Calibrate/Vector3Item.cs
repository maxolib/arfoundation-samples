using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Wedo
{
    public class Vector3Item : MonoBehaviour
    {
        public Button Decrease;
        public Button Increase;
        public Slider Slider;
        public float Value => Slider.value;

        public Action OnUpdate { get; private set; }
        public float Step { get; private set; }

        public void Init(float min, float max, float value, float step, Action onUpdate)
        {
            Slider.minValue = min;
            Slider.maxValue = max;
            Slider.value = value;
            OnUpdate = onUpdate;
            Step = step;
        }

        public void Start() {
            Decrease.onClick.AddListener(() =>
            {
                Slider.value -= Step;
            });
            Increase.onClick.AddListener(() =>
            {
                Slider.value += Step;
            });

            Slider.onValueChanged.AddListener((value) =>
            {
                OnUpdate?.Invoke();
            });
        }

        private void OnDestroy() {
            Slider.onValueChanged.RemoveAllListeners();
        }
    }
}
