using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueData
{
    public string nextSceneName;
    public List<DialogueLine> lines = new();

    public DialogueData(string nextScene)
    {
        this.nextSceneName = nextScene;
    }

    public void Add(string speaker, string text)
    {
        lines.Add(new DialogueLine(speaker, text));
    }
}

