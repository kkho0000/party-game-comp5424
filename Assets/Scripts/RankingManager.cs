using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RankingManager : MonoBehaviour
{
    public List<CollisionCounter> planes = new List<CollisionCounter>();

    private List<PlaneInfo> planeInfos = new List<PlaneInfo>();

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

        // Display the ranking
        for (int i = 0; i < planeInfos.Count; i++)
        {
            Debug.Log("Rank " + (i + 1) + ": " + planeInfos[i].playerName);
        }
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