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
    Vector2 velocity; // FixedUpdate()���� �ʿ��� ���� �߰�

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);  // �ߺ� ����
        }
    }

    public void SaveData(PlayerData playerData)
    {
        string filePath = Application.persistentDataPath + "/player_data.json"; // ���ϸ� ����
        string json = JsonUtility.ToJson(playerData, true);
        System.IO.File.WriteAllText(filePath, json);
        Debug.Log("���� ������ �����: " + json);
    }

    public PlayerData LoadData()
    {
        string filePath = Application.persistentDataPath + "/player_data.json"; // ���ϸ� ����
        if (System.IO.File.Exists(filePath))
        {
            string json = System.IO.File.ReadAllText(filePath);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("���� ������ �ε��: " + json);
            return playerData;
        }
        else
        {
            Debug.LogWarning("����� ���� �����Ͱ� �����ϴ�.");
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
        if (playerData != null) // null üũ ��� ����
        {
            playerData.stage = 1; // �������� �ʱ�ȭ

            for (int i = playerData.collectedItems.Count - 1; i >= 0; i--) // ������ ������ ���� ���
            {
                if (UnityEngine.Random.Range(0, 2) == 0)  // 50% Ȯ��
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
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime); // velocity ���� ����
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