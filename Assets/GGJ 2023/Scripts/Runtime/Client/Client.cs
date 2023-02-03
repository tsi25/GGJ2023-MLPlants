using UnityEngine;
using System.Collections.Generic;

namespace GGJRuntime
{
    public class Client : MonoBehaviour
    {
        private Dictionary<System.Type, Component> singletons = new Dictionary<System.Type, Component>();

        private static Client instance = null;

        public static Client Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = FindObjectOfType<Client>();

                    if(instance == null)
                    {
                        instance = Instantiate((Client)Resources.Load("_Client"));
                    }

                    DontDestroyOnLoad(instance);
                }

                return instance;
            }
        }


        public static T Get<T>() where T : Component
        {
            return Instance.Get_Internal<T>();
        }


        private T Get_Internal<T>() where T : Component
        {
            if(!singletons.TryGetValue(typeof(T), out Component singleton))
            {
                singleton = GetComponentInChildren<T>(true);
            }

            return (T)singleton;
        }


        private void Awake()
        {
            if(instance == null) instance = this;
            else if(instance != this)
            {
                Debug.LogWarning("Disabling duplicate Client in the scene!");
                this.gameObject.SetActive(false);
            }
        }
    }
}