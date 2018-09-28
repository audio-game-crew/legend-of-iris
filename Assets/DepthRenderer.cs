using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class DepthRenderer : MonoBehaviour {

    public Shader depthRenderer;

    public float start;
    public float end;

    public bool toggle;

    private bool activated;

    void LateUpdate()
    {
        if (activated)
        {
            Shader.SetGlobalFloat("_DepthStart", start);
            Shader.SetGlobalFloat("_DepthEnd", end);
        }
        if (toggle)
        {
            toggle = false;
            SetDepthRenderer(!activated, start, end);
        }
    }

    public void SetDepthRenderer(bool active, float start, float end)
    {
        this.start = start;
        this.end = end;
        if (active && !activated)
        {
            activated = true;
            GetComponent<Camera>().SetReplacementShader(depthRenderer, "");
        }
        else if (!active && activated)
        {
            activated = false;
            GetComponent<Camera>().ResetReplacementShader();
        }
    }

}
