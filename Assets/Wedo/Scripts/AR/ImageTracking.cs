using System;
using UniRx;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Wedo
{
    [RequireComponent(typeof(ARTrackedImageManager))]
    public class ImageTracking : MonoBehaviour
    {
        public Transform TrackedTransform;
        public GameObject TrackedObject;
        
        private ARTrackedImageManager TrackedManager;

        private Action<ARTrackedImage> onTrackedImage;

        public bool AutoUpdate = true;

        private Vector3 DEFAULT_LEFT_POSITION = new Vector3(-0.40f, -0.61f, -0.78f);
        private Vector3 DEFAULT_LEFT_ROTATION = new Vector3(85.54f, 0, 0);
        private Vector3 DEFAULT_LEFT_SCALE = new Vector3(0.68f, 1, 1);
        
        private Vector3 DEFAULT_RIGHT_POSITION = new Vector3(-0.36f, -0.57f, -0.75f);
        private Vector3 DEFAULT_RIGHT_ROTATION = new Vector3(-96.23f, 17.58f, 160.06f);
        private Vector3 DEFAULT_RIGHT_SCALE = new Vector3(0.68f, 1, 1);
        

        public string TrackedName { get; private set; }

        private void Awake()
        {
            TrackedManager = GetComponent<ARTrackedImageManager>();
            
            PlayerPrefsExtension.SetVector3("LEFT_POSITION", DEFAULT_LEFT_POSITION);
            PlayerPrefsExtension.SetVector3("LEFT_ROTATION", DEFAULT_LEFT_ROTATION);
            PlayerPrefsExtension.SetVector3("LEFT_SCALE", DEFAULT_LEFT_SCALE);
            
            PlayerPrefsExtension.SetVector3("RIGHT_POSITION", DEFAULT_RIGHT_POSITION);
            PlayerPrefsExtension.SetVector3("RIGHT_ROTATION", DEFAULT_RIGHT_ROTATION);
            PlayerPrefsExtension.SetVector3("RIGHT_SCALE", DEFAULT_RIGHT_SCALE);
        }

        private void Start()
        {
            if (AutoUpdate)
            {
                OnTrackedImagedAsObservable().Subscribe(UpdateTransform).AddTo(this);
            }
        }

        private void OnEnable()
        {
            TrackedManager.trackedImagesChanged += ImageChanged;
        }

        private void OnDisable()
        {
            TrackedManager.trackedImagesChanged -= ImageChanged;
        }

        private void ImageChanged(ARTrackedImagesChangedEventArgs args)
        {
            foreach (var trackedImage in args.added)
            {
                Debug.Log($"added: {trackedImage.transform.position}");
                TrackedObject.SetActive(true);
                onTrackedImage?.Invoke(trackedImage);
            }
            foreach (var trackedImage in args.updated)
            {
                Debug.Log($"update: {trackedImage.referenceImage.name} {trackedImage.transform.position}");
                TrackedObject.SetActive(true);
                onTrackedImage?.Invoke(trackedImage);
            }
        }

        private void UpdateTransform(ARTrackedImage trackedImage)
        {
            UpdateTrackedTransform(trackedImage);
            UpdateTargetTransform(TrackedName);
        }

        public void UpdateTrackedTransform(ARTrackedImage trackedImage)
        {
            TrackedName = trackedImage.referenceImage.name;
            var position = trackedImage.transform.position;
            var eulerAngles = trackedImage.transform.eulerAngles;
            var scale = trackedImage.transform.localScale;
            
            TrackedTransform.position = position;
            TrackedTransform.eulerAngles = eulerAngles;
            TrackedTransform.localScale = scale;
        }

        public void UpdateTargetTransform(string targetName)
        {
            var position = PlayerPrefsExtension.GetVector3($"{targetName}_POSITION", new Vector3());
            var rotation = PlayerPrefsExtension.GetVector3($"{targetName}_ROTATION", new Vector3());
            var scale = PlayerPrefsExtension.GetVector3($"{targetName}_SCALE", new Vector3(1, 1, 1));
            
            UpdateTargetPosition(position);
            UpdateTargetRotation(rotation);
            UpdateTargetScale(scale);
        }

        public IObservable<ARTrackedImage> OnTrackedImagedAsObservable() => Observable.FromEvent<ARTrackedImage>(
                action => onTrackedImage += action,
                action => onTrackedImage -= action
            );

        public IObservable<ARTrackedImage> OnTrackedImagedAsObservable(string trackedName) => 
            OnTrackedImagedAsObservable().Where(trackedImage => trackedImage.referenceImage.name == trackedName);
        
        
        public void UpdateTargetPosition(Vector3 position)
        {
            TrackedObject.transform.localPosition = position;
        }
        
        public void UpdateTargetRotation(Vector3 rotation)
        {
            TrackedObject.transform.localEulerAngles = rotation;
        }
        
        public void UpdateTargetScale(Vector3 scale)
        {
            TrackedObject.transform.localScale = new Vector3(scale.x, scale.x, scale.x);
        }
    }
}

