using UnityEngine;

namespace GGJRuntime
{
    [CreateAssetMenu(fileName="Logic - Reparent", menuName="GGJ/UI/Drag ang Drop/Logic/Drag/Reparent")]
    public class ReparentDragLogic : DragLogic
    {
        [Tooltip("If true, the dragged object's new parent will become its default parent.")]
        public bool setDefaultParent = false;

        public override void DoLogic(Draggable dragTarget, Droppable dropArea)
        {
            dragTarget.rectTransform.SetParent(dropArea.rectTransform);
            dragTarget.rectTransform.anchoredPosition3D = Vector3.zero;

            if(setDefaultParent) dragTarget.DefaultParent = dropArea.rectTransform;
        }
    }
}