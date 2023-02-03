using UnityEngine;

namespace GGJRuntime
{
    public enum GameViewId
    {
        [InspectorName("<NONE>")]
        None    = 0,
        Title   = 1,
        Main    = 2,
        Settings    = 3,
        Game    = 4,
        Credits = 5,
        Background  = 6,
        Modal   = 100
    }
}