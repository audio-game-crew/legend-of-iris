using UnityEngine;
using System.Collections;
using System.Linq;

public class SoundSystemManager : MonoBehaviour {
    public static SoundSystemManager instance;

    public SoundSystem soundSystem = SoundSystem.UnityDefault;

    void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        HandleSourceSettings();
        HandleListenerSettings();
    }

    public static void HandleAudioSource(GameObject gameObject)
    {
        if (gameObject.audio != null)
        {
            HandleAudioSource(gameObject.audio);
        }
    }

    public static void HandleAudioSource(AudioSource audio)
    {
        SetAstoundSoundEnabled(audio, instance.soundSystem == SoundSystem.AstoundSound); 
        SetPhononEnabled(audio, instance.soundSystem == SoundSystem.Phonon);
        audio.panLevel = instance.soundSystem == SoundSystem.Phonon ? 0 : 1;
    }

    private static void SetAstoundSoundEnabled(AudioSource audio, bool enabled)
    {
        AstoundSoundRTIFilter filter = audio.GetComponent<AstoundSoundRTIFilter>();
        if (enabled && filter == null)
        {
            filter = audio.gameObject.AddComponent<AstoundSoundRTIFilter>();
        }
        if (filter != null)
            filter.enabled = enabled;
    }

    private static void SetPhononEnabled(AudioSource audio, bool enabled)
    {
        var filter = audio.GetComponent<BinauralSource>();
        if (enabled && filter == null)
        {
            filter = audio.gameObject.AddComponent<BinauralSource>();
        }
        if (filter != null)
            filter.enabled = enabled;
    }

    public static void HandleListenerSettings()
    {
        SetListenersEnabled<AstoundSoundRTIListener>(instance.soundSystem == SoundSystem.AstoundSound);
        SetListenersEnabled<BinauralListener>(instance.soundSystem == SoundSystem.Phonon);
    }

    private static void SetListenersEnabled<T>(bool enabled) where T:MonoBehaviour
    {
        var listeners = FindObjectsOfType<T>();
        foreach (var l in listeners)
        {
            // only possible if the listener is currently on an active audio listener
            if (l.GetComponent<AudioListener>().enabled)
                l.enabled = enabled;
            else
                l.enabled = false;
        }
    }

    public static void HandleSourceSettings()
    {
        FindObjectsOfType<AudioSource>().ToList().ForEach(a => a.panLevel = instance.soundSystem == SoundSystem.Phonon ? 0 : 1);
        SetSourcesEnabled<AstoundSoundRTIFilter>(instance.soundSystem == SoundSystem.AstoundSound);
        SetSourcesEnabled<BinauralSource>(instance.soundSystem == SoundSystem.Phonon);
    }

    private static void SetSourcesEnabled<T>(bool enabled) where T:MonoBehaviour
    {
       var filters = FindObjectsOfType<T>();
        foreach (var f in filters)
        {
            f.enabled = enabled;
        }
    }

    public void SetUnityDefault()
    {
        instance.soundSystem = SoundSystem.UnityDefault;
        HandleListenerSettings();
        HandleSourceSettings();
    }

    public void SetAstoundSound()
    {
        instance.soundSystem = SoundSystem.AstoundSound;
        HandleListenerSettings();
        HandleSourceSettings();
    }

    public void SetPhonon3D()
    {
        instance.soundSystem = SoundSystem.Phonon;
        HandleListenerSettings();
        HandleSourceSettings();
    }

    public enum SoundSystem
    {
        UnityDefault = 0, AstoundSound = 1, Phonon = 2
    }
}
