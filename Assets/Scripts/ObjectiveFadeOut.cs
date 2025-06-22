using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveFadeOut : MonoBehaviour
{
    public float displayTime = 2f;
    public float fadeDuration = 3f;

    private TextMeshProUGUI textMesh;
    private float timer = 0f;
    private bool fading = false;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        Color c = textMesh.color;
        c.a = 1f;
        textMesh.color = c;

        // 지정 시간 후 페이드 시작
        Invoke(nameof(StartFadeOut), displayTime);
    }

    void StartFadeOut()
    {
        fading = true;
    }

    void Update()
    {
        if (fading)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            Color c = textMesh.color;
            c.a = alpha;
            textMesh.color = c;

            if (alpha <= 0f)
            {
                fading = false;
                gameObject.SetActive(false); // 사라진 뒤 오브젝트 숨기기
            }
        }
    }
}