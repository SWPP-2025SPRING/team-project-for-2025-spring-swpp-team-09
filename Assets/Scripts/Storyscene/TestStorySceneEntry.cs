using UnityEngine;

public class TestStorySceneEntry : MonoBehaviour
{
    void Awake()
    {
        // 테스트용: Prologue 컷씬 로드
        StorySceneLoader.cutsceneId = "Stage1_Enter";
    }
}
