using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    PlayerInput playerInput;

    // Start is called before the first frame update
    private void Awake()
    {
        InitPlayerInput();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitPlayerInput()
    {
        playerInput = InputManager.GetPlayerInput();
        InputManager.SetInputEnabled(true);
    }
}
