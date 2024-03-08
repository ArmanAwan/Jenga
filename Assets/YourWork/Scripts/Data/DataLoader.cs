using System.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace Jenga.Data
{
    public class DataLoader
    {
        public async Task<StackedSkill[]> LoadStackFromWeb()
        {
            const string url = "https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack";

            using UnityWebRequest webRequest = UnityWebRequest.Get(url);
            webRequest.SendWebRequest();
            while (webRequest.result == UnityWebRequest.Result.InProgress)
            {
                await Task.Yield();
            }

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                return null;
            }

            StackedSkill[] skills = JsonConvert.DeserializeObject<StackedSkill[]>(webRequest.downloadHandler.text);
            return skills;
        }
    }
}