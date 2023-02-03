using UnityEngine;

namespace GGJRuntime
{
    public class OpenView : MonoBehaviour
    {
        [SerializeField]
        private GameViewId id = GameViewId.None;

        private void Start()
        {
            UIManager.GetView<GameView>(id).OpenAsync();
        }
    }
}