using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Ячейка хранящая ноль и более "Объектов мира".
/// </summary>
public class Cell
{
    private List<IWorldObject> storedObjects;

    /// <summary>
    /// Добавляет объект в текущую ячейку
    /// </summary>
    /// <returns>Успешность операции</returns>
    public bool Add(IWorldObject newWorldObject)
    {
        if(!IsOccupied)
        {
            var storedObjectsBefore = storedObjects.ToList();
            storedObjects.Add(newWorldObject);
            newWorldObject.cell = this;
            newWorldObject.Sort(SortId);

            IsOccupied = newWorldObject.OccupiesSpace;

            if(storedObjectsBefore.Any())
                storedObjectsBefore.ForEach(wo => wo.Collision(newWorldObject));

            return true;
        }
        return false;
    }

    /// <summary>
    /// Извлекает объект из текущей ячейки
    /// </summary>
    public void Remove(IWorldObject worldObject)
    {
        storedObjects.Remove(worldObject);
        worldObject.cell = null;
        IsOccupied = false; // Во всех сценариях становится false
    }

    public void Clear() 
    {
        storedObjects.ForEach(wo => wo.cell = null);
        storedObjects.Clear();
        IsOccupied = false;
    }

    public List<IWorldObject> StoredObjects => storedObjects;

    public Cell(int sortId, Vector2Int position)
    {
        SortId = sortId;
        Position = position;
        storedObjects = new List<IWorldObject>();
    }

    ~Cell()
    {
        storedObjects.ForEach(wo => wo.cell = null);
    }

    public bool IsOccupied { get; private set; } = true;
    public bool CanWalk => IsOccupied;
    public int SortId { get; }
    public Vector2Int Position { get; }
}