using System;
using UnityEngine;

[Serializable]
public struct CourseTime
{
    public int minutes;
    public int seconds;
    public int milliseconds;

    public float GetCourseToFloat()
    {
        return minutes * 60f + seconds + milliseconds * 0.001f; 
    }

    public void SetCourseFromFloat(float timeInSeconds)
    {
        this.minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        float remainingSeconds = timeInSeconds - (this.minutes * 60f);

        this.seconds = Mathf.FloorToInt(remainingSeconds);
        this.milliseconds = Mathf.FloorToInt((remainingSeconds - this.seconds) * 1000f);
    }

    public override string ToString()
    {
        return "Min : " + minutes + " S : " + seconds + " Ms : " + milliseconds;
    }

    public string ChronoToString()
    {
        string chrono = null;
        if (minutes < 10)
        {
            chrono += "0" + minutes;
        } else
        {
            chrono += minutes;
        }

        if (seconds < 10)
        {
            chrono += ":0" + seconds;
        } else
        {
            chrono += ":" + seconds;
        }

        if (milliseconds <10)
        {
            chrono += ":00" + milliseconds;
        } else if (milliseconds < 100)
        {
            chrono += ":0" + milliseconds;
        } else
        {
            chrono += ":" + milliseconds;
        }

        return chrono;
    }
}
