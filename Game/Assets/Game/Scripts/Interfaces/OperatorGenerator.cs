using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public interface OperatorGenerator
{
    BlockingCollection<Operator> Operators { get; }
}
