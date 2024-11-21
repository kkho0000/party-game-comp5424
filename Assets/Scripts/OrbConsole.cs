using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbConsole : MonoBehaviour
{

    public Sprite filledSprite; // 能量球满时的图片
    public Sprite emptySprite; // 能量球空时的图片
    public SpriteRenderer[] spriteRenderers; // 能量槽UI
    
    void Awake() {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i <= 3; i++)
        {
            spriteRenderers[i].sprite = emptySprite;
        }
    }

    // Update is called when the energy is updated
    public void UpdateEnergyUI(int currentEnergy)
    {
        for ( int i = 1; i <= 3; i ++ )
        {
            if ( i <= currentEnergy )
            {
                spriteRenderers[i].sprite = filledSprite;
            }
            else
            {
                spriteRenderers[i].sprite = emptySprite;
            }
        }
        // Debug.Log("能量收集: " + currentEnergy);
    }
}
