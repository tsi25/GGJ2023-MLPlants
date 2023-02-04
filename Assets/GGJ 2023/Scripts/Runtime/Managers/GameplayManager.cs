using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJRuntime
{
    public class GameplayManager : MonoBehaviour
    {
        private void Start()
        {
            UIManager.GetView<TitleView>(GameViewId.Title, true).Open();
        }
    }
}
