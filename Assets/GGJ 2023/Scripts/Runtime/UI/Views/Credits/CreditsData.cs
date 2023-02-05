using UnityEngine;

namespace GGJRuntime
{
    [System.Serializable]
    public class CreditsData
    {
        [Tooltip("Name of the person being credited.")]
        public string name = "";
        [Tooltip("Role during development.")]
        public string role = "";
        [TextArea(3, 5), Tooltip("[Optional] Links to social media or other contact info.")]
        public string socials = "";
    }
}