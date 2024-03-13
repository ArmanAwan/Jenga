using System.Collections;
using System.Collections.Generic;
using Jenga.Logic;
using Jenga.Logic.Stack;
using UnityEngine;

public class TestStackGame
{
    private static int? FallenStackIndex { get; set; }

    public static void TestStack()
    {
        if (FallenStackIndex == null)
        {
            foreach (StackBrick brick in StackManager.Stacks[GameManager.StackIndex].Bricks)
            {
                brick.TestBrick(false);
            }

            FallenStackIndex = GameManager.StackIndex;
            return;
        }

        ResetStack();
    }

    public static void ResetStack()
    {
        if (FallenStackIndex == null) return;
        foreach (StackBrick brick in StackManager.Stacks[FallenStackIndex.Value].Bricks)
        {
            brick.TestBrick(true);
        }

        StackManager.BuildStack(FallenStackIndex.Value);
        FallenStackIndex = null;
    }
}