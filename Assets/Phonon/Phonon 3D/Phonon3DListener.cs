/************************************************************************/
/* Copyright (C) 2011-2015 Impulsonic Inc. All Rights Reserved.         */
/*                                                                      */
/* The source code, information  and  material ("Material") contained   */
/* herein is owned  by Impulsonic Inc. or its suppliers or licensors,   */
/* and title to such  Material remains  with Impulsonic  Inc.  or its   */
/* suppliers or licensors. The Material contains proprietary informa-   */
/* tion  of  Impulsonic or  its  suppliers and licensors. No  part of   */
/* the Material may be used, copied, reproduced, modified, published,   */
/* uploaded, posted, transmitted, distributed or disclosed in any way   */
/* without Impulsonic's prior express written permission. No  license   */
/* under  any patent, copyright or other intellectual property rights   */
/* in the Material is  granted  to  or  conferred  upon  you,  either   */
/* expressly, by implication, inducement, estoppel or otherwise.  Any   */
/* license  under  such intellectual property rights must  be express   */
/* and approved by Impulsonic in writing.                               */
/*                                                                      */
/* Third Party trademarks are the property of their respective owners.  */
/*                                                                      */
/* Unless otherwise  agreed upon by Impulsonic  in  writing, you  may   */
/* not remove or  alter this  notice or any other  notice embedded in   */
/* Materials by Impulsonic or Impulsonic's  suppliers or licensors in   */
/* any way.                                                             */
/************************************************************************/

using System;
using System.IO;

using UnityEngine;

using Phonon;


//
// Phonon3DListener
// Represents a binaural listener and its HRTF.
//

[AddComponentMenu("Phonon/Phonon 3D Listener")]
public class Phonon3DListener : MonoBehaviour
{
    /// <summary>
    /// Bool to track if a listener exists somewhere in the scene
    /// </summary>
    private static bool exists = false;

    //
    // Initializes the listener.
    //
    void Awake()
    {
        exists = true;
        InitializePhonon3D();
    }

    //
    // Main initialization function.
    //
    public static void InitializePhonon3D()
    {
        if (effectEnabled)
            return;

        // Save out the audio pipeline settings.
        int numBuffers;
        AudioSettings.GetDSPBufferSize(out frameSize, out numBuffers);

        // Initialize the audio pipeline.
        if (Common.iplInitializeAudioPipeline(AudioSettings.outputSampleRate, frameSize, SpeakerLayout.STEREO) != Error.NONE)
        {
            Debug.Log("Unable to initialize Phonon audio pipeline. Please check the log file for details.");
            return;
        }

        // Make sure that a listener exists.
        Phonon3DListener listener = GameObject.FindObjectOfType<Phonon3DListener>();
        if (listener == null)
        {
            Debug.Log("No Phonon 3D Listener found. Please add a Phonon 3D Listener component to your main camera and try again.");
            return;
        }

        // Construct the full path to the HRTF file.
#if UNITY_ANDROID

	    string hrtfAssetFile = Path.Combine(Application.streamingAssetsPath, hrtfFileName);
		Debug.Log(hrtfAssetFile);
	    WWW streamingAssetLoader = new WWW(hrtfAssetFile);
	    while (!streamingAssetLoader.isDone) ;
	    byte[] assetData = streamingAssetLoader.bytes;
	    string hrtfPath = Path.Combine(Application.temporaryCachePath, hrtfFileName);
	    BinaryWriter dataWriter = new BinaryWriter(new FileStream(hrtfPath, FileMode.Create));
	    dataWriter.Write(assetData);

#else
        string hrtfPath = Path.Combine(Application.streamingAssetsPath, hrtfFileName);
#endif

        // Copy the listener settings.
        ListenerSettings listenerSettings;
        listenerSettings.maxSources = listener.MaxSources;
        listenerSettings.maxDistance = listener.MaxDistance;
        listenerSettings.minAttenuation = 0.02f;

        // Create the listener.
        if (Phonon3D.iplCreateListener(hrtfPath, listenerSettings) != Error.NONE)
        {
            Debug.Log("Unable to create Phonon 3D Listener. Please check the log file for details.");
            return;
        }

        // Mark the effect as enabled.
        effectEnabled = true;
    }

    //
    // Destroys the listener.
    //
    void OnDestroy()
    {
        if (!effectEnabled)
            return;

        if (exists)
            Phonon3D.iplDestroyListener();
        exists = false;
    }

    //
    // Updates the listener position and orientation.
    //
    void Update()
    {
        if (!effectEnabled)
            return;

        Phonon3D.iplUpdateListener(Common.ConvertVector(transform.position), Common.ConvertVector(transform.forward), Common.ConvertVector(transform.up));
    }

    //
    // Returns the frame size.
    //
    public static int GetFrameSize()
    {
        return frameSize;
    }

    //
    // Data members.
    //

    // Is the effect enabled?
    static bool effectEnabled = false;

    // Default HRTF file name.
    static string hrtfFileName = "cipic_124.hrtf";

    // Audio pipeline settings.
    static int frameSize = 0;

    //
    // Public properties.
    //

    [Range(1, 64)]
    public int MaxSources = 32;

    [Range(0.0f, 500.0f)]
    public float MaxDistance = 100.0f;
}