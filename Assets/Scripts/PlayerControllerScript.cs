using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

[RequireComponent(typeof(PlayerInput))]
public class PlayerControllerScript : MonoBehaviour
{
    public Player CurrentPlayer { get; set; }
    private Grid grid;
    private Direction? LastMemorizedMove;
    public PlayerInput input;

    // Update is called once per frame
    void Update()
    {
        if(CurrentPlayer != null)
        {
            ProcessInputs();
        }
    }

    private void ProcessInputs()
    {
        if(LastMemorizedMove != null)
        {
            if(CurrentPlayer?.CanMove ?? false)
            {
                CurrentPlayer.Move(LastMemorizedMove.Value);
            }
        }
    }

    public void DetachPlayer() => CurrentPlayer = null;

    public void OnMove()
    {
        var stickPos = input.actions.FindAction("Move").ReadValue<Vector2>();
        var x2 = stickPos.x * stickPos.x;
        var y2 = stickPos.y * stickPos.y;
        
        if(stickPos.magnitude > 0.2f) // ignore small movements from center
        {
            if(x2 >= y2 && stickPos.x >= 0) // right
            {
                LastMemorizedMove = Direction.right;
            }
            else if(x2 >= y2 && stickPos.x < 0) // left
            {
                LastMemorizedMove = Direction.left;
            }
            else if(y2 > x2 && stickPos.y >= 0) // up
            {
                LastMemorizedMove = Direction.up;
            }
            else if(y2 > x2 && stickPos.y < 0) // down
            {
                LastMemorizedMove = Direction.down;
            }
        }
        else
        {
            LastMemorizedMove = null;
        }
    }

    public void PlaceBomb() => CurrentPlayer?.PlaceBomb();
}
