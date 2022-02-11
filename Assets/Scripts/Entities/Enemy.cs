using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public Direction LookingDirection;

    public enum Direction 
    {
        up,
        down,
        left,
        right
    }
}
