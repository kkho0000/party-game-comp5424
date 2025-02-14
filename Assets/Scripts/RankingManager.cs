using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Linq;

public class RankingManager : MonoBehaviour
{
    public List<CollisionCounter> planes = new List<CollisionCounter>();
    public string rankingText;
    private List<PlaneInfo> planeInfos = new List<PlaneInfo>();
    public RankingUIManager rankingUIManager;

    private ulong localClientId; // 存储本地客户端ID

    public void SetLocalClientId(ulong clientId)
    {
        localClientId = clientId; // 设置本地客户端ID
    }
    public List<PlaneInfo> getPlanInfos()
    {
        return planeInfos; 
    }

    private void Start()
    {
        planes.AddRange(FindObjectsOfType<CollisionCounter>());
        Debug.Log("游戏开始时执行Planes Count: " + planes.Count); // 添加这一行
        UpdateRanking();
    }

    void UpdateRanking()
    {
        planeInfos.Clear();

        foreach (var plane in planes)
        {
            PlaneInfo info = plane.GetPlaneInfo();
            planeInfos.Add(info);

        }

        Debug.Log("Plane Infos Count: " + planeInfos.Count); // 添加这一行
        // Sort the planeInfos list based on collisionCount and timer
        planeInfos.Sort(new RankingComparator());

        PlaneInfo playerInfo = planeInfos.FirstOrDefault(p => p.clientId == localClientId); // 使用clientId查找

        int playerRank = playerInfo != null ? planeInfos.IndexOf(playerInfo) + 1 : -1;

        rankingText = "Top";
        // Display the ranking
        /*for (int i = 0; i < planeInfos.Count; i++)
        {
            rankingText += (i + 1) + ":" + planeInfos[i].playerName + "\n";
        }*/
        /*planeInfos.Sort(new RankingComparator());*/

        // 查找玩家的排名
        //int playerRank = planeInfos.IndexOf(playerPlaneInfo) + 1; // 获取玩家的排名
        // 根据玩家的排名生成文本
        if (playerRank > 0)
        {
            rankingText = $"{playerRank} ";
        }
        else
        {
            rankingText = "2"; // 如果落后则为2
        }
        rankingUIManager.UpdateRankingText(rankingText);
    }

        public void UpdateRankingAfterCollision()
    {
        Debug.Log("Updating ranking after collision..."); 
        UpdateRanking();
    }

}

public class RankingComparator : IComparer<PlaneInfo>
{
    public int Compare(PlaneInfo x, PlaneInfo y)
    {
        // 根据需要定义比较逻辑，这里以碰撞次数为首要排序条件，如果碰撞次数相同，则比较计时器
        if (x.collisionCount != y.collisionCount)
        {
            return y.collisionCount.CompareTo(x.collisionCount); // 按照碰撞次数从高到低排序
        }
        else
        {
            return x.timer.CompareTo(y.timer); // 如果碰撞次数相同，则按照计时器从低到高排序
        }
    }
}