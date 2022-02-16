using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Entity
{
    private const float Lifetime = 1.5f;

    private void Start() {
        StartCoroutine(ExplosionTimer());   
    }

    public override void Collision(IWorldObject newObj)
    {
        switch(newObj)
        {
            case Player player:
                player.IsDirty = true;
                break;
            case Enemy enemy:
                enemy.IsDirty = true;
                break;
            default: break;
        }
    }

    private IEnumerator ExplosionTimer()
    {
        yield return new WaitForSeconds(Lifetime);
        Remove();
    }
}
