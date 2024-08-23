using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Unity.Burst.Intrinsics.X86;
using System;

public class MainManager : MonoBehaviour
{
    [SerializeField] MusicManager musicManager;
    [SerializeField] MapImageManager mapImageManager;
    [SerializeField] MapData mapData;

    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject clearPanel;
    [SerializeField] GameObject stageText;
    [SerializeField] GameObject nextStageButton;

    List<int[,]> mapList = new List<int[,]>();
    private int listKey = 0;
    private int[,] currentMap;

    private int rows = 13; // 行数
    private int cols = 9; // 列数

    private int playerX = 0;
    private int playerY = 0;
    private int previousX = 0;
    private int previousY = 0;

    private bool inputKey = false;
    private bool stageClear = false;

    private void Start()
    {
        // 「次のステージ」ボタンを押したときは、ルール説明画像を表示しない
        if (PlayerPrefs.GetString("NextStage") == "true")
        {
            StartGame();
            PlayerPrefs.SetString("NextStage", "false");
            PlayerPrefs.Save();
        }
    }

    void Update()
    {
        if (stageClear) return;

        CheckInputKey();
        if (inputKey == false) return;

        MovePlayer();
        UpdateMap();
    }

    private void CheckInputKey()
    {
        if (Input.GetKeyDown(KeyCode.T))  // タイトルに戻る
        {
            SceneManager.LoadScene("TitleScene");
            return;
        }

        if (Input.GetKeyDown(KeyCode.Z))  // 1つ戻る
        {
            if (listKey > 1)
            {
                listKey--;
                mapList.RemoveAt(listKey);
                currentMap = CopyMap(mapList[listKey - 1]);
                mapImageManager.UpdateMapImage(currentMap);
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.R))  // リセットする
        {
            mapList.Clear();

            listKey = 0;
            currentMap = mapData.SettingMap();
            UpdateMap();

            return;
        }

        inputKey = false;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            playerY++;  // 上
            inputKey = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            playerX--;  // 左
            inputKey = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            playerY--;  // 下
            inputKey = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            playerX++;  // 右
            inputKey = true;
        }
    }

    private void MovePlayer()
    {
        var playerValue = currentMap[playerX, playerY];

        // 2:プレイヤー の移動先が 0:地面 の場合
        if (playerValue == 0)
        {
            currentMap[previousX, previousY] = 0;
            currentMap[playerX, playerY] = 2;
            return;
        }

        // 2:プレイヤー の移動先が 3:カメ の場合
        if (playerValue == 3)
        {
            var turtleX = playerX + (playerX - previousX);
            var turtleY = playerY + (playerY - previousY);
            var turtleValue = currentMap[turtleX, turtleY];

            // 3:カメ の移動先が 0:地面 の場合
            if (turtleValue == 0)
            {
                currentMap[previousX, previousY] = 0;
                currentMap[playerX, playerY] = 2;
                currentMap[turtleX, turtleY] = 3;
            }

            // 3:カメ の移動先が 4:水槽 の場合
            if (turtleValue == 4)
            {
                musicManager.PlaySE2();
                currentMap[previousX, previousY] = 0;
                currentMap[playerX, playerY] = 2;
                currentMap[turtleX, turtleY] = 5;
            }
        }
    }

    private int[,] CopyMap(int[,] originalMap)
    {
        int[,] newMap = new int[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                newMap[i, j] = originalMap[i, j];
            }
        }
        return newMap;
    }

    private void UpdateMap()
    {
        mapList.Add(CopyMap(currentMap));
        mapImageManager.UpdateMapImage(mapList[listKey]);
        listKey++;
    }

    public void SetPlayerPos(int x, int y)
    {
        playerX = x;
        playerY = y;
        previousX = playerX;
        previousY = playerY;
    }

    public void StartGame()
    {
        musicManager.PlaySE1();
        startPanel.SetActive(false);
        mainPanel.SetActive(true);

        currentMap = mapData.SettingMap();
        mapList.Add(CopyMap(currentMap));
        mapImageManager.SetUpMapImage(mapList[listKey], rows, cols);
        listKey++;
    }

    public void StageClear()
    {
        mainPanel.SetActive(false);
        clearPanel.SetActive(true);
        stageText.GetComponent<UnityEngine.UI.Text>().text = PlayerPrefs.GetInt("Stage").ToString();
        stageClear = true;

        if (PlayerPrefs.GetInt("Stage") == 6)
        {
            nextStageButton.SetActive(false);
        }
    }

    public void NextStage()
    {
        var nextStage = PlayerPrefs.GetInt("Stage") + 1;
        PlayerPrefs.SetInt("Stage", nextStage);
        PlayerPrefs.SetString("NextStage", "true");

        PlayerPrefs.Save();

        SceneManager.LoadScene("GameScene");
    }

    public void ReturnTitle()
    {
        musicManager.PlaySE1();
        SceneManager.LoadScene("TitleScene");
    }
}
