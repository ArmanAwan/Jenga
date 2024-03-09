using System.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace Jenga.Data
{
    public class DataLoader
    {
        public static async Task<BrickInfo[]> LoadStackFromWeb()
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
            BrickInfo[] skills = JsonConvert.DeserializeObject<BrickInfo[]>(webRequest.downloadHandler.text);
            return skills;
        }
    }
}