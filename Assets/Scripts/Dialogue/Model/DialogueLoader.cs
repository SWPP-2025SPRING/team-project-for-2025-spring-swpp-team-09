using System.Collections.Generic;
using UnityEngine;

public static class DialogueLoader
{
    public static DialogueData Load(string id)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("dialogues_ko");
        if (jsonFile == null)
        {
            Debug.LogError("JSON 파일을 찾을 수 없습니다: dialogues_ko");
            return null;
        }

        DialogueWrapper wrapper = JsonUtility.FromJson<DialogueWrapper>(jsonFile.text);
        foreach (var entry in wrapper.entries)
        {
            if (entry.id == id)
            {
                var data = new DialogueData(entry.nextScene);
                foreach (var line in entry.lines)
                    data.Add(line.speaker, line.text);
                return data;
            }
        }

        Debug.LogWarning("해당 ID의 대사를 찾을 수 없습니다: " + id);
        return null;
    }

    [System.Serializable]
    private class DialogueWrapper
    {
        public List<DialogueEntry> entries;
    }

    [System.Serializable]
    private class DialogueEntry
    {
        public string id;
        public string nextScene;
        public List<DialogueLineJSON> lines;
    }

    [System.Serializable]
    private class DialogueLineJSON
    {
        public string speaker;
        public string text;
    }
}
