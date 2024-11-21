using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public GameObject _checkpoints;
    private CheckConsole _console;
    private int _totalCheckpoints;
    private int _currentCheckpoint = 0;
    // Start is called before the first frame update
    void Awake()
    {
        _totalCheckpoints = _checkpoints.transform.childCount;
        _console = GetComponentInChildren<CheckConsole>();
    }
    
    public void updateCheckpoint( int index )
    {
        _currentCheckpoint = index;
        _console.UpdateConsole(index, _totalCheckpoints);
    }
}
