using UnityEngine;

namespace GGJRuntime
{
    [CreateAssetMenu(fileName="Logic - Reparent", menuName="GGJ/UI/Drag ang Drop/Logic/Drag/Reparent")]
    public class ReparentDragLogic : DragLogic
    {
        public override void DoLogic(Draggable dragTarget, Droppable dropArea)
        {
            dragTarget.rectTransform.SetParent(dropArea.rectTransform);
            dragTarget.rectTransform.anchoredPosition3D = Vector3.zero;

            dragTarget.DefaultParent = dropArea.rectTransform;
        }
    }
}