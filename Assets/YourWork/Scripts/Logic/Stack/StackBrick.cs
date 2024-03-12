using System;
using System.Linq;
using Jenga.Data;
using UnityEngine;

namespace Jenga.Logic.Stack
{
    public class StackBrick : MonoBehaviour
    {
        private MeshRenderer _renderer;
        private MeshRenderer Renderer => _renderer ??= GetComponent<MeshRenderer>();
        
        private BrickInfo BrickInfo { get; set; }

        public void Initialize(BrickInfo brickInfo, params Material[] materials)
        {
            BrickInfo = brickInfo;
            Renderer.SetMaterials(materials.ToList());
        }

        public BrickInfo SetSelected(bool setSelected)
        {
            Renderer.materials[1].SetFloat("_Size", setSelected ? 1.1f : 0);
            return BrickInfo;
        }
    }
}
