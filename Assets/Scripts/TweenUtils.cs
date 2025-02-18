using UnityEngine;

public static class TweenUtils 
{
    public static float EaseIn(float t)
    {
        return t * t;
    }
    public static float EaseOut(float t)
    {
        return 1 - (1 - t) * (1 - t);
    }

    public static float NormalizeTime(float t, float maxTime)
    {
        return Mathf.Clamp01(t / maxTime);
    }

    public static float EaseInOut(float t)
    {
        return t < 0.5f ? EaseIn(t * 2f) / 2f : 0.5f + EaseOut((t - 0.5f) * 2f) / 2f;
    }
}