using UnityEngine;
using System.Collections.Generic;

namespace GGJRuntime
{
    public class CreditsView : GameView
    {
        [SerializeField]
        private CreditsDataCollection credits = null;
        [SerializeField]
        private RectTransform contentParent = null;
        [SerializeField]
        private CreditContent contentPrefab = null;

        private List<CreditContent> contentLabels = new List<CreditContent>();

        public override void Open()
        {
            if(IsOpen) return;

            base.Open();

            GenerateCredits();
        }


        public override void Close()
        {
            if(!IsOpen) return;

            CleanupCredits();

            base.Close();
        }


        private void GenerateCredits()
        {
            CleanupCredits();

            for(int i=0; i < credits.credits.Length; i++)
            {
                CreditContent content = Instantiate(contentPrefab, contentParent);

                content.Initialize(credits.credits[i]);

                contentLabels.Add(content);
            }
        }


        private void CleanupCredits()
        {
            contentLabels.ForEach(c => Destroy(c.gameObject));

            contentLabels.Clear();
        }
    }
}