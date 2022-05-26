using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;

namespace Wedo {
    public class Vector3Setting : MonoBehaviour {
        public Vector3Item X;
        public Vector3Item Y;
        public Vector3Item Z;

        public Button ResetButton;
        public Button ClearButton;
        public Button SaveButton;

        public string SaveName { get; private set; }
        public Action<Vector3> OnUpdate { get; private set;}

        public Vector3 StartValue { get; private set; }
        public Vector3 DefaultValue { get; private set; }

        public Vector3 Value => new Vector3(X.Value, Y.Value, Z.Value);
        

        public void Init(Vector3 min, Vector3 max, Vector3 value, Vector3 defaultValue, float step, Action<Vector3> onUpdate) {
            X.Init(min.x, max.x, value.x, step, () => OnUpdate?.Invoke(Value));
            Y.Init(min.y, max.y, value.y, step, () => OnUpdate?.Invoke(Value));
            Z.Init(min.z, max.z, value.z, step, () => OnUpdate?.Invoke(Value));

            StartValue = value;
            DefaultValue = defaultValue;

            OnUpdate = onUpdate;
        }

        public void UpdateSaveName(string saveName)
        {
            SaveName = saveName;
        }

        public void SetValue(Vector3 value)
        {
            X.Slider.value = value.x;
            Y.Slider.value = value.y;
            Z.Slider.value = value.z;
        }

        public void Start()
        {
            ResetButton.OnClickAsObservable().Subscribe(_ =>
            {
                SetValue(DefaultValue);
                OnUpdate?.Invoke(Value);
            }).AddTo(this);
            
            ClearButton.OnClickAsObservable().Subscribe(_ =>
            {
                SetValue(PlayerPrefsExtension.GetVector3(SaveName, new Vector3()));
                OnUpdate?.Invoke(Value);
            }).AddTo(this);
            
            SaveButton.OnClickAsObservable().Subscribe(_ =>
            {
                PlayerPrefsExtension.SetVector3(SaveName, Value);
            }).AddTo(this);
        }
    }
}
