using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GGJRuntime
{
    public class GameView : RectMonoBehaviour
    {
        [SerializeField]
        protected GameViewId id = GameViewId.None;
        [SerializeField, Tooltip("Root GameObject controlled by the modal.")]
        protected GameObject root = null;
        [SerializeField, Tooltip("Order for the view. Higher orders draw over lower orders.")]
        protected int order = 0;
        [SerializeField, Tooltip("[Optional] Background to show when this view opens.")]
        protected BackgroundData background = null;
        [SerializeField, Tooltip("Button navigations for the view.")]
        protected ViewNavigationData[] navigation = new ViewNavigationData[0];

        public GameViewId ID
        {
            get { return id; }
        }

        public GameObject Root
        {
            get { return root != null ? root : (root = this.gameObject); }
        }


        public bool IsOpen
        {
            get { return root.activeSelf; }
        }


        public int Order
        {
            get { return order; }
        }


        public virtual void Open()
        {
            if(IsOpen) return;

            Root.SetActive(true);

            if(background != null)
            {
                BackgroundView bgView = UIManager.GetView<BackgroundView>(GameViewId.Background);

                bgView.Open();
                bgView.ShowBackground(background);
            }
        }


        public virtual UniTask OpenAsync()
        {
            Open();

            return UniTask.CompletedTask;
        }


        public virtual void Close()
        {
            if(!IsOpen) return;

            Root.SetActive(false);
        }


        public virtual UniTask CloseAsync()
        {
            Close();

            return UniTask.CompletedTask;
        }


        protected virtual void Start()
        {
            for(int i=0; i < navigation.Length; i++)
            {
                navigation[i].Initialize(this);
            }
        }
    }
}