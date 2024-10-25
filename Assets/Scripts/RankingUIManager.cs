using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class RankingUIManager : MonoBehaviour
{
    public TMP_Text rankingText; // 指向你的Text UI元素

    public List<PlaneInfo> planInfos; 

    public void UpdateRankingText(string text)
    {
        planInfos = GetComponent<RankingManager>().getPlanInfos();
        if (planInfos != null)
        {
            rankingText.text = text;

            /*Debug.LogError(planInfos.Count);
            for ( int i = 0; i < planInfos.Count; i++ )
            {
                rankingText.text += (i+1).ToString()+":";
                rankingText.text += planInfos[i].playerName;
                rankingText.text += "\n";
            }*/
        }
    }
}
