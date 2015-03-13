using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class DepthRenderer : MonoBehaviour {

    public Material blackMaterial;

    public float start;
    public float end;

    public bool toggle;

    void LateUpdate()
    {
        if (toggle)
        {
            toggle = false;
            SetDepthRenderer(!activated, start, end);
        } 
        else if (activated)
        {
            ensureActivated();
        } 
    }

    // ------------------- ACTUAL FUNCTIONALITY

    private bool activated;

    private Dictionary<GameObject, Material[]> materials;

    private float _start;
    private float _end;

    private bool fog;
    private FogMode fogMode;
    private float fogDensity;
    private float fogStartDistance;
    private float fogEndDistance;
    private Color fogColor;
    private Color backgroundColor;
    private CameraClearFlags clearFlags;

    public void SetDepthRenderer(bool active, float start, float end)
    {
        if (active && !activated)
        {
            activated = true;
            _start = start;
            _end = end;

            rememberFogSettings();
            materials = new Dictionary<GameObject, Material[]>();
        }
        else if (!active && activated)
        {
            activated = false;
            revertScene();
        }
    }

    void rememberFogSettings()
    {
        fog = RenderSettings.fog;
        fogDensity = RenderSettings.fogDensity;
        fogMode = RenderSettings.fogMode;
        fogStartDistance = RenderSettings.fogStartDistance;
        fogEndDistance = RenderSettings.fogEndDistance;
        fogColor = RenderSettings.fogColor;
        backgroundColor = Camera.main.backgroundColor;
        clearFlags = Camera.main.clearFlags;
    }

    void revertScene()
    {
        revertRenderers();
        revertTerrains();

        RenderSettings.fog = fog;
        RenderSettings.fogDensity = fogDensity;
        RenderSettings.fogMode = fogMode;
        RenderSettings.fogStartDistance = fogStartDistance;
        RenderSettings.fogEndDistance = fogEndDistance;
        RenderSettings.fogColor = fogColor;
        Camera.main.backgroundColor = backgroundColor;
        Camera.main.clearFlags = clearFlags;
    }

    void revertTerrains()
    {
        Terrain[] ts = Object.FindObjectsOfType<Terrain>();
        foreach (var t in ts)
        {
            if (!materials.ContainsKey(t.gameObject)) continue;
            t.materialTemplate = materials[t.gameObject][0];
        }
    }

    void revertRenderers()
    {
        Renderer[] rs = Object.FindObjectsOfType<Renderer>();
        foreach (var r in rs)
        {
            if (!materials.ContainsKey(r.gameObject)) continue;

            r.material = materials[r.gameObject][0];
            for (int i = 0; i < r.sharedMaterials.Length; i++)
            {
                r.materials[i] = materials[r.gameObject][i];
            }
        }
    }

    void ensureActivated()
    {
        ensureRenderers();
        ensureTerrains();

        RenderSettings.fog = true;
        RenderSettings.fogDensity = 0;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogStartDistance = _start;
        RenderSettings.fogEndDistance = _end;
        RenderSettings.fogColor = Color.white;
        Camera.main.backgroundColor = Color.white;
        Camera.main.clearFlags = CameraClearFlags.Skybox;
    }

    void ensureTerrains()
    {
        Terrain[] ts = Object.FindObjectsOfType<Terrain>();
        foreach (var t in ts)
        {
            // remember new objects
            if (!materials.ContainsKey(t.gameObject))
            {
                materials.Add(t.gameObject, new Material[1]{ t.materialTemplate });
            }

            // ensure everything is black
            if (t.materialTemplate != blackMaterial)
            {
                t.materialTemplate = blackMaterial;
            }
        }
    }

    void ensureRenderers()
    {
        Renderer[] rs = Object.FindObjectsOfType<Renderer>();
        foreach (var r in rs)
        {
            // remember new objects
            if (!materials.ContainsKey(r.gameObject))
            {
                Material[] copy = new Material[r.sharedMaterials.Length];
                for (int i = 0; i < r.sharedMaterials.Length; i++)
                {
                    copy[i] = r.sharedMaterials[i];
                }
                materials.Add(r.gameObject, copy);
            }

            // ensure everything is black
            if (r.material != blackMaterial)
            {
                r.material = blackMaterial;
                r.sharedMaterial = blackMaterial;
                for (int i = 0; i < r.sharedMaterials.Length; i++)
                {
                    r.sharedMaterials[i] = blackMaterial;
                }
            }
        }
    }
}
