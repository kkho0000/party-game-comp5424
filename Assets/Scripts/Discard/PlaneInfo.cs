using System;

public class PlaneInfo
{
    public string playerName;
    public int collisionCount;
    public float timer;
    public ulong clientId;

    public PlaneInfo(string name, int count, float time, ulong id)
    {
        playerName = name;
        collisionCount = count;
        timer = time;
        clientId = id;
    }


}