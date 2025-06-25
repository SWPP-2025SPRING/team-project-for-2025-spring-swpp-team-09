using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeStopSkill : ISkill
{
    [SerializeField] private SoundEventChannel soundEventChannel;
    public TimeStopSkill(SoundEventChannel soundEventChannel)
    {
        this.soundEventChannel = soundEventChannel;
    }
    public IEnumerator Execute(SkillExecutionContext context)
    {
        Debug.Log($"[TimeStopSkill] Execute 실행됨. 채널 상태: {(soundEventChannel == null ? "NULL" : "OK")}");
        soundEventChannel?.RaisePlaySFX("time_stop");
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1f;
        context.onSkillEnded?.Invoke();
    }
}

