using UnityEngine;
using System.Collections.Generic;

namespace GGJRuntime
{
    public class BackgroundView : GameView
    {
        [SerializeField]
        private RectTransform backgroundParent = null;
        [SerializeField]
        private BackgroundImage bgImagePrefab = null;

        private List<BackgroundImage> bgImages = new List<BackgroundImage>();


        public override void Close()
        {
            CleanupBackgrounds();

            base.Close();
        }


        public void ShowBackground(IBackgroundData data)
        {
            if(bgImages.Count > 0 && bgImages[bgImages.Count - 1].Data == data) return; //Do nothing if same data.

            BackgroundImage bgImage = Instantiate(bgImagePrefab, backgroundParent);

            bgImage.Initialize(data);

            bgImages.Add(bgImage);

            //TODO: Fade out support
            if(bgImages.Count > 1)
            {
                for(int i=bgImages.Count-2; i >= 0; i--)
                {
                    Destroy(bgImages[i].gameObject);
                }

                bgImages.RemoveRange(0, bgImages.Count - 1);
            }
        }



        private void CleanupBackgrounds()
        {
            for(int i=0; i < bgImages.Count; i++)
            {
                Destroy(bgImages[i].gameObject);
            }

            bgImages.Clear();
        }
    }
}