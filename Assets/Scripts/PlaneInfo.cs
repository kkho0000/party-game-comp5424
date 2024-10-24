    public class PlaneInfo
{
    public string playerName;
    public int collisionCount;
    public float timer;

    public PlaneInfo(string name, int count, float time)
    {
        playerName = name;
        collisionCount = count;
        timer = time;
    }
}