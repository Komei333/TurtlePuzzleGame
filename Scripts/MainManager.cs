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
    private int mapListKey = 0;
    private int[,] currentMap;

    private int rows = 13; // �s��
    private int cols = 9; // ��

    private int playerX = 0;
    private int playerY = 0;
    private int previousX = 0;
    private int previousY = 0;

    private bool isUsingKeyboard = false;
    private bool isStageCleared = false;

    private void Start()
    {
        // �u���̃X�e�[�W�v�{�^�����������Ƃ��́A���[�������摜��\�����Ȃ�
        if (PlayerPrefs.GetString("NextStage") == "true")
        {
            StartGame();
            PlayerPrefs.SetString("NextStage", "false");
            PlayerPrefs.Save();
        }
    }

    void Update()
    {
        if (isStageCleared) return;

        CheckUsingKeyboard();
        if (isUsingKeyboard == false) return;

        MovePlayer();
        UpdateMap();
    }

    private void CheckUsingKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.T))  // �^�C�g���ɖ߂�
        {
            SceneManager.LoadScene("TitleScene");
            return;
        }

        if (Input.GetKeyDown(KeyCode.Z))  // 1�߂�
        {
            if (mapListKey > 1)
            {
                mapListKey--;
                mapList.RemoveAt(mapListKey);
                currentMap = CopyMap(mapList[mapListKey - 1]);
                mapImageManager.UpdateMapImage(currentMap);
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.R))  // ���Z�b�g����
        {
            mapList.Clear();

            mapListKey = 0;
            currentMap = mapData.SettingMap();
            UpdateMap();

            return;
        }

        isUsingKeyboard = false;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            playerY++;  // ��
            isUsingKeyboard = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            playerX--;  // ��
            isUsingKeyboard = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            playerY--;  // ��
            isUsingKeyboard = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            playerX++;  // �E
            isUsingKeyboard = true;
        }
    }

    private void MovePlayer()
    {
        var playerValue = currentMap[playerX, playerY];

        // 2:�v���C���[ �̈ړ��悪 0:�n�� �̏ꍇ
        if (playerValue == 0)
        {
            currentMap[previousX, previousY] = 0;
            currentMap[playerX, playerY] = 2;
            return;
        }

        // 2:�v���C���[ �̈ړ��悪 3:�J�� �̏ꍇ
        if (playerValue == 3)
        {
            var turtleX = playerX + (playerX - previousX);
            var turtleY = playerY + (playerY - previousY);
            var turtleValue = currentMap[turtleX, turtleY];

            // 3:�J�� �̈ړ��悪 0:�n�� �̏ꍇ
            if (turtleValue == 0)
            {
                currentMap[previousX, previousY] = 0;
                currentMap[playerX, playerY] = 2;
                currentMap[turtleX, turtleY] = 3;
            }

            // 3:�J�� �̈ړ��悪 4:���� �̏ꍇ
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
        mapImageManager.UpdateMapImage(mapList[mapListKey]);
        mapListKey++;
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
        mapImageManager.SetUpMapImage(mapList[mapListKey], rows, cols);
        mapListKey++;
    }

    public void StageClear()
    {
        mainPanel.SetActive(false);
        clearPanel.SetActive(true);
        stageText.GetComponent<UnityEngine.UI.Text>().text = PlayerPrefs.GetInt("StageID").ToString();
        isStageCleared = true;

        if (PlayerPrefs.GetInt("StageID") == 6)
        {
            nextStageButton.SetActive(false);
        }
    }

    public void NextStage()
    {
        var nextStageID = PlayerPrefs.GetInt("StageID") + 1;
        PlayerPrefs.SetInt("StageID", nextStageID);
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
