using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorldObject
{
    /// <summary>
    /// Уничтожает экземпляр
    /// </summary>
    public void Remove();

    /// <summary>
    /// Изменяет id порядка сортировки
    /// </summary>
    public void Sort(int id);

    public void Collision(IWorldObject newObj);

    /// <summary>
    /// Занимает ли объект всю ячейку карты
    /// </summary>
    public bool OccupiesSpace { get; }

    public Cell cell { get;  set; } //прощай инкапсуляция ;_;
}
