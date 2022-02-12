using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    public Player CurrentPlayer { get; set; }
    private Grid grid;
    private Direction? LastMemorizedMove;

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
        if(Input.GetKey(KeyCode.W))
        {
            LastMemorizedMove = Direction.up;
        }
        else if(Input.GetKey(KeyCode.A))
        {
            LastMemorizedMove = Direction.left;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            LastMemorizedMove = Direction.down;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            LastMemorizedMove = Direction.right;
        }
        if(LastMemorizedMove != null)
        {
            if(CurrentPlayer?.CanMove ?? false)
            {
                CurrentPlayer.Move(LastMemorizedMove ?? Direction.up);
                LastMemorizedMove = null;
            }
        }
    }

    private IEnumerator ArtificialDelay() {yield return new WaitForSeconds(1);}

    public void PlaceBomb()
    {
        grid.SpawnBombUnderPlayer(CurrentPlayer);
    }
}