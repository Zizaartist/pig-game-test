using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс принимающий решения передвижения существа
/// </summary>
public abstract class CreatureAI : MonoBehaviour
{
    public Creature body { protected get; set; }

    void Update()
    {
        // возможно лучше было бы использовать событие о готовности чем теребить флаг CanMove каждый фрэйм, но не смертельно
        if(body?.CanMove ?? false && IntendedMovementDirection != null)
        {
            body.Move(IntendedMovementDirection.Value);
        }
    }

    protected Direction? IntendedMovementDirection;

    protected abstract void DecideNextMove();
}
