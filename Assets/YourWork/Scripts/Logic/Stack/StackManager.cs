using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        [SerializeField]
        private Material _outlineMaterial;
        private Material OutlineMaterial => _outlineMaterial;
        

        public static GradeStack[] Stacks { get; private set; }
        
        public async Task InitializeStacks()
        {
            List<GradeStack> gradeStacks = new();
            BrickInfo[] brickInfos = await DataLoader.LoadStackFromWeb();
            Dictionary<string, List<BrickInfo>> sortedBrickInfos = brickInfos.GroupBy(info => info.Grade)
                .ToDictionary(infos => infos.Key, infos => infos.ToList());

            const float stackWidth = 10f;

            Transform stacksHolder = new GameObject("StacksHolder").transform;
            int stackIndex = 0;
            foreach (KeyValuePair<string, List<BrickInfo>> stackInfos in sortedBrickInfos)
            {
                List<BrickInfo> sortedInfos = stackInfos.Value.OrderBy(info => info.Domain)
                    .ThenBy(info => info.Cluster)
                    .ThenBy(info => info.StandardId)
                    .ToList();
                gradeStacks.Add(Instantiate(StackPrefab, parent: stacksHolder));
                
                gradeStacks[stackIndex].name = gradeStacks[stackIndex].GradeText.text = stackInfos.Key;
                gradeStacks[stackIndex].transform.position = new Vector3(stackIndex * stackWidth,0,0);
                for (int i = 0; i < sortedInfos.Count; i++)
                {
                    StackBrick brick = Instantiate(StackBrickPrefab, parent: gradeStacks[stackIndex].transform);
                    brick.Initialize(sortedInfos[i], BrickMaterials[sortedInfos[i].Mastery], OutlineMaterial);
                    gradeStacks[stackIndex].Bricks.Add(brick);
                }
                BuildStack(gradeStacks[stackIndex]);
                stackIndex++;
            }
            Stacks = gradeStacks.ToArray();
        }

        public static void BuildStack(int stackIndex)
        {
            BuildStack(Stacks[stackIndex]);
        }

        private static void BuildStack(GradeStack stack)
        {
            const float brickWidth = 1.25f;
            const float brickHeight = 0.62f;
            
            for (int i = 0; i < stack.Bricks.Count; i++)
            {
                float brickPosition = brickWidth * (i % 3) - brickWidth;
                float brickYPosition = Mathf.FloorToInt(i / 3f) * brickHeight;

                if (Mathf.FloorToInt(i / 3f) % 2 == 0)
                {
                    stack.Bricks[i].transform.localPosition = new Vector3(brickPosition, brickYPosition, 0);
                    stack.Bricks[i].transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                else
                {
                    stack.Bricks[i].transform.localPosition = new Vector3(0, brickYPosition, brickPosition);
                    stack.Bricks[i].transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }

        public static Vector3 GetStackCentre(int stackIndex)
        {
            Vector3 stackPos = Stacks[stackIndex].transform.position;
            float yPos = Stacks[stackIndex].Bricks[Stacks[stackIndex].Bricks.Count / 2].transform.position.y;
            stackPos += yPos * Vector3.up;
            return stackPos;
        }
    }
}