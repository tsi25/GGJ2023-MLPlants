using UnityEngine;
using UnityEngine.UI;

namespace GGJRuntime
{
    [System.Serializable]
    public class ViewNavigationData
    {
        public Button button = null;
        public GameViewId id = GameViewId.None;
        public bool keepOpen = false;

        private GameView view = null;

        public void Initialize(GameView parentView)
        {
            Debug.Log(parentView.name);
            view = parentView;
            button.onClick.AddListener(OnButtonClicked);
        }


        public void Cleanup()
        {
            view = null;
            button.onClick.RemoveListener(OnButtonClicked);
        }


        private void OnButtonClicked()
        {
            Debug.Log("Test");
            UIManager.GetView<GameView>(id).OpenAsync();

            if(!keepOpen) view.CloseAsync();
        }
    }
}