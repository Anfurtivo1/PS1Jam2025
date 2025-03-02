using UnityEngine;

public class PSXFramerateLimiter : MonoBehaviour
{
    public int targetFPS = 15;
    private float frameTime;
    private float lastFrameTime;

    void Start()
    {
        frameTime = 1f / targetFPS;
    }

    void Update()
    {
        if (Time.time - lastFrameTime < frameTime) return;
        lastFrameTime = Time.time;
        Time.captureFramerate = targetFPS;
    }
}
