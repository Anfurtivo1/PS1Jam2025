using UnityEngine;

public class PSXResolution : MonoBehaviour
{
    public int width = 320;
    public int height = 240;
    private RenderTexture renderTexture;

    void Start()
    {
        renderTexture = new RenderTexture(width, height, 16);
        renderTexture.filterMode = FilterMode.Point; // Nearest-neighbor scaling
        Camera cam = FindObjectOfType<Camera>();
        if (cam != null)
        {
            cam.targetTexture = renderTexture;
        }
        else
        {
            Debug.LogError("No camera found in the scene!");

        }

        void OnGUI()
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), renderTexture);
        }

    }
}
