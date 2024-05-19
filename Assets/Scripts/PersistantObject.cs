using UnityEngine;
using System.Collections.Generic;

public class PersistantObject : MonoBehaviour
{
    public static Dictionary<string, PersistantObject> persistantObjects = new Dictionary<string, PersistantObject>();

    private void Start()
    {
        if (persistantObjects.TryGetValue(gameObject.name, out _))
        {
            Destroy(gameObject);
        }
        else
        {
            persistantObjects.Add(gameObject.name, this);
            DontDestroyOnLoad(gameObject);
        }
    }
}
