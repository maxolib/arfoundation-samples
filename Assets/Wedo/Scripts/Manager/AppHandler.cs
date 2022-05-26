using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wedo {
    public class AppHandler : MonoBehaviour
    {
        public void LoadScene(string scene){
            AppManager.Instance.LoadScene(scene);
        }

        public void LoadSceneAsync(string scene){
            AppManager.Instance.LoadSceneAsync(scene);
        }
    }
}
