using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class DepthRenderer : MonoBehaviour {

    public Shader depthCalculator;

    public float start;
    public float end;

    public bool toggle;

    private bool activated;

    void LateUpdate()
    {
        Shader.SetGlobalFloat("_DepthStart", start);
        Shader.SetGlobalFloat("_DepthEnd", end);
        if (toggle)
        {
            toggle = false;
            SetDepthRenderer(!activated, start, end);
        }
    }

    public void SetDepthRenderer(bool active, float start, float end)
    {
        if (active && !activated)
        {
            activated = true;
            Camera.main.SetReplacementShader(depthCalculator, "");
        }
        else if (!active && activated)
        {
            activated = false;
            Camera.main.ResetReplacementShader();
        }
    }

}
