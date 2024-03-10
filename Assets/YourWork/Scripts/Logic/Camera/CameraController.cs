using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform TargetStack { get; set; }
    private const float OrbitSpeed = 5f;

    private bool IsDragging { get; set; }
    private Vector3 LastMousePosition{ get; set; }
    
    public void SwitchOrbit(Transform targetStack)
    {
        TargetStack = targetStack;
    }
    
    void Update()
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

        float rotationX = mouseDelta.y * OrbitSpeed * Time.deltaTime;
        float rotationY = -mouseDelta.x * OrbitSpeed * Time.deltaTime;

        transform.RotateAround(TargetStack.position, Vector3.up, rotationY);
        transform.RotateAround(TargetStack.position, transform.right, rotationX);

        LastMousePosition = Input.mousePosition;
    }
}
