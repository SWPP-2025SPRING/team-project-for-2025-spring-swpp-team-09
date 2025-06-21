public static class StageRecordPresenter
{
    public static string GetStageRecordDisplay(string stageId)
    {
        StageRecord record = GameFlowManager.Instance.GetStageRecord(stageId);

        if (!record.IsPlayed)
            return "플레이 기록이 없습니다.";

        if (record.Rank == "F")
            return $"등급: F\n클리어 기록이 없습니다.";

        if (!record.IsCleared || float.IsPositiveInfinity(record.BestTime))
            return $"등급: {record.Rank}\n클리어 기록이 없습니다.";

        return $"등급: {record.Rank}\n최고 기록: {record.BestTime:F2}초";
    }
}
