using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    [Header(
        "Controls: \nW, S, A, D to Move\nMove mouse to look" +
        "\nLeft click to attack" +
        "\nE to do emote" +
        "\nK to kill self"+
        "\n\nHow to make Adjustment:"+
        "\n1,Use the Movement Component to adjust the movement related values" +
        "\n\n2,Use the Camera Controller to control camera location and trun speed" +
        "\n\n3,Add the combo animations to the combos list in the Anim Event Component" +
        "\nThe combo clear wait time for each combo is how log it waits after" +
        "\na combo is fired before setting combo back to the first combo" + 
        "\n\n4, Character Controller defines the actually collision of the player" +
        "\n\n5, Anything you wish the player to walk on need to be in the ground layer" +
        "\n\n6, the jump land animation needs to be set to additive." +
        "\n\n7, if you wish to adjust the animator, keep upperBody and Fullbody layer" +
        "\nthe same, you can adjust one of them, and copy everything to the other one.")
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
