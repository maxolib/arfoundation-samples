using System;
using UnityEngine;

namespace Wedo
{
    [RequireComponent(typeof(Animator))]
    public abstract class UIPage : MonoBehaviour
    {
        public UIPageManager Manager;
        private Animator Animator;

        public Action OnOpenStartAction;
        public Action OnCloseStartAction;

        public Action OnOpenCompletedAction;
        public Action OnCloseCompletedAction;
        
        private static readonly int IsOpen = Animator.StringToHash("isOpen");

        public void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        public void Open(Action onCompleted = null)
        {
            gameObject.SetActive(true);
            OnOpenCompletedAction = onCompleted;
            OnOpenStartAction?.Invoke();
            Animator.SetBool(IsOpen, true);
            OnOpen();
        }

        public void Close(Action onCompleted = null)
        {
            OnCloseCompletedAction = onCompleted;
            OnCloseCompletedAction?.Invoke();
            Animator.SetBool(IsOpen, false);
            OnClose();
        }

        public void OnOpenCompleted() => OnOpenCompletedAction?.Invoke();

        public void OnCloseCompleted()
        {
            gameObject.SetActive(false);
            OnCloseCompletedAction?.Invoke();
        }

        public abstract void OnOpen();
        public abstract void OnClose();
    }
}
