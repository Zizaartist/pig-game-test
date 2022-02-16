using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Creature : Entity
{
    public List<Sprite> DirectionSprites; // каждый индекс соответствует int значениям Direction
    public List<Sprite> DirtyDirectionSprites;
    public UnityEvent<Direction> MoveEvent;
    public UnityEvent GotDirty;
    public Dictionary<Direction, bool> currentVision { get; set; }

    private Direction lookingDirection = Direction.down;
    private Direction LookingDirection
    {
        get => lookingDirection;
        set
        {
            lookingDirection = value;
            UpdateSprite();
        }
    }
    
    protected abstract float MoveTime { get; }
    public bool CanMove { get; private set; } = true;
    private bool isDirty;
    public bool IsDirty 
    {
        get => isDirty;
        set
        {
            if(isDirty == value) return;
            isDirty = value;
            UpdateSprite();
            if(isDirty) GotDirty.Invoke();
        }
    }
    private const float DirtCoef = 2.0f;

    private void UpdateSprite()
    {
        GetComponent<SpriteRenderer>().sprite = IsDirty ? DirtyDirectionSprites[(int)lookingDirection] : 
                                                          DirectionSprites[(int)lookingDirection];
    }

    private IEnumerator MoveTimer() 
    { 
        yield return new WaitForSeconds(MoveTime * (IsDirty ? DirtCoef : 1f)); 
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

    public override void Remove() 
    {
        MoveEvent.RemoveAllListeners();
        GotDirty.RemoveAllListeners();
        base.Remove();
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
            var increment = distance * Time.deltaTime / MoveTime / (IsDirty ? DirtCoef : 1f);
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
