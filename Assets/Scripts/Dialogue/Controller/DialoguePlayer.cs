using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePlayer : MonoBehaviour
{
    [SerializeField] private DialogueUIController uiController;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private SoundEventChannel soundEventChannel;

    private DialogueState state;
    private DialogueLine currentLine;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string dialogueId;

    void Start()
    {
        DialogueData data = InitializeDialogueData(); 
        
        if (data == null) { return; }

        LoadBackgroundImage(data);
        PlayNext();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                }
                uiController.ShowFullText(currentLine.text);
                isTyping = false;
            }
            else
            {
                PlayNext();
            }
        }
    }

    private DialogueData InitializeDialogueData()
{
    dialogueId = SceneController.Instance.GetPendingDialogueId();
    DialogueData data = DialogueLoader.Load(dialogueId);

    if (data == null)
    {
        Debug.LogError($"[DialoguePlayer] 대화 데이터를 찾을 수 없습니다: {dialogueId}");
        SceneController.Instance.ClearPendingSceneData();
        SceneController.Instance.LoadScene("StageSelectScene");
        return null;
    }

    state = new DialogueState(data);
    PlayBGMForDialogue(dialogueId);
    return data;
}

    private void LoadBackgroundImage(DialogueData data)
    {
        if (!string.IsNullOrEmpty(data.backgroundPath))
        {
            Sprite bgSprite = Resources.Load<Sprite>(data.backgroundPath);
            if (bgSprite != null)
            {
                backgroundImage.sprite = bgSprite;
            }
            else
            {
                Debug.LogWarning($"[DialoguePlayer] 배경 이미지 로드 실패: {data.backgroundPath}");
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

        if (!currentLine.IsNarration)
        {
            uiController.DisplayCharacter(currentLine);
        }

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

        string nextStageId = state.NextSceneName;
            if (dialogueId == "Stage3_Clear")
            {
                SceneController.Instance.LoadDialogueThenScene("Epilogue", "StageSelectScene");
                yield break;
            }
            if (string.IsNullOrEmpty(nextStageId))
            {
                SceneController.Instance.LoadScene("StageSelectScene");
                yield break;
            }

        SceneController.Instance.LoadTutorialThenStage("TutorialScene", nextStageId);
    }

    public void SkipDialogue()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        string nextScene = state.NextSceneName;

        if (dialogueId == "Stage3_Clear")
        {
            SceneController.Instance.LoadDialogueThenScene("Epilogue", "StageSelectScene");
            return;
        }

        if (string.IsNullOrEmpty(state.NextSceneName))
        {
            HandleMissingNextScene();
            return;
        }
        if (state.NextSceneName.Contains("GameScene"))
            HandleGameSceneSkip(state.NextSceneName);
        else
            HandleGeneralSkip(state.NextSceneName);
    }

    private void PlayBGMForDialogue(string id)
    {
        string bgmToPlay = id switch
        {
            "Prologue" => "prologue",
            "Stage1_Enter" or "Stage1_Clear" => "stage1dialogue",
            "Stage2_Enter" or "Stage2_Clear" => "stage2dialogue",
            "Stage3_Enter" or "Stage3_Clear" => "stage3dialogue",
            "Epilogue" => "mainStage",
            _ => null
        };

        if (!string.IsNullOrEmpty(bgmToPlay))
        {
            soundEventChannel.RaisePlayBGM(bgmToPlay);
        }
        else
        {
            Debug.LogWarning($"[DialoguePlayer] Unknown BGM for dialogue ID: {id}");
        }
    }

    private void HandleMissingNextScene()
    {
        Debug.LogWarning("[DialoguePlayer] Skip 시 다음 씬 정보가 없어 기본 씬으로 이동합니다.");
        SceneController.Instance.ClearPendingSceneData();
        SceneController.Instance.LoadScene("StageSelectScene");
    }

    private void HandleGameSceneSkip(string nextScene)
    {
        string stageId = nextScene.Replace("GameScene", "");
        PlayerPrefs.SetString("PendingDialogueId", $"{stageId}_Enter");
        SceneController.Instance.LoadTutorialThenStage("TutorialScene", $"{stageId}GameScene");
    }

    private void HandleGeneralSkip(string nextScene)
    {
        SceneController.Instance.ClearPendingSceneData();
        SceneController.Instance.LoadScene(nextScene);
    }
}
