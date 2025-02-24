using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Solver
{
    public List<Operator> Operators { get; set; }
    protected Solver(OperatorGenerator operatorGenerator)
    {
        Operators = operatorGenerator.Operators.ToList();
    }
    public abstract State NextMove(State state);
}
