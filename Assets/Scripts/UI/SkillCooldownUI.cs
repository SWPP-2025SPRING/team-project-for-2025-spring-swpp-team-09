using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCooldownUI : MonoBehaviour
{
    public Image cooldownImage; // The child image with fill method
    private float cooldownTime;
    private float cooldownRemaining;
    private bool isCoolingDown;

    public void StartCooldown(float duration)
    {
        cooldownTime = duration;
        cooldownRemaining = duration;
        isCoolingDown = true;
        cooldownImage.fillAmount = 1f;
    }

    void Update()
    {
        if (!isCoolingDown) return;

        cooldownRemaining -= Time.deltaTime;
        float ratio = Mathf.Clamp01(cooldownRemaining / cooldownTime);
        cooldownImage.fillAmount = ratio;

        if (cooldownRemaining <= 0f)
        {
            isCoolingDown = false;
            cooldownImage.fillAmount = 0f;
        }
    }
}
