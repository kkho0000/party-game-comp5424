using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedConsole : MonoBehaviour
{
    private TMPro.TMP_Text _inputField;
    int cntFrame;
    int totDis;
    void Awake() {
        _inputField = GetComponentInChildren<TMPro.TMP_Text>();
        cntFrame = 0;
    }
    
    public void UpdateConsole( int speed )
    {
        cntFrame ++;
        totDis += speed;
        if ( cntFrame >= 50 ) {
            _inputField.text = totDis/cntFrame + " m/s ";
            cntFrame = 0;
            totDis = 0;
        }
    }
}
