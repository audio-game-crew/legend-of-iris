using UnityEngine;
using System.Collections;
using System.IO;

public class Screenshotter : MonoBehaviour {

    [Range(1, 5)]
    public int screenshotScale = 1;
    public string folder = "./";
    public KeyCode button = KeyCode.F11;
    public bool ctrl;
    public bool alt;
    public bool cmd;
    public bool shift;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(button))
        {
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            string filename = "Legend of Iris (" + Screen.width * screenshotScale + " x " + Screen.height * screenshotScale + ") " + System.DateTime.Now.ToString("yyyy-MM-dd hh.mm.ss") + ".png";
            string path = Path.GetFullPath(folder + filename);
            ScreenCapture.CaptureScreenshot(path, screenshotScale);
            SubtitlesManager.ShowSubtitle(3f, "Iris", "Screenshot saved at \n\n" + path);
            Debug.Log("Screenshot saved at \n\n" + path);

            /* the below script doesnt work... why?
            if (File.Exists(path))
            {
                SubtitlesManager.ShowSubtitle(3f, "Iris", "Screenshot saved at \n\n" + path);
            }
            else
            {
                SubtitlesManager.ShowSubtitle(3f, "Iris", "ERROR: Taking screenshot failed");
            }*/
        }
	}
}
