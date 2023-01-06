using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    [Header("How to use:" +
        "\n1,Use the Movement Component to adjust the movement related values" +
        "\n\n2,Use the Camera Controller to control camera location and trun speed" +
        "\n\n3,Add the combo animations to the combos list in the Anim Event Component" +
        "\nThe combo clear wait time for each combo is how log it waits after" +
        "\na combo is fired before setting combo back to the first combo" + 
        "\n\n4, Character Controller defines the actually collision of the player" +
        "\n\n5, Anything you wish the player to walk on need to be in the ground layer" +
        "\n")
        ]
    [SerializeField] [Multiline] string Note;
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
