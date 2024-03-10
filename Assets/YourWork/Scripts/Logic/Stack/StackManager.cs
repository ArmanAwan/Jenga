using System.Collections.Generic;
using System.Linq;
using Jenga.Data;
using UnityEngine;

namespace Jenga.Logic.Stack
{
    public class StackManager : MonoBehaviour
    {
        [SerializeField]
        private StackBrick _stackBrickPrefab;
        private StackBrick StackBrickPrefab => _stackBrickPrefab;

        [SerializeField]
        private GradeStack _stackPrefab;
        private GradeStack StackPrefab => _stackPrefab;

        [SerializeField]
        private Material[] _brickMaterials;
        private Material[] BrickMaterials => _brickMaterials;

        private Dictionary<string, GradeStack> Stacks { get; set; } = new();

        public async void BuildStacks()
        {
            BrickInfo[] brickInfos = await DataLoader.LoadStackFromWeb();
            Dictionary<string, List<BrickInfo>> sortedBrickInfos = brickInfos.GroupBy(info => info.Grade)
                .ToDictionary(infos => infos.Key, infos => infos.ToList());

            const float brickWidth = 1.25f;
            const float brickHeight = 0.64f;
            const float stackWidth = 10f;

            Transform stacksHolder = new GameObject("StacksHolder").transform;
            int stackIndex = 0;
            foreach (KeyValuePair<string, List<BrickInfo>> stackInfos in sortedBrickInfos)
            {
                List<BrickInfo> sortedInfos = stackInfos.Value.OrderBy(info => info.Domain)
                    .ThenBy(info => info.Cluster)
                    .ThenBy(info => info.StandardId)
                    .ToList();
                Stacks[stackInfos.Key] = Instantiate(StackPrefab, parent: stacksHolder);
                
                Stacks[stackInfos.Key].GradeText.text = stackInfos.Key;
                Stacks[stackInfos.Key].transform.position = new Vector3(stackWidth * (stackIndex % 2 != 0 ? Mathf.CeilToInt(stackIndex/2f) : Mathf.CeilToInt(-stackIndex / 2f)),0,0);
                for (int i = 0; i < sortedInfos.Count; i++)
                {
                    float brickPosition = brickWidth * (i % 3) - brickWidth;
                    float brickYPosition = Mathf.FloorToInt(i / 3f) * brickHeight;

                    StackBrick brick = Instantiate(StackBrickPrefab, parent: Stacks[stackInfos.Key].transform);

                    if (Mathf.FloorToInt(i / 3f) % 2 == 0)
                    {
                        brick.transform.localPosition = new Vector3(brickPosition, brickYPosition, 0);
                        brick.transform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                    else
                    {
                        brick.transform.localPosition = new Vector3(0, brickYPosition, brickPosition);
                    }

                    brick.Initialize(sortedInfos[i], BrickMaterials[sortedInfos[i].Mastery]);
                    Stacks[stackInfos.Key].Bricks.Add(brick);
                }

                stackIndex++;
            }
        }
    }
}