using System;
using UnityEngine;

namespace Utils
{
    public class Singleton<T> : MonoBehaviour
    {
        public static T Instance;

        private protected void Awake()
        {
            var component = GetComponent<T>();
            if (Instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Debug.Log("set Instance");
                Instance = component;
            }
        }
    }
}
