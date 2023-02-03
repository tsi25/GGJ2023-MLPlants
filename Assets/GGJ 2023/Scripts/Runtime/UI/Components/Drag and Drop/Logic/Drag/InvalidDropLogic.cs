using UnityEngine;

namespace GGJRuntime
{
    [CreateAssetMenu(fileName="Logic - Invalid Drop", menuName="GGJ/UI/Drag ang Drop/Logic/Invalid Drop")]
    public class InvalidDropLogic : DragLogic
    {
        public SoundId invalidSound = SoundId.None;

        public override void DoLogic(Draggable dragTarget, Droppable dropArea)
        {
            if(invalidSound != SoundId.None) SoundManager.Play(invalidSound);

            dragTarget.ResetToDefaultParent();
        }
    }
}