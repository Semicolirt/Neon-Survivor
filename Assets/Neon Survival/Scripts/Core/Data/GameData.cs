using System;

[Serializable]
public class GameData
{
    public int highScore;
    public int gold;

    public GameData()
    {
        highScore = 0;
        gold = 0;
    }
}
