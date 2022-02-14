using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

/// <summary>
/// Предоставляет методы управления игровым пространством. 
/// Управляет "объектами мира", позволяет (объектам мира) изменять игровой мир только с помощью событий
/// </summary>
public class Grid : MonoBehaviour
{
    public Stone StonePrefab;
    public Bush BushPrefab;
    public Enemy EnemyPrefab;
    public Bomb BombPrefab;
    public Explosion ExplosionPrefab;
    public Player PlayerPrefab;
    public GridLayout grid;

    public UnityEvent<Player> FinishedInitialization;

    private Cell[,] GridArray;
    private Vector3 cellOffset;

    [Range(2, 100)]
    public int Width = 17;
    [Range(2, 100)]
    public int Height = 9;
    public int BushCount = 10;
    public int EnemyCount = 5;
    public Vector2Int PlayerSpawnPoint = new Vector2Int(0, 0);

    public void Initialize()
    {
        GridArray = new Cell[Width, Height];
        for(int row = 0; row < Height; row++)
        {
            for(int column = 0; column < Width; column++)
            {
                GridArray[column, row] = new Cell((Height - row - 1) * Width + (Width - column - 1), 
                    new Vector2Int(column, row));
            }
        }
        cellOffset = grid.cellSize * .5f;

        ClearGrid();
        SpawnStones();
        var newPlayer = SpawnPlayer();
        SpawnBushes();
        SpawnEnemies();

        FinishedInitialization.Invoke(newPlayer);
    }

    private void ClearGrid()
    {
        foreach(var cell in GridArray)
            cell.Clear();
    }

    private Player SpawnPlayer()
    {
        var newPlayer = Instantiate(PlayerPrefab, GetWorldPosFromCellPos(PlayerSpawnPoint) + cellOffset, Quaternion.identity);
        GridArray[PlayerSpawnPoint.x, PlayerSpawnPoint.y].Add(newPlayer);
        newPlayer.MoveEvent.AddListener((dir) => MoveCreature(dir, newPlayer));
        UpdateCreatureVision(newPlayer);
        return newPlayer;
    }

    // TO-DO replace temp implementation
    // TO-DO spawn in safe range from player
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
            randomCell.Item1.Add(newEnemy);
            newEnemy.transform.SetParent(transform);
            newEnemy.MoveEvent.AddListener(dir => MoveCreature(dir, newEnemy));
            UpdateCreatureVision(newEnemy);

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
                GridArray[column, row].Add(newStone);
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
            randomCell.Item1.Add(newBush);
            newBush.transform.SetParent(transform);

            unoccupiedCells.Remove(randomCell);
        }
    }

    public void SpawnBombUnderPlayer(Player player)
    {
        var playerPos = player.cell.Position;
        var newBomb = Instantiate(BombPrefab, GetWorldPosFromCellPos(playerPos) + cellOffset, Quaternion.identity);
        if(GridArray[playerPos.x, playerPos.y].Add(newBomb))
        {
            newBomb.Exploded.AddListener(Explosion);
        }
        else
        {
            newBomb.Remove();
        }
    }

    private void Explosion(Vector2Int position)
    {
        var potentialPositions = GetOccupationAround(position);
        foreach(var explPos in potentialPositions.Where(pos => !pos.Value).Select(pos => pos.Key))
        {
            var newExplosionPart = Instantiate(ExplosionPrefab, GetWorldPosFromCellPos(explPos) + cellOffset, Quaternion.identity);
            GridArray[explPos.x, explPos.y].Add(newExplosionPart);
        }
    }

    private void MoveCreature(Direction dir, Creature creature)
    {
        var currentPos = creature.cell.Position;
        var moveVector = DirectionToVector(dir);
        var newPos = currentPos + moveVector;
        if(IsWithinBoundaries(newPos) && !GridArray[newPos.x, newPos.y].IsOccupied)
        {
            GridArray[currentPos.x, currentPos.y].Remove(creature);
            GridArray[newPos.x, newPos.y].Add(creature);
            var worldDestinationPos = GetWorldPosFromCellPos(newPos) + cellOffset;
            UpdateCreatureVision(creature);
            creature.AnimateMovement(worldDestinationPos);
        }
    }

    private void UpdateCreatureVision(Creature creature)
    {
        var creatureVision = new Dictionary<Direction, bool>();

        var upCoords = creature.cell.Position + DirectionToVector(Direction.up);
        creatureVision.Add(Direction.up, (IsWithinBoundaries(upCoords) && !GridArray[upCoords.x, upCoords.y].IsOccupied));
        var downCoords = creature.cell.Position + DirectionToVector(Direction.down);
        creatureVision.Add(Direction.down, (IsWithinBoundaries(downCoords) && !GridArray[downCoords.x, downCoords.y].IsOccupied));
        var leftCoords = creature.cell.Position + DirectionToVector(Direction.left);
        creatureVision.Add(Direction.left, (IsWithinBoundaries(leftCoords) && !GridArray[leftCoords.x, leftCoords.y].IsOccupied));
        var rightCoords = creature.cell.Position + DirectionToVector(Direction.right);
        creatureVision.Add(Direction.right, (IsWithinBoundaries(rightCoords) && !GridArray[rightCoords.x, rightCoords.y].IsOccupied));

        creature.currentVision = creatureVision;
    }

    /// <summary>
    /// Получаем занятость соседних клеток
    /// </summary>
    public Dictionary<Vector2Int, bool> GetOccupationAround(Vector2Int centerPos)
    {
        var result = new Dictionary<Vector2Int, bool>();

        var up = new Vector2Int(centerPos.x, centerPos.y + 1);
        var isUpOccupied = !IsWithinBoundaries(up) || GridArray[up.x, up.y].IsOccupied;
        result.Add(up, isUpOccupied);
        
        var down = new Vector2Int(centerPos.x, centerPos.y - 1);
        var isDownOccupied = !IsWithinBoundaries(down) || GridArray[down.x, down.y].IsOccupied;
        result.Add(down, isDownOccupied);
        
        var left = new Vector2Int(centerPos.x - 1, centerPos.y);
        var isLeftOccupied = !IsWithinBoundaries(left) || GridArray[left.x, left.y].IsOccupied;
        result.Add(left, isLeftOccupied);
        
        var right = new Vector2Int(centerPos.x + 1, centerPos.y);
        var isRightOccupied = !IsWithinBoundaries(right) || GridArray[right.x, right.y].IsOccupied;
        result.Add(right, isRightOccupied);

        return result;
    }

    /// <summary>
    /// Находится ли координата внутри границ grid-а
    /// </summary>
    public bool IsWithinBoundaries(Vector2Int position) => (position.x >= 0 && position.x < Width) && (position.y >= 0 && position.y < Height);

    #region reusable code

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

    private Vector2Int DirectionToVector(Direction dir)
    {
        switch (dir)
        {
            case Direction.up: return new Vector2Int(0, 1);
            case Direction.down: return new Vector2Int(0, -1);
            case Direction.left: return new Vector2Int(-1, 0);
            case Direction.right: return new Vector2Int(1, 0);
            default: return Vector2Int.zero;
        };
    }

    #endregion reusable code
}
