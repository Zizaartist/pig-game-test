using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private IWorldObject storedObject;
    public IWorldObject StoredObject 
    { 
        get => storedObject; 
        set
        {
            storedObject = value;
            if(value == null)
            {
                storedObject?.Remove();
                IsOccupied = false;
                CanWalk = false;
            }
            else
            {
                storedObject.Sort(SortId);
                IsOccupied = true;
                CanWalk = true;
            }
        } 
    }

    public Cell(int sortId)
    {
        SortId = sortId;
    }

    public bool IsOccupied { get; private set; } = true;
    public bool CanWalk { get; private set; } = true;
    public int SortId { get; }
}