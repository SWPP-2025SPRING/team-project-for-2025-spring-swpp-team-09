using UnityEngine;
using UnityEngine.UI;

public class UIButtonSound : MonoBehaviour
{
    [SerializeField] private SoundEventChannel soundEventChannel;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            soundEventChannel?.RaisePlaySFX("button_click");
        });
    }
}