using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorldObject
{
    /// <summary>
    /// Уничтожает instance
    /// </summary>
    public void Remove();

    /// <summary>
    /// Изменяет id порядка сортировки
    /// </summary>
    public void Sort(int id);
    
}
