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
        StartCoroutine(PlayNext());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isTyping)
        {
            StartCoroutine(PlayNext());
        }
    }

    IEnumerator PlayNext()
    {
        if (queue.Count == 0)
        {
            cutsceneUI.Clear();
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(nextSceneName);
            yield break;
        }

        CutsceneLine line = queue.Dequeue();
        cutsceneUI.Clear();
        isTyping = true;

        cutsceneUI.speakerText.text = line.speaker;
        cutsceneUI.dialogueText.text = "";

        foreach (char c in line.text)
        {
            cutsceneUI.dialogueText.text += c;
            yield return new WaitForSeconds(0.03f);
        }

        isTyping = false;
    }

    public void SkipCutscene()
    {
        StopAllCoroutines();
        cutsceneUI.Clear();
        SceneManager.LoadScene(nextSceneName);
    }
}
