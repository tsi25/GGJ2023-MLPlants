using UnityEngine;
using System.Collections.Generic;

namespace GGJRuntime
{
    [CreateAssetMenu(fileName="Sound Collection", menuName="GGJ/Audio/Sound Collection")]
    public class SoundCollection : ScriptableObject
    {
        public List<Sound> sounds = new List<Sound>();


        public Sound Find(SoundId id)
        {
            for(int i=0; i < sounds.Count; i++)
            {
                if(sounds[i].id == id) return sounds[i];
            }

            return null;
        }


        public bool TryFind(SoundId id, out Sound sound)
        {
            for(int i = 0; i < sounds.Count; i++)
            {
                if(sounds[i].id == id)
                {
                    sound = sounds[i];
                    return true;
                }
            }

            sound = null;

            return false;
        }
    }
}