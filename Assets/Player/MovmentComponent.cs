using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovmentComponent : MonoBehaviour
{
    PlayerInput playerInput;
    Vector2 moveInput;
    [SerializeField] float MoveSpeed = 20f;
    [SerializeField] float BodyRotateSpeed = 20f;
    [SerializeField] float jumpSpeed = 3f;
    
    CharacterController characterController;
    CameraController cameraController;
    GroundChecker groundChecker;
    float verticalSpeed = 0;
    
    public Vector3 MoveDir {get; private set; }

    private void Start()
    {
        playerInput = InputManager.GetPlayerInput();
        
        playerInput.Gameplay.Move.performed += ProcessMoveInput;
        playerInput.Gameplay.Jump.performed += Jump;

        cameraController = GetComponent<CameraController>();
        characterController = GetComponent<CharacterController>();
        groundChecker = GetComponent<GroundChecker>();
    }
    private void ProcessMoveInput(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        moveInput = obj.ReadValue<Vector2>();
    }

    private void Jump(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if(groundChecker.IsOnGround())
        {
            verticalSpeed = jumpSpeed;
        }
    }

    private void Update()
    {
        CalculateMoveDir();
        ProcessMovement();
        ProcessBodyRotation();
    }

    private void CalculateMoveDir()
    {
        Vector3 FaceDir = cameraController.GetFlatForwardDir();
        Vector3 RightDir = cameraController.GetRightDir();
        MoveDir = (FaceDir * moveInput.y + RightDir * moveInput.x).normalized;
    }

    private void ProcessBodyRotation()
    {
        if (moveInput.magnitude == 0)
            return;

        Quaternion GoalRot = Quaternion.LookRotation(MoveDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, GoalRot, Time.deltaTime * BodyRotateSpeed);
    }

    private void ProcessMovement()
    {
        if (characterController != null && cameraController != null)
        {
            characterController.Move(MoveDir * MoveSpeed * Time.deltaTime + Vector3.up * verticalSpeed * Time.deltaTime);
        }
        if(!groundChecker.IsOnGround())
        {
            verticalSpeed -= Physics.gravity.magnitude * Time.deltaTime;
        }
    }
}
