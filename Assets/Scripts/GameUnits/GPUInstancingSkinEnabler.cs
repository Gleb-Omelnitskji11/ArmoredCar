using UnityEngine;

namespace GameUnits
{
    [RequireComponent(typeof(SkinnedMeshRenderer))]
    public class GPUInstancingSkinEnabler : MonoBehaviour
    {
        private void Awake()
        {
            // MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            // SkinnedMeshRenderer renderer = GetComponent<SkinnedMeshRenderer>();
            // renderer.SetPropertyBlock(materialPropertyBlock);
        }
    }
}