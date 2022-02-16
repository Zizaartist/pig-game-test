using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : Creature
{
    public UnityEvent PlaceBombEvent;
    public UnityEvent PlayerDied;

    protected override float MoveTime => 0.6f;
    private const float DirtDebuffDuration = 3.0f;

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

    
    public override void Remove()
    {
        PlayerDied.Invoke();
        PlayerDied.RemoveAllListeners();
        PlaceBombEvent.RemoveAllListeners();
        base.Remove();
    }

    public void PlaceBomb() => PlaceBombEvent.Invoke();
}
