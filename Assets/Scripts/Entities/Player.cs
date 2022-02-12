using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : Creature
{
    public UnityEvent PlaceBombEvent;

    protected override float MoveTime => 1.0f;

    public override void Collision(IWorldObject newObj)
    {
        switch(newObj)
        {
            case Enemy enemy:
                Remove();
                break;
            case Explosion explosion:
                Remove();
                break;
            default: break;
        }
    }

    public void PlaceBomb() => PlaceBombEvent.Invoke();
}
