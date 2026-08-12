using UnityEngine;
using System.Collections;

public class KeepOnLoad : MonoBehaviour
{
    void Awake()
    {
         
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        
    }
}
