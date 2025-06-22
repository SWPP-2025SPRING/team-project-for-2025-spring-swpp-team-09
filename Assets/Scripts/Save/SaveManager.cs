using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveStagePlayed(string stageId)
    {
        PlayerPrefs.SetInt($"{stageId}_Played", 1);
        PlayerPrefs.Save();
    }

    public bool IsStagePlayed(string stageId)
    {
        return PlayerPrefs.HasKey($"{stageId}_Played");
    }

    public void SaveStageClear(string stageId)
    {
        PlayerPrefs.SetInt($"{stageId}_Cleared", 1);
        PlayerPrefs.Save();
    }

    public bool IsStageCleared(string stageId)
    {
        return PlayerPrefs.HasKey($"{stageId}_Cleared");
    }

    public void SaveBestTimeIfBetter(string stageId, float newTime)
    {
        string key = $"{stageId}_BestTime";
        float prev = PlayerPrefs.GetFloat(key, float.MaxValue);
        if (newTime < prev)
        {
            PlayerPrefs.SetFloat(key, newTime);
            PlayerPrefs.Save();
        }
    }

    public float GetBestTime(string stageId)
    {
        return PlayerPrefs.GetFloat($"{stageId}_BestTime", float.MaxValue);
    }

    public void SaveClearRank(string stageId, string rank)
    {
        PlayerPrefs.SetString($"{stageId}_ClearRank", rank);
        PlayerPrefs.Save();
    }

    public string GetClearRank(string stageId)
    {
        if (PlayerPrefs.HasKey($"{stageId}_ClearRank"))
        {
            return PlayerPrefs.GetString($"{stageId}_ClearRank");
        }
        else
        {
            return null;
        }
    }

    public void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
