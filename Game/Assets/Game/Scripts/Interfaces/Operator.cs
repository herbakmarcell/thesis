using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Operator
{
    bool IsApplicable(StateRepresentation state);
    StateRepresentation Apply(StateRepresentation state);
}
