using UnityEngine;
using System.Collections.Generic;

namespace GGJRuntime
{
    public class UIManager : RectMonoBehaviour
    {
        [SerializeField]
        private List<GameView> viewPrefabs = null;
        //[SerializeField]
        //private List<GameModal> modalPrefabs = null;

        private RectTransform viewsParent = null;
        //private RectTransform modalsParent = null;
        private List<GameView> views = new List<GameView>();
        //private List<GameModal> modals = new List<GameModal>();

        private static UIManager instance = null;

        public static UIManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = FindObjectOfType<UIManager>();

                    if(instance == null)
                    {
                        instance = Client.Get<UIManager>();
                    }
                }

                return instance;
            }
        }


        public static RectTransform ViewsParent
        {
            get
            {
                if(Instance.viewsParent == null) Instance.CreateViewParent();

                return Instance.viewsParent;
            }
        }


        public static T GetView<T>(GameViewId id, bool create=true) where T : GameView
        {
            GameView view = Instance.views.Find((a) => a.ID == id);

            if(view == null && create)
            {
                GameView prefab = Instance.viewPrefabs.Find((a) => a.ID == id);

                if(prefab == null) return null;

                view = Instantiate(prefab, ViewsParent);

                Instance.views.Add(view);

                Instance.SortViews();
            }

            return (T)view;
        }


        public void SortViews()
        {
            views.Sort((a, b) => a.Order.CompareTo(b.Order));

            views.ForEach(v => v.rectTransform.SetAsLastSibling());
        }


        private void CreateViewParent()
        {
            GameObject canvasGO = new GameObject("Views");
            RectTransform parentTransform = canvasGO.AddComponent<RectTransform>();

            parentTransform.SetParent(Instance.transform);
            parentTransform.SetAsFirstSibling();

            parentTransform.anchorMin = Vector2.zero;
            parentTransform.anchorMax = Vector2.one;
            parentTransform.sizeDelta = Vector2.zero;
            parentTransform.anchoredPosition3D = Vector3.zero;
            parentTransform.localScale = Vector3.one;

            canvasGO.layer = parentTransform.parent.gameObject.layer;

            //canvasGO.AddComponent<Canvas>();
            canvasGO.AddComponent<CanvasGroup>();

            Instance.viewsParent = parentTransform;
        }
    }
}