using System;
using System.Collections.Generic;
using UnityEngine;

namespace Graph
{
    [Serializable]
    [StorableType(typeof(LibNoise.QualityMode))]
    public class QualityModeVariable : VariableStorage<LibNoise.QualityMode> { }

    public partial class GraphVariableStorage
    {
        [SerializeField] public List<QualityModeVariable> QualityModes = new List<QualityModeVariable>();
    }
}