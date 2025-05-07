using System.Collections.Generic;
using UnityEngine;

public static class CutsceneDatabase
{
    private static Dictionary<string, CutsceneData> cache = new();
    private static bool loaded = false;

    public static void LoadFromJSON(string resourcePath)
    {
        if (loaded) return;

        TextAsset jsonFile = Resources.Load<TextAsset>(resourcePath);
        if (jsonFile == null)
        {
            Debug.LogError("Failed to load JSON from Resources: " + resourcePath);
            return;
        }

        RawCutsceneWrapper wrapper = JsonUtility.FromJson<RawCutsceneWrapper>(jsonFile.text);

        foreach (RawCutsceneEntry entry in wrapper.entries)
        {
            var data = new CutsceneData(entry.nextScene);
            foreach (var line in entry.lines)
            {
                data.Add(line.speaker, line.text);
            }
            cache[entry.id] = data;
        }

        loaded = true;
    }

    public static CutsceneData Get(string id)
    {
        if (!loaded)
        {
            LoadFromJSON("cutscenes_ko");
        }
        return cache.ContainsKey(id) ? cache[id] : null;
    }

    [System.Serializable]
    private class RawCutsceneWrapper
    {
        public List<RawCutsceneEntry> entries;
    }

    [System.Serializable]
    private class RawCutsceneEntry
    {
        public string id;
        public string nextScene;
        public List<CutsceneLineJSON> lines;
    }

    [System.Serializable]
    private class CutsceneLineJSON
    {
        public string speaker;
        public string text;
    }
}
