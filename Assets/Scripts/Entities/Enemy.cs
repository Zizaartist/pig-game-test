using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : Creature
{
    protected override float MoveTime => 1.0f;

    public override void Collision(IWorldObject newObj)
    {
        switch(newObj)
        {
            case Player player: 
                player.Remove();
                break;
            case Explosion explosion:
                Remove();
                break;
            default: break;
        }
    }
}
