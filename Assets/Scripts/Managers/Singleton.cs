using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Michael
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindFirstObjectByType<T>();
                    if (instance == null)
                    {
                        GameObject singletonObject = new GameObject(typeof(T).Name);
                        instance = singletonObject.AddComponent<T>();
                    }
                }
                return instance;
            }
        }
        [SerializeField] bool keepAliveOnLoad = true;
        public virtual void Awake()
        {
            if(instance)
            {// don't destroy object as some developers like to hard reference singletons in scenes
                gameObject.SetActive(false);
                return;
            }

            instance = this as T;
            if (!keepAliveOnLoad) return;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
    }
}
