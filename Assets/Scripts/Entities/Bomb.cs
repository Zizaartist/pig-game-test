using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bomb : Entity
{
    private const float Lifetime = 2.0f;
    public UnityEvent<Vector2Int> Exploded;

    private void Start() {
        StartCoroutine(BombTimer());
    }

    private IEnumerator BombTimer()
    {
        yield return new WaitForSeconds(Lifetime);
        Exploded.Invoke(cell.Position);
        Exploded.RemoveAllListeners();
        Remove();
    }

    public override void Collision(IWorldObject newObj) {}
}
