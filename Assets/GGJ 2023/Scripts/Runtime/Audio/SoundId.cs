using UnityEngine;

namespace GGJRuntime
{
    public enum SoundId
    {
        [InspectorName("<NONE>")]
        None    = 0,
        [InspectorName("Music/Ambient")]
        Music_Ambient   = 50,
        [InspectorName("UI/Click")]
        UI_Click    = 100,
        [InspectorName("UI/Valid")]
        UI_Valid = 101,
        [InspectorName("UI/Invalid")]
        UI_Invalid = 102,
        [InspectorName("UI/Pick Up")]
        UI_PickUp = 105,
        [InspectorName("UI/Drop")]
        UI_Drop = 106,
        [InspectorName("Gameplay/Success")]
        Gameplay_Success = 200,
        [InspectorName("Gameplay/Fail")]
        Gameplay_Fail = 201,
    }
}