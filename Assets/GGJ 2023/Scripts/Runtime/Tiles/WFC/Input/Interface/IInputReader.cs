using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJRuntime
{
    public interface IInputReader<T>
    {
        IValue<T>[][] ReadInputToGrid();
    }
}