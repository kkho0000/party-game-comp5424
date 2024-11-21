using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class OrbManager : MonoBehaviour
{
    private int currentEnergy = 0; // 当前能量数量
    private int maxEnergy = 3; // 最大能量数量
    private OrbConsole _orbUI; // 能量UI
    // Start is called before the first frame update

    void Awake() {
        _orbUI = GameObject.Find("CanvasConsole").GetComponent<OrbConsole>();
    }

    public int GetCurrentEnergy()
    {
        return currentEnergy;
    }
    public void CollectEnergyOrb()
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy ++;
            _orbUI.UpdateEnergyUI(currentEnergy);
        }
    }
    public void clearEnergyOrb()
    {
        currentEnergy = 0;
        _orbUI.UpdateEnergyUI(currentEnergy);
    }

    public void reduceEnergyOrb()
    {
        if (currentEnergy > 0) {
            currentEnergy --;
            _orbUI.UpdateEnergyUI(currentEnergy);
        }
    }
}