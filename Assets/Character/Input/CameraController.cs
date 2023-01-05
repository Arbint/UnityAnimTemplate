using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float CameraHeightOffset = 2f;
    [SerializeField] float CameraArmLength = 2f;
    [SerializeField] float turnSpeed = 8f;
    [SerializeField] float pitchSpeed = 8f;
    [SerializeField] Vector2 pitchLimit = new Vector2(-80, 80);

    PlayerInput playerInput;
    Transform yawPivot;
    Transform pitchPivot;

    public Vector2 LookInput { get; private set; }
    float pitchAngle = 0;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = InputManager.GetPlayerInput();

        Cursor.lockState = CursorLockMode.Locked;
        playerInput.Gameplay.Look.performed += ProcessLookInput;
        GameObject yawPivotGO = new GameObject("CameraYaw");
        GameObject pitchPivotGO = new GameObject("CameraPitch");

        yawPivot = yawPivotGO.transform;
        pitchPivot = pitchPivotGO.transform;

        pitchPivot.parent = yawPivot;
        
        Camera.main.transform.position = Vector3.zero;
        Camera.main.transform.rotation = Quaternion.identity;

        Camera.main.transform.parent = pitchPivot;
        Camera.main.transform.localPosition += Vector3.forward * -CameraArmLength;
    }

    private void ProcessLookInput(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        LookInput = obj.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void LateUpdate()
    {
        FollowPlayer();
        ProcessLook();
    }

    private void ProcessLook()
    {
        //Turn
        yawPivot.Rotate(Vector3.up, LookInput.x * turnSpeed * Time.deltaTime);
        
        //Pitch
        pitchAngle = Mathf.Clamp(pitchAngle - LookInput.y * pitchSpeed * Time.deltaTime, pitchLimit.x, pitchLimit.y);
        pitchPivot.localRotation = Quaternion.Euler(pitchAngle, 0f, 0f);
    }

    private void FollowPlayer()
    {
        yawPivot.transform.position = transform.position + Vector3.up * CameraHeightOffset;
    }

    public Vector3 GetFlatForwardDir()
    {
        return yawPivot.forward;
    }

    public Vector3 GetRightDir()
    {
        return yawPivot.right;
    }
}
