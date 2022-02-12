using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour, IWorldObject
{
    public virtual bool OccupiesSpace => true;

    public Cell cell { get; set; }

    public abstract void Collision(IWorldObject newObj);

    public void Remove() 
    {
        cell.Remove(this);
        Destroy(this);
    }

    public void Sort(int id) => this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = id;
}