using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using UnityEngine.SceneManagement;

namespace Wedo
{
    public class AppManager : Singleton<AppManager>
    {
        public void Start()
        {
            LoadScene("01.menu");
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void LoadSceneAsync(string sceneName)
        {
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}