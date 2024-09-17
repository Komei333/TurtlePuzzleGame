using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapImageManager : MonoBehaviour
{
    [SerializeField] MainManager mainManager;

    [SerializeField] GameObject wallPrefab;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject turtlePrefab;
    [SerializeField] GameObject tankPrefab;
    [SerializeField] GameObject turtleTankPrefab;

    private int row = 0;
    private int col = 0;

    public void SetUpMapImage(int[,] map, int rows, int cols)
    {
        row = rows;
        col = cols;

        DisplayMapImage(map);
    }

    public void UpdateMapImage(int[,] map)
    {
        GameObject[] prefabs = GameObject.FindGameObjectsWithTag("Prefab");
        foreach (GameObject prefab in prefabs)
        {
            Destroy(prefab);
        }

        DisplayMapImage(map);
    }

    private void DisplayMapImage(int[,] map)
    {
        int turtleCount = 0;

        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < col; y++)
            {
                var value = map[x, y];
                Vector3 position = new Vector3(x, y, 0);

                if (value == 0) continue;  // 0:地面

                if (value == 1)  // 1:壁
                {
                    Instantiate(wallPrefab, position, Quaternion.identity);
                }
                else if (value == 2)  // 2:プレイヤー
                {
                    Instantiate(playerPrefab, position, Quaternion.identity);
                    mainManager.SetPlayerPos(x, y);
                }
                else if (value == 3)  // 3:カメ
                {
                    Instantiate(turtlePrefab, position, Quaternion.identity);
                    turtleCount++;
                }
                else if (value == 4)  // 4:水槽
                {
                    Instantiate(tankPrefab, position, Quaternion.identity);
                }
                else if (value == 5)  // 5:水槽（カメ入り）
                {
                    Instantiate(turtleTankPrefab, position, Quaternion.identity);
                }
            }
        }

        if (turtleCount == 0)
        {
            mainManager.StageClear();
        }
    }
}
