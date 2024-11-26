using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedConsole : MonoBehaviour
{
    private TMPro.TMP_Text _inputField;
    void Awake() {
        _inputField = GetComponentInChildren<TMPro.TMP_Text>();
    }
    
    public void UpdateConsole( int speed )
    {
        _inputField.text = speed + " m/s ";
    }
}
