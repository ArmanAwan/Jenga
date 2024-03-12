using System;
using System.Threading.Tasks;
using Jenga.Data;
using Jenga.Logic.Stack;
using UnityEngine;

namespace Jenga.Logic
{
    public class GameManager : MonoBehaviour
    {
        
        
        private void Start() =>
            CriticalLoad();

        private async Task CriticalLoad()
        {
            await GetComponent<StackManager>().InitializeStacks();
            CameraController.ResetView();
        }
    }
}
