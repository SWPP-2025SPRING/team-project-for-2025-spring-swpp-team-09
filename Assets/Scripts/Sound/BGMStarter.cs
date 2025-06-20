using UnityEngine;

public class BGMStarter : MonoBehaviour
{
    [SerializeField] private SoundEventChannel eventChannel;
    [SerializeField] private string bgmName;
    [SerializeField] private bool playOnStart = true;

    private void Start()
    {
        if (playOnStart && eventChannel != null && !string.IsNullOrEmpty(bgmName))
        {
            eventChannel.RaisePlayBGM(bgmName);
        }
    }

    public void Play()
    {
        if (eventChannel != null && !string.IsNullOrEmpty(bgmName))
        {
            eventChannel.RaisePlayBGM(bgmName);
        }
    }
}