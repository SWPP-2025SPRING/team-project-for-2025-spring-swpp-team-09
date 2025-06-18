using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueLine
{
    public string speaker;
    public string text;
    
    public string spriteName;
    public bool isLeft;

    public DialogueLine(string speaker, string text, string spriteName = "", bool isLeft = true)
    {
        this.speaker = speaker;
        this.text = text;
        this.spriteName = spriteName;
        this.isLeft = isLeft;
    }

    public bool IsNarration => string.IsNullOrEmpty(speaker) || speaker.ToLower() == "나레이션";
}