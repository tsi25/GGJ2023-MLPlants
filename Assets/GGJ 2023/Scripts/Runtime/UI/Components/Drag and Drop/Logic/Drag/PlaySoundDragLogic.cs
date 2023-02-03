using UnityEngine;

namespace GGJRuntime
{
    [CreateAssetMenu(fileName="Logic - Play Sound", menuName="GGJ/UI/Drag ang Drop/Logic/Drag/Play Sound")]
    public class PlaySoundDragLogic : DragLogic
    {
        public SoundId sound = SoundId.None;

        public override void DoLogic(Draggable dragTarget, Droppable dropArea)
        {
            if(sound != SoundId.None) SoundManager.Play(sound);
        }
    }
}