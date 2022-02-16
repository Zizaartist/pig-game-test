using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour, IWorldObject
{
    public virtual bool OccupiesSpace => true;

    public Cell cell { get; set; }

    public bool MarkedForDestruction => throw new System.NotImplementedException();

    public abstract void Collision(IWorldObject newObj);

    public virtual void Remove() 
    {
        cell.Remove(this);
        Debug.Log($"Destroyed - {this.GetType()}");
        Destroy(gameObject);
    }

    public void Sort(int id) => this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = id;
}