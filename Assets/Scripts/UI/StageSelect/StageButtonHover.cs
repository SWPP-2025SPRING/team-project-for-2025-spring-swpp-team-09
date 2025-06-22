using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class StageButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string stageId;
    [SerializeField] private TextMeshProUGUI displayText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (displayText != null)
        {
            string info = StageRecordPresenter.GetStageRecordDisplay(stageId);
            displayText.text = info;
            displayText.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (displayText != null)
        {
            displayText.text = "";
            displayText.gameObject.SetActive(false);
        }
    }
}
