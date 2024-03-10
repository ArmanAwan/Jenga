using System;
using Jenga.Data;
using UnityEngine;

namespace Jenga.Logic.Stack
{
    public class StackBrick : MonoBehaviour
    {
        private BrickInfo BrickInfo { get; set; }

        public void Initialize(BrickInfo brickInfo, Material material)
        {
            BrickInfo = brickInfo;
            GetComponent<MeshRenderer>().material = material;
        }
    }
}
