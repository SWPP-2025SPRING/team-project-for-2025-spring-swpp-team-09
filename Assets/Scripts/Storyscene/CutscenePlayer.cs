using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutscenePlayer : MonoBehaviour
{
    public CutsceneUI cutsceneUI;
    private Queue<CutsceneLine> queue;
    private string nextSceneName;
    private bool isTyping = false;
    private CutsceneLine currentLine;
    private Coroutine typingCoroutine;

    void Awake()
    {
        Time.timeScale = 1f;
    }

    void Start()
    {
        CutsceneData data = CutsceneDatabase.Get(StorySceneLoader.cutsceneId);
        if (data == null)
        {
            Debug.LogError("Cutscene data not found for ID: " + StorySceneLoader.cutsceneId);
            return;
        }

        nextSceneName = data.nextSceneName;
        queue = new Queue<CutsceneLine>(data.lines);
        PlayNext();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                cutsceneUI.dialogueText.text = currentLine.text;
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
        if (queue.Count == 0)
        {
            cutsceneUI.Clear();
            StartCoroutine(LoadSceneAfterDelay(1f));
            return;
        }

        currentLine = queue.Dequeue();
        cutsceneUI.Clear();
        cutsceneUI.speakerText.text = currentLine.speaker;

        typingCoroutine = StartCoroutine(TypeText(currentLine.text));
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        cutsceneUI.dialogueText.text = "";

        foreach (char c in text)
        {
            cutsceneUI.dialogueText.text += c;
            yield return new WaitForSeconds(0.03f);
        }

        isTyping = false;
    }

    IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextSceneName);
    }

    public void SkipCutscene()
    {
        StopAllCoroutines();
        cutsceneUI.Clear();
        SceneManager.LoadScene(nextSceneName);
    }
}
