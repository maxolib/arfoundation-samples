using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;

namespace Wedo
{
    public class CalibrateManager : MonoBehaviour
    {
        public Vector3Setting PositionSetting;
        public Vector3Setting RotationSetting;
        public Vector3Setting ScaleSetting;

        public TextMeshProUGUI PositionText;

        public ImageTracking Tracking;

        [HideInInspector]
        public string TargetName;

        public bool IsDetected;


        private IDisposable UpdateTrackedTransformDispose;

        private void Start () {
            PositionSetting.Init( 
                new Vector3(-5, -5, -5), 
                new Vector3(5, 5, 5), 
                PlayerPrefsExtension.GetVector3($"{TargetName}_POSITION", new Vector3()),
                new Vector3(),
                0.005f,
                (position) => {
                    Debug.Log("Update Position");
                    Tracking.UpdateTargetPosition(position);
                });


            RotationSetting.Init( 
                new Vector3(-180, -180, -180), 
                new Vector3(180, 180, 180), 
                PlayerPrefsExtension.GetVector3($"{TargetName}_ROTATION", new Vector3()),
                new Vector3(),
                0.01f,
                (rotation) => {
                    Debug.Log("Update Rotation");
                    Tracking.UpdateTargetRotation(rotation);
                });

            ScaleSetting.Init( 
                new Vector3(0, 0, 0), 
                new Vector3(10, 10, 10), 
                PlayerPrefsExtension.GetVector3($"{TargetName}_SCALE", new Vector3(1, 1, 1)),
                new Vector3(1, 1, 1),
                0.01f,
                (scale) => {
                    Debug.Log("Update Scale");
                    Tracking.UpdateTargetScale(scale);
                });
        }

        private void Update() {
            UpdateStatus();
        }

        public void SetTarget(string targetName) {
            TargetName = targetName;
            IsDetected = false;
            Tracking.UpdateTargetTransform(TargetName);
            SetTargetTransform(targetName);
            
            UpdateTrackedTransformDispose?.Dispose();
            UpdateTrackedTransformDispose = Tracking
                .OnTrackedImagedAsObservable(TargetName)
                .Subscribe(imageTracked =>
                {
                    Debug.Log($"update2: {imageTracked.transform.position}");
                    Tracking.UpdateTrackedTransform(imageTracked);
                    IsDetected = true;
                }).AddTo(this);
        }

        public void SetTargetTransform(string targetName)
        {
            PositionSetting.UpdateSaveName($"{targetName}_POSITION");
            RotationSetting.UpdateSaveName($"{targetName}_ROTATION");
            ScaleSetting.UpdateSaveName($"{targetName}_SCALE");
            PositionSetting.SetValue(PlayerPrefsExtension.GetVector3($"{targetName}_POSITION", new Vector3()));
            RotationSetting.SetValue(PlayerPrefsExtension.GetVector3($"{targetName}_ROTATION", new Vector3()));
            ScaleSetting.SetValue(PlayerPrefsExtension.GetVector3($"{targetName}_SCALE", new Vector3(1, 1, 1)));
        }

        private void UpdateStatus() {
            PositionText.text = 
                $"Target Name: {(string.IsNullOrEmpty(TargetName) ? "-" : TargetName)}" +
                $"\nIs Detected: {IsDetected}" +
                $"\nPosition: {Tracking.TrackedTransform.position}" +
                $"\nRotation: {Tracking.TrackedTransform.rotation}" +
                $"\nScale: {Tracking.TrackedTransform.localScale}" +
                $"\nPosition: {PositionSetting.Value}" +
                $"\nRotation: {RotationSetting.Value}" +
                $"\nScale: {ScaleSetting.Value}";
        }

    }
}
