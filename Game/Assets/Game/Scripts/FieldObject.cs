using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FieldObject : ICloneable
{
    public string id;
    public Vector2 position;

    public FieldObject(string id, Vector2 position)
    {
        this.id = id;
        this.position = position;
    }

    public virtual object Clone()
    {
        return new FieldObject(id, new Vector2(position.x, position.y));
    }
}
