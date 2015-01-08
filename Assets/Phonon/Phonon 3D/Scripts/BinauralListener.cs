using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


//
// BinauralListener
// A Phonon 3D component that tracks the head position and orientation for 3D audio.
// Also performs culling and prioritization to maintain performance.
//

[AddComponentMenu("Phonon/Phonon 3D Listener")]
public class BinauralListener : MonoBehaviour 
{
    //
    // Upon initialization, cull and rank sources.
    //
    void Awake()
    {
    }
    
    //
    // At the end of every frame, cull and rank sources.
    //
    void LateUpdate()
    {
        CullRankAndConfigure();
    }

    // Comparator for sorting.
    static int PriorityComparer(KeyValuePair<int, BinauralSource> a, KeyValuePair<int, BinauralSource> b)
    {
        return b.Key.CompareTo(a.Key);
    }

    //
    // Culls unimportant sources, and sorts important sources by priority.
    //
    private void CullRankAndConfigure()
    {
        // Find all the active sources.
        if (binauralSources == null)
        {
            binauralSources = GameObject.FindObjectsOfType<BinauralSource>();
            if (binauralSources == null)
            {
                Debug.LogWarning("No Phonon 3D Sources found. Phonon 3D Listener disabled.");
                return;
            }
        }

        // Initialize the list of important sources.
        var sourceRankList = new List<KeyValuePair<int, BinauralSource>>();

        // Cull sources that are too far, too low in volume, or that have been explicitly
        // set to panning mode.
        foreach (BinauralSource source in binauralSources)
        {
            if (source.Attenuation() < cutOffAmplitude || source.Distance() > cutOffDistance || !source.enableBinauralRendering)
            {
                source.SetBinauralRendering(false);
            }
            else
            {
                int priority = ComputePriority(source.priority, source.Distance(), source.Attenuation());
                sourceRankList.Add(new KeyValuePair<int, BinauralSource>(priority, source));
            }
        }

        // Sort the list of sources.
        sourceRankList.Sort(PriorityComparer);

        // Set the highest-ranked sources to binaural mode, and the rest to panning mode.
        for (int i = 0; i < sourceRankList.Count; ++i)
        {
            sourceRankList[i].Value.SetBinauralRendering(i < max3DSoundSources);
        }
    }

    //
    // Calculate the rank of a source based on its amplitude, distance, and user-specified priority.
    //
    private int ComputePriority(int sourcePriority, float distance, float amplitude)
    {
        float priority = sourcePriority + 256 * ((cutOffDistance - distance) / cutOffDistance) + 256 * ((amplitude - cutOffAmplitude) / (1.0f - cutOffAmplitude));
        return (int)priority;
    }

    //
    // Loads an HRTF file.
    //
    public static IntPtr HRTF()
    {
        if (hrtf == IntPtr.Zero)
        {
            string hrtfFileName = Path.Combine(Application.streamingAssetsPath, "cipic_124.hrtf");
            
            IPLerror errorCode = PhononRuntime.iplLoadHRTF(hrtfFileName, ref hrtf);
            if (errorCode != IPLerror.NONE)
            {
                Debug.LogError("Unable to load HRTF data from " + hrtfFileName + ". Please verify that the file is present.");
                return IntPtr.Zero;
            }
        }

        numSources++;

        return hrtf;
    }

    //
    // Unloads HRTF data when sources are being destroyed.
    //
    public static void UnloadHRTF()
    {
        numSources--;

        if (numSources == 0)
        {
            if (hrtf != IntPtr.Zero)
            {
                PhononRuntime.iplUnloadHRTF(hrtf);
                hrtf = IntPtr.Zero;
            }
        }
    }

    //
    // Data members.
    //
    
    [Range(0.0f, 1000.0f)]
    public float cutOffDistance = 100.0f;
    
    [Range(.0f, 1.0f)]
    public float cutOffAmplitude = 0.02f;
    
    [Range(1, 256)]
    public int max3DSoundSources = 25;

    BinauralSource[] binauralSources = null;

    static IntPtr hrtf = IntPtr.Zero;
    static int numSources = 0;
}
