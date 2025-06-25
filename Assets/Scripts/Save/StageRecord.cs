using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageRecord
{
    public bool IsPlayed { get; }
    public bool IsCleared { get; }
    public string Rank { get; }
    public float BestTime { get; }

    public StageRecord(bool isPlayed, bool isCleared, string rank, float bestTime)
    {
        IsPlayed = isPlayed;
        IsCleared = isCleared;
        Rank = rank;
        BestTime = bestTime;
    }
}

