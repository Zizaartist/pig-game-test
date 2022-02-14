using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Creature : Entity
{
    public List<Sprite> DirectionSprites; // каждый индекс соответствует int значениям Direction
    public UnityEvent<Direction> MoveEvent;
    public Dictionary<Direction, bool> currentVision { get; set; }

    private Direction lookingDirection = Direction.down;
    private Direction LookingDirection
    {
        get => lookingDirection;
        set
        {
            lookingDirection = value;
            GetComponent<SpriteRenderer>().sprite = DirectionSprites[(int)lookingDirection];
        }
    }
    
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
            LookingDirection = dir;
            MoveEvent.Invoke(dir);
            StartCoroutine(MoveTimer());
        }
    }

    public void AnimateMovement(Vector3 dest)
    {
        var currPos = transform.position;
        var distance = dest - currPos;
        StartCoroutine(Animation(distance, dest));
    }

    private IEnumerator Animation(Vector3 distance, Vector3 dest)
    {
        var plannedDistanceLength = distance.magnitude;
        var currentDistanceLength = 0f;
        var IsMoving = true;
        while (IsMoving)
        {
            yield return null;
            var increment = distance * Time.deltaTime / MoveTime;
            currentDistanceLength += increment.magnitude;
            if(plannedDistanceLength >= currentDistanceLength)
            {
                transform.position += increment;
            }
            else
            {
                IsMoving = false;
                transform.position = dest;
            }
        }
    }
}
