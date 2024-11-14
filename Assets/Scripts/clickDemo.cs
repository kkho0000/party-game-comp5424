using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;


public class clickDemo : MonoBehaviour
{
    private GameManager gameManager;
    private PersistentObject persistentObject;
    
    private void Start()
    {
        gameManager=FindObjectOfType<GameManager>();
        persistentObject = FindObjectOfType<PersistentObject>();

    }
    private void Update()
    {
       

    }
    
}
