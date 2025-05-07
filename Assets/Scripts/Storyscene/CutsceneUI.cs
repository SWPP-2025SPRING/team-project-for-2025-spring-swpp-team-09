using TMPro;
using UnityEngine;

public class CutsceneUI : MonoBehaviour
{
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;

    public void Show(CutsceneLine line)
    {
        speakerText.text = line.speaker;
        dialogueText.text = line.text;
    }

    public void Clear()
    {
        speakerText.text = "";
        dialogueText.text = "";
    }
}
