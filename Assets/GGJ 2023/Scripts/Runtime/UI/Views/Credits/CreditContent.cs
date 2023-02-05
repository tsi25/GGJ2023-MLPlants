using UnityEngine;
using TMPro;

namespace GGJRuntime
{
    public class CreditContent : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text roleLabel = null;
        [SerializeField]
        private TMP_Text nameLabel = null;
        [SerializeField]
        private TMP_Text socialsLabel = null;


        public void Initialize(CreditsData data)
        {
            roleLabel.text = data.role;
            nameLabel.text = data.name;
            socialsLabel.text = data.socials;
        }
    }
}