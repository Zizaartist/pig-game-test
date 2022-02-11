using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, IWorldObject
{
    public void Remove() => Destroy(this);

    public void Sort(int id) => this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = id;
}