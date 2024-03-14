using Jenga.Data;
using Jenga.Logic.Stack;
using ThirdParty.Misc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Jenga.Presentation
{
    public class UserInterfaceManager : MonoBehaviour
    {
        private enum ButtonType
        {
            ZoomIn,
            ZoomOut,
            StackLeft,
            StackRight,
            TestStack
        }

        [SerializeField]
        private GenericDictionary<ButtonType, Button> _buttons;
        private GenericDictionary<ButtonType, Button> Buttons => _buttons;
        

        [SerializeField]
        private TMP_Text _blockInfo;
        private TMP_Text BlockInfo => _blockInfo;

        private StackBrick SelectedBrick { get; set; }

        private void Start()
        {
            SetDefaultInformation();
            const float zoomAmount = 200f;
            Buttons[ButtonType.ZoomIn].onClick.AddListener(() => CameraController.ChangeZoom(zoomAmount));
            Buttons[ButtonType.ZoomOut].onClick.AddListener(() => CameraController.ChangeZoom(-zoomAmount));
            Buttons[ButtonType.StackRight].onClick.AddListener(() => CameraController.SetStack(true));
            Buttons[ButtonType.StackLeft].onClick.AddListener(() => CameraController.SetStack(false));
            Buttons[ButtonType.TestStack].onClick.AddListener(TestStackGame.TestStack);
            
        }

        private void SetDefaultInformation()
        {
            if (SelectedBrick)
            {
                SelectedBrick.SetSelected(false);
            }

            BlockInfo.text = "Please select Brick for more information";
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TestStackGame.TestStack();
            }

            if (!Input.GetMouseButtonDown(0)) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 1 << 6))
            {
                SetDefaultInformation();
                return;
            }

            StackBrick stackBrick = hit.collider.gameObject.GetComponent<StackBrick>();
            if (!stackBrick) return;
            if (SelectedBrick)
            {
                SelectedBrick.SetSelected(false);
            }

            SelectedBrick = stackBrick;
            BrickInfo info = stackBrick.SetSelected(true);
            BlockInfo.text = $"{info.Grade}: {info.Domain}\n{info.Cluster}\n{info.StandardId}: {info.StandardDescription}";
        }
    }
}