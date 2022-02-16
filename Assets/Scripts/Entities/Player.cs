using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : Creature
{
    public UnityEvent PlaceBombEvent;
    public UnityEvent PlayerDied;
    public UnityEvent<bool> BombIsAvailable;

    protected override float MoveTime => 0.6f;
    private bool CanUseBombs = true;
    private const float DirtDebuffDuration = 3.0f;
    private const float BombCooldownTime = 3.0f;

    private void Awake() {
        GotDirty.AddListener(() => StartCoroutine(DirtDebuffCooldown()));
    }

    public override void Collision(IWorldObject newObj)
    {
        switch(newObj)
        {
            case Enemy enemy:
                Remove();
                break;
            case Explosion explosion:
                IsDirty = true;
                break;
            default: break;
        }
    }

    private IEnumerator DirtDebuffCooldown()
    {
        yield return new WaitForSeconds(DirtDebuffDuration);
        IsDirty = false;
    }

    private IEnumerator BombCooldown()
    {
        yield return new WaitForSeconds(BombCooldownTime);
        CanUseBombs = true;
        BombIsAvailable.Invoke(CanUseBombs);
    }
    
    public override void Remove()
    {
        PlayerDied.Invoke();
        PlayerDied.RemoveAllListeners();
        PlaceBombEvent.RemoveAllListeners();
        BombIsAvailable.RemoveAllListeners();
        base.Remove();
    }

    public void PlaceBomb() 
    {
        if(CanUseBombs) 
        {
            CanUseBombs = false;
            BombIsAvailable.Invoke(CanUseBombs);
            StartCoroutine(BombCooldown());
            PlaceBombEvent.Invoke();
        }
    } 
}
