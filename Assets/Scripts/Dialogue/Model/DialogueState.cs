using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueState
{
    private Queue<DialogueLine> queue;
    public string NextSceneName { get; private set; }

    public DialogueState(DialogueData data)
    {
        this.queue = new Queue<DialogueLine>(data.lines);
        this.NextSceneName = data.nextSceneName;
    }

    public bool HasNext => queue.Count > 0;

    public DialogueLine NextLine() => queue.Dequeue();
}

