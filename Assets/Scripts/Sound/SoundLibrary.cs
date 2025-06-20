using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundLibrary", menuName = "Audio/SoundLibrary")]
public class SoundLibrary : ScriptableObject
{
    [SerializeField] private List<SoundEntry> sfxList;
    [SerializeField] private List<SoundEntry> bgmList;

    private Dictionary<string, SoundEntry> sfxDict;
    private Dictionary<string, AudioClip> bgmDict;

    [Serializable]
    public class SoundEntry
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f;
    }

    private void OnEnable()
    {
        sfxDict = new Dictionary<string, SoundEntry>();
        bgmDict = new Dictionary<string, AudioClip>();

        foreach (var entry in sfxList)
            if (!sfxDict.ContainsKey(entry.name))
                sfxDict.Add(entry.name, entry);

        foreach (var entry in bgmList)
            if (!bgmDict.ContainsKey(entry.name))
                bgmDict.Add(entry.name, entry.clip);
    }

    public (AudioClip clip, float volume) GetSFX(string name)
    {
        if (sfxDict.TryGetValue(name, out var entry))
        {
            return (entry.clip, entry.volume);
        }
        return (null, 1f);
    }

    public AudioClip GetBGM(string name)
    {
        return bgmDict.TryGetValue(name, out var clip) ? clip : null;
    }
}