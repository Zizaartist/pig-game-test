using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Creature : Entity
{
    public UnityEvent<Direction> MoveEvent;
    protected Dictionary<Direction, bool> currentView { get; private set; }

    private Direction LookingDirection = Direction.down;
    
    protected abstract float MoveTime { get; }
    public bool CanMove { get; private set; } = true;

    private IEnumerator MoveTimer() 
    { 
        yield return new WaitForSeconds(MoveTime); 
        CanMove = true;
    }

    public void Move(Direction dir)
    {
        if(CanMove)
        {
            CanMove = false;
            MoveEvent.Invoke(dir);
            StartCoroutine(MoveTimer());
        }
    }
}
