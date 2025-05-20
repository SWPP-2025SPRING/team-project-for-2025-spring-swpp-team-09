using TMPro;
using UnityEngine;

public class DialogueUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    public void DisplayLine(DialogueLine line)
    {
        speakerText.text = line.speaker;
        dialogueText.text = "";
    }

    public void AppendCharacter(char c)
    {
        dialogueText.text += c;
    }

    public void ShowFullText(string text)
    {
        dialogueText.text = text;
    }

    public void Clear()
    {
        speakerText.text = "";
        dialogueText.text = "";
    }
}
