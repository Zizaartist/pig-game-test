using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : Block
{
    public override void Collision(IWorldObject newObj) 
    {
        if(newObj.GetType() == typeof(Explosion))
        {
            Remove();
        }
    }
}
