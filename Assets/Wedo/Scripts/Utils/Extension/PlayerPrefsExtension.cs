using UnityEngine;

namespace Wedo
{
    public static class PlayerPrefsExtension
    {
        public static Vector3 GetVector3(string name, Vector3 defaultVector)
        {
            return new Vector3(
                    PlayerPrefs.GetFloat($"{name}_X", defaultVector.x),
                    PlayerPrefs.GetFloat($"{name}_Y", defaultVector.y),
                    PlayerPrefs.GetFloat($"{name}_Z", defaultVector.z)
                );
        }
        
        public static void SetVector3(string name, Vector3 vector)
        {
            PlayerPrefs.SetFloat($"{name}_X", vector.x);
            PlayerPrefs.SetFloat($"{name}_Y", vector.y);
            PlayerPrefs.SetFloat($"{name}_Z", vector.z);
        }
    }
}
