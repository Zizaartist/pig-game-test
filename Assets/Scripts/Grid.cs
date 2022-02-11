using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Grid : MonoBehaviour
{
    public Stone StonePrefab;
    public Bush BushPrefab;
    public Enemy EnemyPrefab;
    public Bomb BombPrefab;
    public Player PlayerPrefab;
    public GridLayout grid;

    private Cell[,] GridArray;
    private Vector3 cellOffset;

    [Range(2, 100)]
    public int Width = 17;
    [Range(2, 100)]
    public int Height = 9;
    public int BushCount = 10;
    public int EnemyCount = 5;

    private void Start() {
        Initialize();
    }

    public void Initialize()
    {
        GridArray = new Cell[Width, Height];
        for(int row = 0; row < Height; row++)
        {
            for(int column = 0; column < Width; column++)
            {
                GridArray[column, row] = new Cell((Height - row - 1) * Width + (Width - column - 1));
            }
        }
        cellOffset = grid.cellSize * .5f;

        ClearGrid();
        SpawnStones();
        SpawnBushes();
        SpawnEnemies();
    }

    private void ClearGrid()
    {
        foreach(var cell in GridArray)
            cell.StoredObject = null;
    }

    // TO-DO replace temp implementation
    private void SpawnEnemies()
    {
        var unoccupiedCells = GetListOfUnoccupiedCells();

        if(unoccupiedCells.Count() < EnemyCount)
            throw new System.NotImplementedException();

        for(int i = 0; i < EnemyCount; i++)
        {
            var randomIndex = (int)(Random.value * unoccupiedCells.Count());
            var randomCell = unoccupiedCells[randomIndex];

            var newEnemy = Instantiate(EnemyPrefab, GetWorldPosFromCellPos(randomCell.Item2.x, randomCell.Item2.y) + cellOffset, Quaternion.identity);
            randomCell.Item1.StoredObject = newEnemy;
            newEnemy.transform.SetParent(transform);

            unoccupiedCells.Remove(randomCell);
        }
    }

    private void SpawnStones()
    {
        for(int row = 1; row < Height; row += 2) // Ставить камни на четных рядах
        {
            for(int column = 1; column < Width; column += 2) // Ставить камни на четных колоннах
            {
                var newStone = Instantiate(StonePrefab, GetWorldPosFromCellPos(column, row) + cellOffset, Quaternion.identity);
                GridArray[column, row].StoredObject = newStone;
                newStone.transform.SetParent(transform);
            }
        }
    }

    // TO-DO replace temp implementation
    private void SpawnBushes()
    {
        var unoccupiedCells = GetListOfUnoccupiedCells();

        if(unoccupiedCells.Count() < BushCount)
            throw new System.NotImplementedException();

        for(int i = 0; i < BushCount; i++)
        {
            var randomIndex = (int)(Random.value * unoccupiedCells.Count());
            var randomCell = unoccupiedCells[randomIndex];

            var newBush = Instantiate(BushPrefab, GetWorldPosFromCellPos(randomCell.Item2.x, randomCell.Item2.y) + cellOffset, Quaternion.identity);
            randomCell.Item1.StoredObject = newBush;
            newBush.transform.SetParent(transform);

            unoccupiedCells.Remove(randomCell);
        }
    }

    // Возвращает клетку и её позицию
    private List<(Cell, Vector2Int)> GetListOfUnoccupiedCells()
    {
        var resultList = new List<(Cell, Vector2Int)>();
        for(var row = 0; row < Height; row++)
        {
            for(var column = 0; column < Width; column++)
            {
                var currCell = GridArray[column, row];
                if(!currCell.IsOccupied)
                {
                    resultList.Add((currCell, new Vector2Int(column, row)));
                }
            }
        }
        return resultList;
    }

    private Vector3 GetWorldPosFromCellPos(int column, int row) => grid.CellToWorld(new Vector3Int(column, row, 0));
    private Vector3 GetWorldPosFromCellPos(Vector2Int cellPos) => GetWorldPosFromCellPos(cellPos.x, cellPos.y);
}
