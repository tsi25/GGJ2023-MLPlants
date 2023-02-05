using UnityEngine;

namespace GGJRuntime
{
    [CreateAssetMenu(fileName="Credits", menuName="GGJ/Credits")]
    public class CreditsDataCollection : ScriptableObject
    {
        public CreditsData[] credits = new CreditsData[0];
    }
}