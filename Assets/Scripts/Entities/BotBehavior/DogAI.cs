using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Любит ходить по прямым линиям
/// </summary>
public class DogAI : CreatureAI
{
    // Даем больше шансов на выбор того же направления что и в предыдущей итерации
    private const int sameDirWeight = 8;
    private const int diffDirWeight = 1;

    private Direction previousDecision = Direction.up;

    protected override void DecideNextMove()
    { 
        var vision = body.currentVision ?? new Dictionary<Direction, bool>();
        var oppositeDirToPrevious = GetOppositeDirection(previousDecision);
        var movementOptions = vision.Where(pair => pair.Value)
            .Select(pair => pair.Key)
            .Except(new Direction[]{ oppositeDirToPrevious });

        Direction decidedDirection;
        if(movementOptions?.Any() ?? false)
        {
            var totalWeight = 0;
            var directionWeights = movementOptions.ToList();
            
            var asd = directionWeights.Select(dir => 
            {
                if(dir == previousDecision)
                    totalWeight += sameDirWeight;
                else
                    totalWeight += diffDirWeight;
                return (dir, totalWeight);
            });
            var randomNum = (int) (Random.value * totalWeight);
            decidedDirection = asd.OrderBy(t => t.totalWeight)
                .FirstOrDefault(t => randomNum < t.totalWeight).dir;
        }
        else
        {
            decidedDirection = oppositeDirToPrevious;
        }

        previousDecision = decidedDirection;
        IntendedMovementDirection = decidedDirection;
    }

    private Direction GetOppositeDirection(Direction dir)
    {
        switch(dir)
        {
            case Direction.up: return Direction.down;
            case Direction.down: return Direction.up;
            case Direction.left: return Direction.right;
            case Direction.right: return Direction.left;
            default: return Direction.up;
        }
    }
}
