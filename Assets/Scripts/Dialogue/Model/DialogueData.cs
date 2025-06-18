using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueData
{
    public string nextSceneName;
    public string backgroundPath;
    public List<DialogueLine> lines = new();

    public DialogueData(string nextScene, string background = "")
    {
        this.nextSceneName = nextScene;
        this.backgroundPath = background;
    }

    public void Add(string speaker, string text, string spriteName = "", bool isLeft = true)
    {
        lines.Add(new DialogueLine(speaker, text, spriteName, isLeft));
    }
}

