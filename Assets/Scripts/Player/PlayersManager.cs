using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class control how many type of inputs a device can give
//eg: a pc can only give 2 player on the keyboard at max but 4 controller input and any other viable combination

public class PlayersManager : MonoBehaviour
{
    public List<ControllerType> playerControllers = new List<ControllerType>();
    public List<int> gamepadUsed = new List<int>();

    public bool StorePlayerControlType(ControllerType type, int gamepadNum = -1)
    {
        if(playerControllers.Contains(type) && type != ControllerType.GAMEPAD)
        {
            playerControllers.Remove(type);

            return false;
        }

        if(type == ControllerType.GAMEPAD && gamepadUsed.Contains(gamepadNum))
        {
            playerControllers.Remove(type);
            gamepadUsed.Remove(gamepadNum);

            return false;
        }

        if(type == ControllerType.GAMEPAD)
        {
            gamepadUsed.Add(gamepadNum);
        }

        playerControllers.Add(type);

        return true;
    }
}
