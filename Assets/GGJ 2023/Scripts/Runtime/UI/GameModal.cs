using UnityEngine;

namespace GGJRuntime
{
    public class GameModal : RectMonoBehaviour
    {
        [SerializeField]
        protected GameModalId id = GameModalId.None;
        [SerializeField, Tooltip("Root GameObject controlled by the modal.")]
        protected GameObject root = null;
        [SerializeField, Tooltip("Order for the modal. Higher orders draw over lower orders.")]
        protected int order = 0;

        public GameModalId ID
        {
            get { return id; }
        }

        public GameObject Root
        {
            get { return root != null ? root : (root = this.gameObject); }
        }


        public int Order
        {
            get { return order; }
        }
    }
}