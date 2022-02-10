using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public Vector2Int Boundaries { get; }
    private bool[,] OccupationArray;

    public Grid(Vector2Int boundaries)
    {
        Boundaries = boundaries;
        OccupationArray = new bool[boundaries.x, boundaries.y];
    }

    public Grid(int x, int y) : this(new Vector2Int(x, y)) {}

    //public 

    public Vector3 ChooseEnemySpawningPoint()
    {
        return Vector3.zero; //TO-DO implement
    }
}
