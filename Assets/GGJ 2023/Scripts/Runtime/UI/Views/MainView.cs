using UnityEngine;
using UnityEngine.UI;

namespace GGJRuntime
{
    public class MainView : GameView
    {
        [SerializeField]
        private Button quitButton = null;

        private void OnQuitClicked()
        {

        }


        protected override void Start()
        {
            base.Start();
        }
    }
}