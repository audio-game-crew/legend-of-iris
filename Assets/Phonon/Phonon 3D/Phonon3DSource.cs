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
using UnityEngine;
using Phonon;


//
// Phonon3DSource
// Enables binaural rendering for a given source.
//

[AddComponentMenu("Phonon/Phonon 3D Source")]
public class Phonon3DSource : MonoBehaviour
{
    //
    // Initializes the source.
    //
    void Awake()
    {
        // If no AudioSource is attached to this GameObject,
        // disable binaural filtering.
        if (GetComponent<AudioSource>() == null)
        {
            Debug.LogError("No AudioSource attached to Phonon 3D Source. Phonon 3D effects disabled for GameObject: " + gameObject.name + ".");
            return;
        }

        // If the speaker configuration does not have 2 channels,
        // disable binaural filtering.
        if (AudioSettings.speakerMode != AudioSpeakerMode.Stereo)
        {
            Debug.LogError("Phonon 3D requires stereo output. Use Edit > Project Settings > Audio > Default Speaker Mode to fix this.");
            return;
        }

        // Make sure the listener has been initialized.
        Phonon3DListener.InitializePhonon3D();

        // Initialize the source.
        if (Phonon3D.iplCreateSource(Priority, ref source) != Error.NONE)
        {
            Debug.Log("Unable to create Phonon 3D Source for object: " + gameObject.name + ". Please check the log file for details.");
            return;
        }

        // Allocate a buffer for downmixing dry audio to mono.
        monoAudio = new float[Phonon3DListener.GetFrameSize()];

        // Mark the effect as enabled.
        effectEnabled = true;
    }

    //
    // Destroys the source.
    //
    void OnDestroy()
    {
    }

    //
    // Updates the source position.
    //
    void Update()
    {
        if (!effectEnabled)
            return;

        Phonon3D.iplUpdateSource(source, Common.ConvertVector(transform.position));
    }

    //
    // Applies the Phonon 3D effect to dry audio.
    //
    void OnAudioFilterRead(float[] data, int channels)
    {
        if (!effectEnabled || data == null)
            return;

		for (int i = 0; i < monoAudio.Length; ++i)
		{
			monoAudio[i] = 0.0f;
			for (int j = 0; j < channels; ++j)
				monoAudio[i] += data[i*channels + j];
			monoAudio[i] /= channels;
		}

        Phonon3D.iplProcessSource(source, monoAudio, data);
    }

    //
    // Data members.
    //

    // Is this effect enabled?
    bool effectEnabled = false;

    // Buffer for down-mixing dry audio.
    float[] monoAudio = null;

    // API handles.
    IntPtr source = IntPtr.Zero;

    //
    // Public properties.
    //

	[Range(0, 256)]
    public int Priority = Phonon3D.DefaultSourcePriority;
}