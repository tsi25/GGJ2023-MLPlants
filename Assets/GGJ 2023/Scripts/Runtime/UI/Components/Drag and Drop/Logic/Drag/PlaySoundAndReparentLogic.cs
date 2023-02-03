using UnityEngine;

namespace GGJRuntime
{
    [CreateAssetMenu(fileName="Logic - Play Sound And Reparent", menuName="GGJ/UI/Drag ang Drop/Logic/Drag/Play Sound And Reparent")]
    public class PlaySoundAndReparentLogic : DragLogic
    {
        public SoundId sound = SoundId.None;

        public override void DoLogic(Draggable draggable)
        {
            if(sound != SoundId.None) SoundManager.Play(sound);

            draggable.ResetToDefaultParent();
        }
    }
}