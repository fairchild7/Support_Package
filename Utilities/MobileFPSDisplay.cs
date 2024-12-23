using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileFPSDisplay : MonoBehaviour
{
    private int FramesPerSec;
    private float frequency = 1.0f;
    private string fps;

    private GUIStyle guiStyle = new GUIStyle();

    void Start()
    {
        StartCoroutine(FPS());
    }

    private IEnumerator FPS()
    {
        for (; ; )
        {
            // Capture frame-per-second
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(frequency);
            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;

            // Display it

            fps = string.Format("FPS: {0}", Mathf.RoundToInt(frameCount / timeSpan));
        }
    }

    void OnGUI()
    {
        guiStyle.fontSize = 40;
        GUI.Label(new Rect(Screen.width - 300, 10, 150, 20), fps, guiStyle);
    }
}
