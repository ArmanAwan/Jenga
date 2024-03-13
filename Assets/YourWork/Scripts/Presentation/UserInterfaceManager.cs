using System;
using System.Collections;
using System.Collections.Generic;
using Jenga.Data;
using Jenga.Logic.Stack;
using TMPro;
using UnityEngine;

public class UserInterfaceManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _blockInfo;
    private TMP_Text BlockInfo => _blockInfo;
    
    private StackBrick SelectedBrick { get; set; }

    private void Start()
    {
        SetDefaultInformation();
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
        BrickInfo info =  stackBrick.SetSelected(true);
        BlockInfo.text = $"{info.Grade}: {info.Domain}\n{info.Cluster}\n{info.StandardId}: {info.StandardDescription}";

    }
}