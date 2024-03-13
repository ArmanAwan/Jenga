using System;
using System.Linq;
using Jenga.Data;
using UnityEngine;

namespace Jenga.Logic.Stack
{
    public class StackBrick : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody _rigidbody;
        private Rigidbody Rigidbody => _rigidbody;
        
        
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
            Renderer.materials[1].SetFloat("_Size", setSelected ? 1.05f : 0);
            return BrickInfo;
        }

        public void TestBrick(bool reset)
        {
            if (BrickInfo.Mastery == 0 && !reset)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);
            Rigidbody.isKinematic = reset;
        }
    }
}
