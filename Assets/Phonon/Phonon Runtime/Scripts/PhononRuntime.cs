using System;
using System.Runtime.InteropServices;
using UnityEngine;

public enum IPLerror
{
	NONE,
    MEMORY,
	FILEIO,
	UNSUPPORTED_FORMAT,
	INVALID_INPUT,
	INITIALIZATION,
	UNKNOWN
};

public enum IPLSpeakerLayout
{
	MONO,
	STEREO,
	BINAURAL,
	QUADROPHONIC,
	FIVE_0,
	FIVE_1,
	SEVEN_1
};

public enum IPLEffectType
{
    DELAY,
    EQ,
    REVERB,
    CONVOLUTIONREVERB,
    DIRECTIONALCONVOLUTIONREVERB,
    PANNING,
    BINAURAL,
    BINAURALUNLIMITED
};

public struct IPLVector3
{
	public float x;
	public float y;
	public float z;
};

public enum IPLbool
{
    IPL_FALSE,
    IPL_TRUE
};

public struct IPLBinauralUnlimitedEffectParams
{
    public IPLVector3 direction;
    public IPLbool enableBinaural;
};

public class PhononRuntime 
{
    [DllImport("phononrt")]
    public static extern void iplOpenLogFile(string fileName);

    [DllImport("phononrt")]
    public static extern void iplCloseLogFile();

    [DllImport("phononrt")]
    public static extern void iplSetAudioFrameSize(int frameSize);

    [DllImport("phononrt")]
    public static extern void iplSetAudioSamplingRate(float samplingRate);

    [DllImport("phononrt")]
    public static extern void iplSetSpeakerLayout(IPLSpeakerLayout speakerLayout);

    [DllImport("phononrt")]
	public static extern IPLerror iplLoadHRTF(string fileName, [In, Out] ref IntPtr hrtf);
	
	[DllImport("phononrt")]
	public static extern void iplUnloadHRTF(IntPtr hrtf);

	[DllImport("phononrt")]
	public static extern IPLerror iplCreateEffect(IPLEffectType type, IntPtr properties, [In, Out] ref IntPtr effect);

	[DllImport("phononrt")]
	public static extern void iplDestroyEffect(IntPtr effect);

	[DllImport("phononrt")]
	public static extern void iplUpdateEffectParameters(IntPtr effect, IntPtr parameters);

	[DllImport("phononrt")]
	public static extern IPLerror iplCreateFilterChain([In, Out] ref IntPtr chain);

	[DllImport("phononrt")]
	public static extern void iplDestroyFilterChain(IntPtr chain);

	[DllImport("phononrt")]
	public static extern IPLerror iplAddEffect(IntPtr chain, IntPtr effect);

	[DllImport("phononrt")]
	public static extern void iplSendAudioData(IntPtr chain, float[] data);

	[DllImport("phononrt")]
	public static extern void iplReceiveAudioData(IntPtr chain, float[] data);

    [DllImport("phononrt")]
    public static extern void iplDumpProfile();

    //
    // Initializes the Phonon runtime.
    //
    public static void Start()
    {
        try
        {
            iplOpenLogFile("phononrt.log");
            iplSetAudioFrameSize(1024);
            iplSetAudioSamplingRate(AudioSettings.outputSampleRate);
            iplSetSpeakerLayout(IPLSpeakerLayout.STEREO);
        }
        catch (Exception e) { }
    }

    //
    // Shuts down the Phonon runtime.
    //
    public static void Stop()
    {
        try
        {
            iplCloseLogFile();
            iplDumpProfile();
        }
        catch (Exception e) { }
    }

}
