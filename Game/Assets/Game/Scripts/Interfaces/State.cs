using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status
{
    PLAYERWIN, AIWIN, PLAYING
}

public abstract class State : ICloneable
{
    public Turn CurrentTurn { get; set; }
    public abstract Status GetStatus();
    public abstract int GetHeuristics(Turn turn);
    public abstract object Clone();
}
