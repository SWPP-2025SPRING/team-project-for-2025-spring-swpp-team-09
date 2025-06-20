using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Event Channel")]
    [SerializeField] private SoundEventChannel eventChannel;

    [Header("Sound Library")]
    [SerializeField] private SoundLibrary soundLibrary;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        if (eventChannel != null)
        {
            eventChannel.OnPlaySFX += PlaySFX;
            eventChannel.OnPlayBGM += PlayBGM;
            eventChannel.OnStopBGM += StopBGM;
        }
    }

    private void OnDisable()
    {
        if (eventChannel != null)
        {
            eventChannel.OnPlaySFX -= PlaySFX;
            eventChannel.OnPlayBGM -= PlayBGM;
            eventChannel.OnStopBGM -= StopBGM;
        }
    }

    private void PlaySFX(string name)
    {
        (AudioClip clip, float volume) = soundLibrary.GetSFX(name);
        if (clip == null)
        {
            Debug.LogWarning($"[SoundManager] SFX '{name}' not found.");
            return;
        }
        sfxSource.PlayOneShot(clip, volume);
    }

    private void PlayBGM(string name)
    {
        AudioClip clip = soundLibrary.GetBGM(name);
        if (clip == null)
        {
            Debug.LogWarning($"[SoundManager] BGM '{name}' not found.");
            return;
        }

        if (bgmSource.clip == clip && bgmSource.isPlaying) return;

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    private void StopBGM()
    {
        bgmSource.Stop();
        bgmSource.clip = null;
    }
}