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
        [SerializeField, Tooltip("Logic invoked when a valid drop occurs.")]
        protected DropLogic[] validDropLogic = new DropLogic[0];
        [SerializeField, Tooltip("Logic invoked when an invalid drop occurs.")]
        protected DropLogic[] invalidDropLogic = new DropLogic[0];

        public DragGroupFlags Flags => flags;

        public void OnDrop(PointerEventData eventData)
        {
            if(eventData.pointerDrag == null) return;
            if(Draggable.CurrentDraggable == null) return;

            Draggable currentDraggable = Draggable.CurrentDraggable;

            if(dropLogic.DoLogic(currentDraggable, this))
            {
                //Valid
                currentDraggable.DoValidDrop(this);

                for(int i=0; i < validDropLogic.Length; i++)
                {
                    validDropLogic[i].DoLogic(currentDraggable, this);
                }
            }
            else
            {
                //Invalid
                currentDraggable.DoInvalidDrop(this);

                for(int i=0; i < invalidDropLogic.Length; i++)
                {
                    invalidDropLogic[i].DoLogic(currentDraggable, this);
                }
            }
        }
    }
}