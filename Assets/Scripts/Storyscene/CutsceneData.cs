using System.Collections.Generic;

public class CutsceneData
{
    public string nextSceneName;
    public List<CutsceneLine> lines = new List<CutsceneLine>();

    public CutsceneData(string nextScene)
    {
        this.nextSceneName = nextScene;
    }

    public void Add(string speaker, string text)
    {
        lines.Add(new CutsceneLine(speaker, text));
    }
}
