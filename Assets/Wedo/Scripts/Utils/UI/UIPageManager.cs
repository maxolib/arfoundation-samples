using System;
using UniRx;
using UnityEngine;

namespace Wedo
{
    public class UIPageManager : MonoBehaviour
    {
        public UIPage[] Pages;
        public UIPage StartPage;
        public float delay = 1;

        [HideInInspector]
        public UIPage CurrentPage;

        private bool IsAnimate;

        private void Start()
        {
            foreach (var page in Pages)
            {
                page.gameObject.SetActive(false);
            }
            
            if(StartPage)
                Open(StartPage);
        }

        public void Open(UIPage page)
        {
            if (IsAnimate) return;
            
            IsAnimate = true;
            Observable.Timer(TimeSpan.FromSeconds(delay)).Subscribe(_ =>
            {
                if (CurrentPage)
                {
                    CurrentPage.Close(() =>
                    {
                        CurrentPage = page;
                        page.Open();
                    });
                }
                else
                {
                    CurrentPage = page;
                    page.Open();
                }

                IsAnimate = false;
            }).AddTo(this);
        }

        public void Close()
        {
            if (IsAnimate) return;
            
            if (!CurrentPage) return;

            IsAnimate = true;
            CurrentPage.Close(() =>
            {
                CurrentPage = null;
                IsAnimate = false;
            });
        }
    }
}
