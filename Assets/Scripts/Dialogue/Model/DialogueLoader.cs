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
                var data = new DialogueData(entry.nextScene, entry.background);

                foreach (var line in entry.lines)
                {
                    string spriteName = line.spriteName;
                    bool isLeft = line.isLeft;

                    if (string.IsNullOrEmpty(spriteName))
                    {
                        switch (line.speaker)
                        {
                            case "정파 맹주":
                                spriteName = "stage1enemy";
                                isLeft = false;
                                break;
                            case "울트론":
                                spriteName = "stage2enemy";
                                isLeft = false;
                                break;
                            case "프레데터":
                                spriteName = "stage3enemy";
                                isLeft = false;
                                break;
                            case "멸화사":
                                spriteName = "boss";
                                isLeft = false;
                                break;
                        }
                    }

                    data.Add(line.speaker, line.text, spriteName, isLeft);
                }

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
        public string background;
        public List<DialogueLineJSON> lines;
    }

    [System.Serializable]
    private class DialogueLineJSON
    {
        public string speaker;
        public string text;
        public string spriteName;
        public bool isLeft = true;
    }
}
