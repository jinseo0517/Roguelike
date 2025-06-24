using System.Collections.Generic;

[System.Serializable]
public class GameSaveData
{
    public int totalKills; // 누적 적 처치 수
    public List<string> unlockedCharacters = new List<string>(); // 해금된 캐릭터 목록
}