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
        var storedObjectsBefore = storedObjects.ToList();
        storedObjects.Add(newWorldObject);
        newWorldObject.cell = this;
        newWorldObject.Sort(SortId);

        UpdateOccupationState();

        if(storedObjectsBefore.Any())
            storedObjectsBefore.ForEach(wo => wo.Collision(newWorldObject));

        return true;
    }

    /// <summary>
    /// Извлекает объект из текущей ячейки
    /// </summary>
    public void Remove(IWorldObject worldObject)
    {
        storedObjects.Remove(worldObject);
        worldObject.cell = null;
        UpdateOccupationState();
    }

    public void Clear()
    {
        storedObjects.ToList().ForEach(wo => wo.Remove());
        IsOccupied = false;
    }

    private void UpdateOccupationState() => IsOccupied = storedObjects.Any(wo => wo.OccupiesSpace);

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

    public bool IsOccupied { get; private set; } = false;
    public bool CanWalk => IsOccupied;
    public int SortId { get; }
    public Vector2Int Position { get; }
}