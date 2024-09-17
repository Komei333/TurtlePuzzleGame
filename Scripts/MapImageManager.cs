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

                if (value == 0) continue;  // 0:�n��

                if (value == 1)  // 1:��
                {
                    Instantiate(wallPrefab, position, Quaternion.identity);
                }
                else if (value == 2)  // 2:�v���C���[
                {
                    Instantiate(playerPrefab, position, Quaternion.identity);
                    mainManager.SetPlayerPos(x, y);
                }
                else if (value == 3)  // 3:�J��
                {
                    Instantiate(turtlePrefab, position, Quaternion.identity);
                    turtleCount++;
                }
                else if (value == 4)  // 4:����
                {
                    Instantiate(tankPrefab, position, Quaternion.identity);
                }
                else if (value == 5)  // 5:�����i�J������j
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
