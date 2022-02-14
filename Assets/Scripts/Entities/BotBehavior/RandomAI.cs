using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAI : CreatureAI
{
    protected override void DecideNextMove()
    {
        var randomIndex = (int) (Random.value * 4f);
        IntendedMovementDirection = (Direction) randomIndex;
    }
}
