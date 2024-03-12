using System.Collections;
using System.Collections.Generic;
using Jenga.Logic.Stack;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float CameraMoveSpeed = 25f;

    private float _zoomDistance = 25f;
    private float ZoomDistance
    {
        get => _zoomDistance;
        set => _zoomDistance = Mathf.Clamp(value, 4f, 35f);
    }

    private float _orbitHeight;
    private float OrbitHeight
    {
        get => _orbitHeight;
        set => _orbitHeight = Mathf.Clamp(value, 0f, 10f);
    }

    private static int _stackIndex;
    private static int StackIndex
    {
        get => _stackIndex;
        set => _stackIndex = value < 0 ? StackManager.Stacks.Length - 1 : value % StackManager.Stacks.Length;
    }

    private static Vector3 TargetPosition { get; set; }
    private static Vector3 TargetLerpPoint { get; set; }
    private static Vector3 CameraLerpPoint { get; set; }

    private bool IsDragging { get; set; }
    private Vector3 LastMousePosition { get; set; }

    public static void ResetView()
    {
        SwitchOrbit(StackManager.Stacks.Length / 2);
        CameraLerpPoint = TargetLerpPoint + new Vector3(0, 15, -20);
    }

    public static void SetStack(bool nextStack) =>
        SwitchOrbit(nextStack ? StackIndex + 1 : StackIndex - 1);

    private static void SwitchOrbit(int newStackIndex)
    {
        StackIndex = newStackIndex;
        TargetLerpPoint = StackManager.GetStackCentre(newStackIndex);
        CameraLerpPoint = TargetLerpPoint + (CameraLerpPoint - TargetPosition);
    }

    private void Update()
    {
        transform.LookAt(TargetPosition, Vector3.up);
        transform.position = Vector3.Lerp(transform.position, CameraLerpPoint, Time.deltaTime * CameraMoveSpeed);
        TargetPosition = Vector3.Lerp(TargetPosition, TargetLerpPoint, Time.deltaTime * CameraMoveSpeed);
        ChangeStack();
        ChangeZoom();
        ChangeHeight();
        ChangeOrbit();
    }

    static void ChangeStack()
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

    private void ChangeZoom()
    {
        const float zoomSpeed = 20f;
        if (Input.GetKey(KeyCode.W))
        {
            ZoomDistance -= Time.deltaTime * zoomSpeed;
            UpdateLerpPosition();
        }

        if (Input.GetKey(KeyCode.S))
        {
            ZoomDistance += Time.deltaTime * zoomSpeed;
            UpdateLerpPosition();
        }
    }

    private void ChangeHeight()
    {
        const float changeSpeed = 20f;
        if (Input.GetKey(KeyCode.Q))
        {
            OrbitHeight -= Time.deltaTime * changeSpeed;
            UpdateLerpPosition();
        }

        if (Input.GetKey(KeyCode.E))
        {
            OrbitHeight += Time.deltaTime * changeSpeed;
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

    private void UpdateLerpPosition()
    {
        Vector3 direction = (CameraLerpPoint - TargetPosition).normalized;
        Vector3 offsetPosition = TargetPosition + direction * ZoomDistance;
        CameraLerpPoint =  offsetPosition;
    }
}