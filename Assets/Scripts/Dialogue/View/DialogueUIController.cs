using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private Image leftCharacterImage;
    [SerializeField] private Image rightCharacterImage;

    [SerializeField] private Animator leftAnimator;
    [SerializeField] private Animator rightAnimator;

    private bool leftCharacterShown = false;
    private bool rightCharacterShown = false;

    public void DisplayLine(DialogueLine line)
    {
        speakerText.text = line.speaker;
        dialogueText.text = "";

        if (line.IsNarration)
        {
            HideCharacters();
        }
    }

    public void DisplayCharacter(DialogueLine line)
    {
        if (line.IsNarration)
            return;

        string path = $"CharacterSprites/{line.spriteName}";
        Sprite newSprite = Resources.Load<Sprite>(path);

        if (newSprite == null)
        {
            Debug.LogError($"❌ [DialogueUIController] Sprite not found at Resources/{path}");
            return;
        }

        if (line.isLeft)
        {
            leftCharacterImage.sprite = newSprite;

            if (!leftCharacterShown)
            {
                leftCharacterImage.gameObject.SetActive(true);
                leftAnimator?.SetTrigger("SlideIn");
                leftCharacterShown = true;
            }

            if (!rightCharacterShown)
                rightCharacterImage.gameObject.SetActive(false);

            leftCharacterImage.color = Color.white;
            if (rightCharacterShown)
                rightCharacterImage.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            rightCharacterImage.sprite = newSprite;

            if (!rightCharacterShown)
            {
                rightCharacterImage.gameObject.SetActive(true);
                rightAnimator?.SetTrigger("SlideIn");
                rightCharacterShown = true;
            }

            if (!leftCharacterShown)
                leftCharacterImage.gameObject.SetActive(false);

            rightCharacterImage.color = Color.white;
            if (leftCharacterShown)
                leftCharacterImage.color = new Color(1, 1, 1, 0.5f);
        }
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

    private void HideCharacters()
    {
        if (leftCharacterShown)
        {
            leftAnimator?.SetTrigger("SlideOut");
            leftCharacterImage.color = Color.white;
            leftCharacterImage.gameObject.SetActive(false);
            leftCharacterShown = false;
        }

        if (rightCharacterShown)
        {
            rightAnimator?.SetTrigger("SlideOut");
            rightCharacterImage.color = Color.white;
            rightCharacterImage.gameObject.SetActive(false);
            rightCharacterShown = false;
        }
    }
}