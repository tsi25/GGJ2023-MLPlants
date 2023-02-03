using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace GGJRuntime
{
    public class Droppable : RectMonoBehaviour, IDropHandler
    {
        [SerializeField, Tooltip("Valid draggable objects flags. Invalid flags will be rejected from the drop.")]
        protected DragGroupFlags flags = DragGroupFlags.None;
        [SerializeField, Tooltip("Logic used to evaluate drop operations.")]
        protected EvaluateDropLogic dropLogic = null;

        public DragGroupFlags Flags => flags;

        public void OnDrop(PointerEventData eventData)
        {
            if(eventData.pointerDrag == null) return;
            if(Draggable.CurrentDraggable == null) return;

            if(dropLogic.DoLogic(Draggable.CurrentDraggable, this))
            {
                //Valid
                Debug.Log("Valid");
                Draggable.CurrentDraggable.DoValidDrop();
            }
            else
            {
                //Invalid
                Debug.Log("Invalid");
                Draggable.CurrentDraggable.DoInvalidDrop();
            }
        }
    }
}