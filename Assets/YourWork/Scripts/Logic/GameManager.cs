using System.Threading.Tasks;
using Jenga.Logic.Camera;
using Jenga.Logic.Stack;
using UnityEngine;

namespace Jenga.Logic
{
    public class GameManager : MonoBehaviour
    {
        private static int _stackIndex;
        public static int StackIndex
        {
            get => _stackIndex;
            set => _stackIndex = value < 0 ? StackManager.Stacks.Length - 1 : value % StackManager.Stacks.Length;
        }
        
        private void Start() =>
            CriticalLoad();

        private async Task CriticalLoad()
        {
            await GetComponent<StackManager>().InitializeStacks();
            CameraController.ResetView();
        }
    }
}
