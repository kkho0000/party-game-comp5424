//using UnityEditor.PackageManager;
using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    
}
