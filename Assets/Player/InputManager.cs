using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
    //global var input manager
    private static PlayerInput playerInput = new PlayerInput();

    public static PlayerInput GetPlayerInput()
    {
        return playerInput;
    }

    public static void SetInputEnabled(bool enabled)
    {
        if(enabled)
        {
            playerInput.Enable();
        }
        else
        {
            playerInput.Disable();
        }
    }
}
