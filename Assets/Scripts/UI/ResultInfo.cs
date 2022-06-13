using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultInfo 
{
    public float time;
    public string timeText;
    public int gold;
    public int killedEnemies;
    public int reachedLevel;

    public ResultInfo(float _time, string _timeText, string _gold, string _killedEnemies, string _reachedlevel)
    {
        time = _time;
        timeText = _timeText;
        gold = int.Parse(_gold);
        killedEnemies = int.Parse(_killedEnemies);
        reachedLevel = int.Parse(_reachedlevel);
    }
}
