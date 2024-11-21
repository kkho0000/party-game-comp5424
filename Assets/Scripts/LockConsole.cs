using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class LockConsole : MonoBehaviour
{
    private SpriteRenderer _lockset;
    public Sprite _locked;
    public Sprite _unlocked;

    void Awake() {
        _lockset = GetComponentInChildren<SpriteRenderer>();
    }

    public void SetLocked(bool locked)
    {
        if (locked) _lockset.sprite = _locked;
        else _lockset.sprite = _unlocked;
    }
}
