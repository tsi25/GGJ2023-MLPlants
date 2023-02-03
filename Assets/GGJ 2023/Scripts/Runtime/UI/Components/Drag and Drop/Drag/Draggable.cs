using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace GGJRuntime
{
    public class Draggable : RectMonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField, Tooltip("Initialize draggable in OnEnable();")]
        private bool initOnEnable = true;
        [SerializeField, Tooltip("Valid drop area flags. Invalid flags will be rejected from the drop.")]
        protected DragGroupFlags flags = DragGroupFlags.None;
        [SerializeField, Tooltip("[Optional] Logic sequentially executed when the draggable is dropped on an invalid droppable.")]
        protected DragLogic[] invalidLogic = new DragLogic[0];
        [SerializeField, Tooltip("[Optional] Logic sequentially executed when the draggable is dropped on an invalid droppable.")]
        protected DragLogic[] validLogic = new DragLogic[0];
        [SerializeField, Tooltip("[Optional] Logic sequentially executed when the draggable is not dropped on a droppable.")]
        protected DragLogic[] dropLogic = new DragLogic[0];
        [SerializeField]
        private CanvasGroup canvasGroup = null;

        public DragGroupFlags Flags => flags;

        public bool IsDragging { get; protected set; }

        public Transform DefaultParent { get; set; }

        private RectTransform DragParent { get; set; }

        private bool CanDoDropLogic { get; set; }

        public static Draggable CurrentDraggable { get; protected set; }

        public void Initialize(RectTransform dragParent)
        {
            DragParent = dragParent;
            DefaultParent = rectTransform.parent;
        }


        public void DoInvalidDrop(Droppable dropArea)
        {
            CanDoDropLogic = false;

            for(int i=0; i < invalidLogic.Length; i++)
            {
                invalidLogic[i].DoLogic(this, dropArea);
            }
        }


        public void DoValidDrop(Droppable dropArea)
        {
            CanDoDropLogic = false;
            
            for(int i=0; i < validLogic.Length; i++)
            {
                validLogic[i].DoLogic(this, dropArea);
            }
        }


        public void ResetToDefaultParent()
        {
            rectTransform.SetParent(DefaultParent);

            rectTransform.anchoredPosition3D = Vector3.zero;
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            IsDragging = true;
            CanDoDropLogic = true;

            if(DragParent != null) rectTransform.SetParent(DragParent);

            CurrentDraggable = this;

            canvasGroup.blocksRaycasts = false;
        }


        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta;
        }


        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("End");

            IsDragging = false;

            if(CurrentDraggable == this) CurrentDraggable = null;

            canvasGroup.blocksRaycasts = true;

            if(CanDoDropLogic)
            {
                for(int i=0; i < dropLogic.Length; i++)
                {
                    dropLogic[i].DoLogic(this, null);
                }
            }
        }


        protected virtual void OnEnable()
        {
            if(initOnEnable) Initialize(null);
        }
    }
}