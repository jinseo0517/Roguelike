/*using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class PlayerData
{
    public List<string> collectedItems = new List<string>();
    public int stage = 1;
}

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;
    public PlayerData PlayerData;

    Rigidbody2D rb;
    SpriteRenderer sR;
    Vector2 velocity; // FixedUpdate()에서 필요한 변수 추가

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);  // 중복 방지
        }
    }

    public void SaveData(PlayerData playerData)
    {
        string filePath = Application.persistentDataPath + "/player_data.json"; // 파일명 통일
        string json = JsonUtility.ToJson(playerData, true);
        System.IO.File.WriteAllText(filePath, json);
        Debug.Log("게임 데이터 저장됨: " + json);
    }

    public PlayerData LoadData()
    {
        string filePath = Application.persistentDataPath + "/player_data.json"; // 파일명 통일
        if (System.IO.File.Exists(filePath))
        {
            string json = System.IO.File.ReadAllText(filePath);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("게임 데이터 로드됨: " + json);
            return playerData;
        }
        else
        {
            Debug.LogWarning("저장된 게임 데이터가 없습니다.");
            return new PlayerData();
        }
    }

    public void GameStart()
    {
        PlayerData playerData = LoadData();
        if (playerData == null)
        {
            playerData = new PlayerData();
            SceneManager.LoadScene("Level_1");
        }
        else
        {
            SceneManager.LoadScene("Level_" + playerData.stage);
        }
    }

    public void PlayerDead()
    {
        PlayerData playerData = LoadData();
        if (playerData != null) // null 체크 방식 수정
        {
            playerData.stage = 1; // 스테이지 초기화

            for (int i = playerData.collectedItems.Count - 1; i >= 0; i--) // 안전한 아이템 삭제 방식
            {
                if (UnityEngine.Random.Range(0, 2) == 0)  // 50% 확률
                {
                    playerData.collectedItems.RemoveAt(i);
                }
            }

            SaveData(playerData);
        }

        SceneManager.LoadScene("GameOver");
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime); // velocity 오류 수정
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            ItemObject item = collision.GetComponent<ItemObject>();
            GameDataManager.Instance.PlayerData.collectedItems.Add(item.GetItem());
            Destroy(collision.gameObject);
            GameDataManager.Instance.SaveData(GameDataManager.Instance.PlayerData);
        }
    }
}
*/