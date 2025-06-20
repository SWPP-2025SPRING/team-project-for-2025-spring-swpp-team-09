using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SoundLibrary", menuName = "Audio/SoundLibrary")]
public class SoundLibrary : ScriptableObject
{
    [Serializable]
    public class SoundEntry
    {
        public string name;
        public AudioClip clip;
    }

    public List<SoundEntry> sfxList = new List<SoundEntry>();
    public List<SoundEntry> bgmList = new List<SoundEntry>();

    private Dictionary<string, AudioClip> sfxDict;
    private Dictionary<string, AudioClip> bgmDict;

    private void OnEnable()
    {
        sfxDict = new Dictionary<string, AudioClip>();
        bgmDict = new Dictionary<string, AudioClip>();

        foreach (var sfx in sfxList)
            sfxDict[sfx.name] = sfx.clip;

        foreach (var bgm in bgmList)
            bgmDict[bgm.name] = bgm.clip;
    }

    public AudioClip GetSFX(string name) =>
        sfxDict.TryGetValue(name, out var clip) ? clip : null;

    public AudioClip GetBGM(string name) =>
        bgmDict.TryGetValue(name, out var clip) ? clip : null;
}