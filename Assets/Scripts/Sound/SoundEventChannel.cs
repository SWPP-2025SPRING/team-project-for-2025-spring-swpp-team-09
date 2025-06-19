using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SoundEventChannel", menuName = "Events/SoundEventChannel")]
public class SoundEventChannel : ScriptableObject
{
    public UnityAction<string> OnPlaySFX;
    public UnityAction<string> OnPlayBGM;
    public UnityAction OnStopBGM;

    public void RaisePlaySFX(string name)
    {
        OnPlaySFX?.Invoke(name);
    }

    public void RaisePlayBGM(string name)
    {
        OnPlayBGM?.Invoke(name);
    }

    public void RaiseStopBGM()
    {
        OnStopBGM?.Invoke();
    }
}