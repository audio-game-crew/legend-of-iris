using UnityEngine;
using System.Collections;

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
            string filename = "Screenshot (" + Screen.width * screenshotScale + " x " + Screen.height * screenshotScale + ") " + System.DateTime.Now.ToString("yyyy-MM-dd hh.mm.ss") + ".jpg";
            Application.CaptureScreenshot(folder + filename, screenshotScale);
            SubtitlesManager.ShowSubtitle(3f, "Iris", "Screenshot saved at \n\n" + System.IO.Path.GetFullPath(folder + filename));
        }
	}
}
