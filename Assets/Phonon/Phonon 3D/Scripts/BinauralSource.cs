using System;
using System.Runtime.InteropServices;
using UnityEngine;


//
// BinauralSource
// A Phonon 3D component that applies 3D audio effects to an Audio Source.
//

[AddComponentMenu("Phonon/Phonon 3D Source")]
public class BinauralSource : MonoBehaviour
{
    //
    // Initializes the binaural filter chain.
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

        // Initialize the Phonon audio device.
        PhononRuntime.Start();

        // Make sure the HRTF is loaded.
        IntPtr hrtf = BinauralListener.HRTF();
        if (hrtf == IntPtr.Zero)
        {
            return;
        }

        // Create a filter chain with a binaural effect.
        IPLerror errorCode = IPLerror.NONE;

        errorCode = PhononRuntime.iplCreateFilterChain(ref binauralChain);
        if (errorCode != IPLerror.NONE)
        {
            Debug.LogError("Unable to initialize Phonon 3D Source. Please review the log file phononrt.log for details.");
            return;
        }

        errorCode = PhononRuntime.iplCreateEffect(IPLEffectType.BINAURALUNLIMITED, hrtf, ref binauralEffect);
        if (errorCode != IPLerror.NONE)
        {
            Debug.LogError("Unable to initialize Phonon 3D Source. Please review the log file phononrt.log for details.");
            return;
        }

        errorCode = PhononRuntime.iplAddEffect(binauralChain, binauralEffect);
        if (errorCode != IPLerror.NONE)
        {
            Debug.LogError("Unable to initialize Phonon 3D Source. Please review the log file phononrt.log for details.");
            return;
        }

        // Allocate a buffer for downmixing audio to mono.
        monoAudio = new float[1024];

        // Allocate unmanaged memory for effect parameters.
        binauralParamsBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IPLBinauralUnlimitedEffectParams)));

        listener = GameObject.FindObjectOfType<AudioListener>();
        if (listener == null)
        {
            Debug.LogError("No AudioListener found in the scene. Phonon 3D effects disabled.");
            return;
        }

        // Disable Unity's attenuation model.
#if UNITY_5_0
        GetComponent<AudioSource>().spatialBlend = 0.0f;
#else
        GetComponent<AudioSource>().panLevel = 0.0f;
#endif

        effectActive = true;
    }

    //
    // Destroys the binaural filter chain.
    //
    void OnDestroy()
    {
        // Free unmanaged memory for effect parameters.
        Marshal.FreeHGlobal(binauralParamsBuffer);

        // Destroy the filter chain.
        if (binauralEffect != IntPtr.Zero)
            PhononRuntime.iplDestroyEffect(binauralEffect);
        if (binauralChain != IntPtr.Zero)
            PhononRuntime.iplDestroyFilterChain(binauralChain);

        // Unload the HRTF data.
        BinauralListener.UnloadHRTF();

        // Shut down the Phonon audio device.
        PhononRuntime.Stop();
    }

    //
    // Updates the effect parameters based on the relative position of
    // the source with respect to the listener.
    //
    void Update()
    {
        // If the effect is inactive, stop.
        if (!effectActive)
            return;

        // Get the relative direction from listener to source.
        Vector3 direction = listener.transform.InverseTransformDirection(Vector3.Normalize(transform.position - listener.transform.position));

        // Update the distance attenuation.
        distanceFromListener = Vector3.Magnitude(transform.position - listener.transform.position);
        distanceAttenuation = 1.0f / (1.0f + distanceFromListener);

        // Set up the parameters.
        binauralParams.direction.x = direction.x;
        binauralParams.direction.y = direction.y;
        binauralParams.direction.z = -direction.z;
        binauralParams.enableBinaural = (binauralStatus && enableBinauralRendering) ? IPLbool.IPL_TRUE : IPLbool.IPL_FALSE;

        // Send to unmanaged memory.
        Marshal.StructureToPtr(binauralParams, binauralParamsBuffer, false);

        // Update the effect.
        PhononRuntime.iplUpdateEffectParameters(binauralEffect, binauralParamsBuffer);
    }

    //
    // Processes an incoming frame of audio.
    //
    void OnAudioFilterRead(float[] data, int channels)
    {
        // If the effect is inactive, stop.
        if (!effectActive)
            return;

        // If initialization is not complete, stop.
        if (data == null)
            return;

        // Apply distance attenuation.
        for (int i = 0; i < data.Length; ++i)
            data[i] *= distanceAttenuation;

        // Downmix the incoming audio data to mono.
        for (int i = 0; i < monoAudio.Length; ++i)
            monoAudio[i] = data[2 * i];

        // Process the audio using the filter chain.
        PhononRuntime.iplSendAudioData(binauralChain, monoAudio);
        PhononRuntime.iplReceiveAudioData(binauralChain, data);
    }

    public float Distance()
    {
        return distanceFromListener;
    }

    public float Attenuation()
    {
        return distanceAttenuation;
    }

    public int Priority()
    {
        return priority;
    }

    public void SetBinauralRendering(bool status)
    {
        binauralStatus = status;
    }

    [Range(1, 256)]
    public int priority = 128;
    public bool enableBinauralRendering = true;

    bool effectActive = false;

    float distanceFromListener = 1.0f;      // Distance from the listener.
    float distanceAttenuation = 1.0f;       // Attenuation due to distance.
    bool binauralStatus = true;             // Binaural rendering status.

    IntPtr binauralChain = IntPtr.Zero;
    IntPtr binauralEffect = IntPtr.Zero;

    IntPtr binauralParamsBuffer = IntPtr.Zero;
    IPLBinauralUnlimitedEffectParams binauralParams;

    float[] monoAudio = null;
    AudioListener listener = null;
}
