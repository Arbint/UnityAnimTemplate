using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovmentComponent : MonoBehaviour
{
    public bool OnGround { get; private set; }
    public Vector3 Velocity { get; private set; }

    PlayerInput playerInput;
    Vector2 moveInput;
    [SerializeField] float MoveMaxSpeed = 20f;
    [SerializeField] float MoveAcceleration = 15f;
    [SerializeField] float MoveDecceleration = 20f;
    [SerializeField] float MoveAirDecceleration = 5f;
    [SerializeField] float BodyRotateSpeed = 20f;
    [SerializeField] float jumpSpeed = 3f;
    
    CharacterController characterController;
    CameraController cameraController;
    GroundChecker groundChecker;

    float verticalSpeed = 0;
    float MoveSpeed = 0;
    public Vector3 MoveDir {get; private set; }

    Vector3 previousLoc;

    private void Start()
    {
        playerInput = InputManager.GetPlayerInput();
        
        playerInput.Gameplay.Move.performed += ProcessMoveInput;
        playerInput.Gameplay.Jump.performed += Jump;

        cameraController = GetComponent<CameraController>();
        characterController = GetComponent<CharacterController>();
        groundChecker = GetComponent<GroundChecker>();
        previousLoc = transform.position;
    }

    internal Vector2 GetMovementInput()
    {
        return moveInput;
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
            OnGround = true;
        }
       }

    private void Update()
    {
        CalculateMoveDir();
        CalculateMoveSpeed();
        ProcessMovement();
        ProcessBodyRotation();
        CalculateVel();
    }

    private void CalculateMoveSpeed()
    {
        if(moveInput.magnitude > 0)
        {
            MoveSpeed += Time.deltaTime * MoveAcceleration;
        }
        else
        {
            if(groundChecker.IsOnGround())
            {
                MoveSpeed -= Time.deltaTime * MoveDecceleration;
            }
            else
            {
                MoveSpeed -= Time.deltaTime * MoveAirDecceleration;
            }
        }
        MoveSpeed = Mathf.Clamp(MoveSpeed, 0, MoveMaxSpeed);
    }

    private void CalculateVel()
    {
        Vector3 locDelta = transform.position - previousLoc;
        Velocity = locDelta / Time.deltaTime;
        previousLoc = transform.position;
    }

    private void CalculateMoveDir()
    {
        Vector3 FaceDir = cameraController.GetFlatForwardDir();
        Vector3 RightDir = cameraController.GetRightDir();
        if(moveInput.magnitude > 0)
        {
            MoveDir = (FaceDir * moveInput.y + RightDir * moveInput.x).normalized;
        }
        else
        {
            MoveDir = MoveSpeed > 0 ? MoveDir : Vector3.zero;
        }
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
            OnGround = false;
            verticalSpeed -= Physics.gravity.magnitude * Time.deltaTime;
        }
    }

}
