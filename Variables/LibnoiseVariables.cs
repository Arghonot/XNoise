using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomGraph
{
    [Serializable]
    [StorableType(typeof(LibNoise.QualityMode))]
    public class QualityModeVariable : VariableStorage<LibNoise.QualityMode> { }

    public partial class GraphVariables
    {
        [SerializeField] public List<QualityModeVariable> QualityModes = new List<QualityModeVariable>();
    }
}