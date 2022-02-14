using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : Creature
{
    public UnityEvent PlaceBombEvent;
    public UnityEvent PlayerDied;

    protected override float MoveTime => 0.6f;

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
    
    public override void Remove()
    {
        PlayerDied.Invoke();
        PlayerDied.RemoveAllListeners();
        base.Remove();
    }

    public void PlaceBomb() => PlaceBombEvent.Invoke();
}
