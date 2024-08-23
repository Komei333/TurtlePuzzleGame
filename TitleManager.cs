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
    [SerializeField] GameObject scorePanel;

    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider seSlider;

    private float bgmValue;
    private float seValue;

    void Start()
    {
        Screen.SetResolution(1280, 1000, FullScreenMode.Windowed, 60);

        // ����v���C���ɐݒ��������
        if (PlayerPrefs.GetString("First") != "false")
        {
            // �ۑ�
            PlayerPrefs.SetFloat("BGM", 1.0f);
            PlayerPrefs.SetFloat("SE", 1.0f);
            PlayerPrefs.SetInt("Stage", 1);
            PlayerPrefs.SetString("NextStage", "false");
            PlayerPrefs.SetString("First", "false");
            PlayerPrefs.Save();
        }

        //BGM�X���C�_�[�𓮂��������̏�����o�^
        bgmSlider.onValueChanged.AddListener(SetBGM);

        //SE�X���C�_�[�𓮂��������̏�����o�^
        seSlider.onValueChanged.AddListener(SetSE);

        // �X���C�_�[�ɒl�𔽉f
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
            //-30�`0�ɕϊ��i���Ηʂ�dB�ɕϊ��j
            bgmValue = -30f + (value * 30f);
        }

        //�ۑ�
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
            //-30�`0�ɕϊ��i���Ηʂ�dB�ɕϊ��j
            seValue = -30f + (value * 30f);
        }

        //�ۑ�
        PlayerPrefs.SetFloat("SE", seValue);
        PlayerPrefs.Save();

        musicManager.SetSE();
    }

    public void StartStage1()
    {
        PlayerPrefs.SetInt("Stage", 1);
        StartGame();
    }
    public void StartStage2()
    {
        PlayerPrefs.SetInt("Stage", 2);
        StartGame();
    }
    public void StartStage3()
    {
        PlayerPrefs.SetInt("Stage", 3);
        StartGame();
    }
    public void StartStage4()
    {
        PlayerPrefs.SetInt("Stage", 4);
        StartGame();
    }
    public void StartStage5()
    {
        PlayerPrefs.SetInt("Stage", 5);
        StartGame();
    }
    public void StartStage6()
    {
        PlayerPrefs.SetInt("Stage", 6);
        StartGame();
    }

    private void StartGame()
    {
        musicManager.PlaySE1();
        PlayerPrefs.Save();
        SceneManager.LoadScene("GameScene");
    }
}
