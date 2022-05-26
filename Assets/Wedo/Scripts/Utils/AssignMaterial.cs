using System;
using UnityEngine;

namespace Wedo
{
    public class AssignMaterial : MonoBehaviour
    {
        public Material TargetMaterial;

        private void Start()
        {
            SetMaterial();
        }

        private void SetMaterial()
        {
            if (!TargetMaterial) return;
            var meshRenderers = GetComponentsInChildren<MeshRenderer>();
            
            foreach (var meshRenderer  in meshRenderers)
            {
                Debug.Log(meshRenderer.name);
                if(meshRenderer)
                {
                    meshRenderer.material = TargetMaterial;
                }
            }
        }
    }
}
