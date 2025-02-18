using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldObject
{
    public string id;
    public Vector2 position;

    public FieldObject(string id, Vector2 position)
    {
        this.id = id;
        this.position = position;
    }
}
