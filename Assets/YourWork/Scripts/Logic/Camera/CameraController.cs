using System.Collections;
using System.Collections.Generic;
using Jenga.Logic;
using Jenga.Logic.Stack;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float CameraMoveSpeed = 25f;

    private static float _zoomDistance = 25f;
    private static float ZoomDistance
    {
        get => _zoomDistance;
        set => _zoomDistance = Mathf.Clamp(value, 4f, 35f);
    }

    private static Vector3 TargetPosition { get; set; }
    private static Vector3 TargetLerpPoint { get; set; }
    private static Vector3 CameraLerpPoint { get; set; }

    private static bool IsDragging { get; set; }
    private static Vector3 LastMousePosition { get; set; }

    public static void ResetView()
    {
        SwitchOrbit(StackManager.Stacks.Length / 2);
        CameraLerpPoint = TargetLerpPoint + new Vector3(0, 15, -20);
    }

    public static void SetStack(bool nextStack) =>
        SwitchOrbit(nextStack ? GameManager.StackIndex + 1 : GameManager.StackIndex - 1);

    private static void SwitchOrbit(int newStackIndex)
    {
        TestStackGame.ResetStack();
        GameManager.StackIndex = newStackIndex;
        TargetLerpPoint = StackManager.GetStackCentre(GameManager.StackIndex);
        CameraLerpPoint = TargetLerpPoint + (CameraLerpPoint - TargetPosition);
    }

    private void Update()
    {
        transform.LookAt(TargetPosition, Vector3.up);
        transform.position = Vector3.Lerp(transform.position, CameraLerpPoint, Time.deltaTime * CameraMoveSpeed);
        TargetPosition = Vector3.Lerp(TargetPosition, TargetLerpPoint, Time.deltaTime * CameraMoveSpeed);
        ChangeStack();
        ChangeZoom(Time.deltaTime * 20f);
        ChangeOrbit();
    }

    private static void ChangeStack()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetStack(false);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            SetStack(true);
        }
    }

    public static void ChangeZoom(float zoomAmount)
    {
        if (Input.GetKey(KeyCode.W))
        {
            ZoomDistance -= zoomAmount;
            UpdateLerpPosition();
        }

        if (Input.GetKey(KeyCode.S))
        {
            ZoomDistance += zoomAmount;
            UpdateLerpPosition();
        }
    }

    private void ChangeOrbit()
    {
        if (Input.GetMouseButtonDown(1))
        {
            IsDragging = true;
            LastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(1))
        {
            IsDragging = false;
        }

        if (!IsDragging) return;

        Vector3 mouseDelta = Input.mousePosition - LastMousePosition;

        const float orbitSpeed = 6f;
        float rotationX = -mouseDelta.x * orbitSpeed * Time.deltaTime;
        float rotationY = -mouseDelta.y * orbitSpeed * Time.deltaTime;
        Vector3 deltaRotation = (transform.up * rotationY) + (transform.right * rotationX);
        CameraLerpPoint += deltaRotation;
        UpdateLerpPosition();
        LastMousePosition = Input.mousePosition;
    }

    private static void UpdateLerpPosition()
    {
        Vector3 direction = (CameraLerpPoint - TargetPosition).normalized;
        Vector3 offsetPosition = TargetPosition + direction * ZoomDistance;
        CameraLerpPoint = offsetPosition;
    }
}