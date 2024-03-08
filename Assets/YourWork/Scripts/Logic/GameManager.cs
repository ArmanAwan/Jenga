using System.Threading.Tasks;
using Jenga.Data;
using UnityEngine;

namespace Jenga.Logic
{
    public class GameManager : MonoBehaviour
    {
        private void Start() =>
            CriticalLoad();

        private void CriticalLoad()
        {
            Task<StackedSkill[]> stackList = DataLoader.LoadStackFromWeb();
        }
    }
}
