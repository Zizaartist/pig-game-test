using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс принимающий решения передвижения существа
/// </summary>
public abstract class CreatureAI : MonoBehaviour
{
    [SerializeField]
    public Creature body;

    public bool IsActive { get; set; } = false;

    void Update()
    {
        // возможно лучше было бы использовать событие о готовности чем теребить флаг CanMove каждый фрэйм, но не смертельно
        if(IsActive && (body?.CanMove ?? false))
        {
            DecideNextMove();
            if(IntendedMovementDirection != null)
                body.Move(IntendedMovementDirection.Value);
        }
    }

    [SerializeField]
    protected Direction? IntendedMovementDirection;

    protected abstract void DecideNextMove();
}
