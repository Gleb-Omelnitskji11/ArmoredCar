using UnityEngine;

namespace GameUnits
{
    [RequireComponent(typeof(MeshRenderer))]
    public class GPUInstancingEnabler : MonoBehaviour
    {
        private void Awake()
        {
            // MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            // MeshRenderer renderer = GetComponent<MeshRenderer>();
            // renderer.SetPropertyBlock(materialPropertyBlock);
        }
    }
}