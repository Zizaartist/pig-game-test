using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Entity
{
    private const float Lifetime = 1.0f;

    private void Start() {
        StartCoroutine(ExplosionTimer());   
    }

    public override void Collision(IWorldObject newObj)
    {
        switch(newObj)
        {
            case Player player:
                player.Remove();
                break;
            case Enemy enemy:
                enemy.Remove();
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
