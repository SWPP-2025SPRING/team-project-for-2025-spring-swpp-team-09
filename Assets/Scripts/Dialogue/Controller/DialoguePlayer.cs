using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialoguePlayer : MonoBehaviour
{
    [SerializeField] private DialogueUIController uiController;

    private DialogueState state;
    private DialogueLine currentLine;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string dialogueId;

    void Start()
    {
        dialogueId = SceneController.Instance.GetPendingDialogueId();
        DialogueData data = DialogueLoader.Load(dialogueId);

        if (data == null)
        {
            Debug.LogError("대화 데이터를 찾을 수 없습니다: " + dialogueId);
            return;
        }

        state = new DialogueState(data);
        PlayNext();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                uiController.ShowFullText(currentLine.text);
                isTyping = false;
            }
            else
            {
                PlayNext();
            }
        }
    }

    void PlayNext()
    {
        if (!state.HasNext)
        {
            StartCoroutine(LoadSceneAfterDelay(1f));
            return;
        }

        currentLine = state.NextLine();
        uiController.Clear();
        uiController.DisplayLine(currentLine);
        typingCoroutine = StartCoroutine(TypeLine(currentLine.text));
    }

    IEnumerator TypeLine(string text)
    {
        isTyping = true;
        foreach (char c in text)
        {
            uiController.AppendCharacter(c);
            yield return new WaitForSeconds(0.03f);
        }
        isTyping = false;
    }

    IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        string nextScene = SceneController.Instance.GetPendingGameScene();
        SceneController.Instance.ClearPendingSceneData();
        SceneController.Instance.LoadScene(nextScene);
    }

    public void SkipDialogue()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(state.NextSceneName);
    }
}
