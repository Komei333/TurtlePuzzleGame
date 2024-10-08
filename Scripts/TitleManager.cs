using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Unity.Burst.Intrinsics.X86;

public class TitleManager : MonoBehaviour
{
    [SerializeField] MusicManager musicManager;

    [SerializeField] GameObject titlePanel;

    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider seSlider;

    private float bgmValue;
    private float seValue;

    void Start()
    {
        // 1280×1000, 60Hz
        Screen.SetResolution(1280, 1000, FullScreenMode.Windowed, 60);

        //BGMスライダーを動かした時の処理を登録
        bgmSlider.onValueChanged.AddListener(SetBGM);

        //SEスライダーを動かした時の処理を登録
        seSlider.onValueChanged.AddListener(SetSE);

        // スライダーに値を反映
        bgmSlider.value = (PlayerPrefs.GetFloat("BGM") + 30f) / 30f;
        seSlider.value = (PlayerPrefs.GetFloat("SE") + 30f) / 30f;
    }

    public void SetBGM(float value)
    {
        if (value == 0)
        {
            bgmValue = -999f;
        }
        else
        {
            //-30〜0に変換（相対量をdBに変換）
            bgmValue = -30f + (value * 30f);
        }

        //保存
        PlayerPrefs.SetFloat("BGM", bgmValue);
        PlayerPrefs.Save();

        musicManager.SetBGM();
    }

    public void SetSE(float value)
    {
        if (value == 0)
        {
            seValue = -999f;
        }
        else
        {
            //-30〜0に変換（相対量をdBに変換）
            seValue = -30f + (value * 30f);
        }

        //保存
        PlayerPrefs.SetFloat("SE", seValue);
        PlayerPrefs.Save();

        musicManager.SetSE();
    }

    public void StartStage1()
    {
        PlayerPrefs.SetInt("StageID", 1);
        StartGame();
    }
    public void StartStage2()
    {
        PlayerPrefs.SetInt("StageID", 2);
        StartGame();
    }
    public void StartStage3()
    {
        PlayerPrefs.SetInt("StageID", 3);
        StartGame();
    }
    public void StartStage4()
    {
        PlayerPrefs.SetInt("StageID", 4);
        StartGame();
    }
    public void StartStage5()
    {
        PlayerPrefs.SetInt("StageID", 5);
        StartGame();
    }
    public void StartStage6()
    {
        PlayerPrefs.SetInt("StageID", 6);
        StartGame();
    }

    private void StartGame()
    {
        musicManager.PlaySE1();
        PlayerPrefs.Save();
        SceneManager.LoadScene("GameScene");
    }
}
