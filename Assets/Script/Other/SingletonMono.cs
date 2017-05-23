using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                if (FindObjectsOfType(typeof(T)).Length > 1)
                {
                    return instance;
                }
                if (instance == null)
                {
                    GameObject singleton = new GameObject("(singleton)" + typeof(T).ToString());
                    instance = singleton.AddComponent<T>();
                }
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }
}