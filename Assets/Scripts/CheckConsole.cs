using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckConsole : MonoBehaviour
{
    private TMPro.TMP_Text _inputField;
    void Awake() {
        _inputField = GetComponentInChildren<TMPro.TMP_Text>();
    }
    
    public void UpdateConsole( int index, int total )
    {
        _inputField.text = "Checkpoint " + index + " / " + total;
    }
}
