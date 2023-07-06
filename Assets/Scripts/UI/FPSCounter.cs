using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI fpsText;
    [SerializeField]
    TextMeshProUGUI msText;

    float pollingTime = 1f;
    float fps = 0;
    float time = 0;

    void Update()
    {
        time += Time.deltaTime;
        fps++;
        if (time >= pollingTime)
        {
            int frameRate=Mathf.RoundToInt(fps/time);
            fpsText.text = frameRate.ToString() + " fps";
            msText.text = Time.deltaTime.ToString("0.000")+" ms";
            time -= pollingTime;
            fps = 0;
        }

    }
}
