using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Operator
{
    bool IsApplicable(State state);
    State Apply(State state);
}
