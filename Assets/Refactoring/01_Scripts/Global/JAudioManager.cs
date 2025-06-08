using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class JAudioManager : MonoBehaviour
{
    #region SINGLETON
    public static JAudioManager Instance { get; private set; }

    private bool SingletonInitialize()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return true;
        }
        else
        {
            Destroy(gameObject);
            return false;
        }
    }
    #endregion





    #region VARIABLES
    [Header("����� �ͼ�")]
    public AudioMixer AudioMixer;

    [Space(10)]
    [Header("BGM")]
    public AudioSource BGM_Player;
    public AudioClip   BGM;

    [Space(20)]
    [Header("SFX")]
    public  AudioSource                   SFX_Player;
    public  List<AudioClip>               SFXs;
    private Dictionary<string, AudioClip> SFXsDict = new Dictionary<string, AudioClip>();

    [Header("�ʱ� ����")]
    private float _bgmVolume = 0.3f;
    private float _sfxVolume = 0.8f;

    [Header("����")]
    private float _currentBGMVolume;
    private float _currentSFXVolume;
    #endregion




    #region MONOBEHAVIOUR
    private void Awake()
    {
        if(!SingletonInitialize())
        {
            return;
        }

        // BGM Source �ʱ�ȭ
        {
            BGM_Player.clip   = BGM;
            BGM_Player.loop   = true;
            BGM_Player.volume = _bgmVolume;

            if(BGM_Player.isPlaying == false)
            {
                BGM_Player.Play();
            }
        }
        // SFX Source �ʱ�ȭ
        {
            SFX_Player.clip   = null;
            SFX_Player.loop   = false;
            SFX_Player.volume = _sfxVolume;

            foreach(AudioClip clip in SFXs)
            {
                if(SFXsDict.ContainsKey(clip.name) == false)
                {
                    SFXsDict.Add(clip.name, clip);
                }
            }
        }
    }
    #endregion





    #region FUNCTIONS
    public void SetBGMVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        AudioMixer.SetFloat("BGM", dB);

        // ���⼭ �ٿ���
        // �Ʒ����� üũ�ϰ� �����ϰ� �ϸ� 0���� ���ƿ��� -80�ǰ� �������°��ݾ�
    }

    // true == üũ�� �� ����
    public void ToggleBGM(bool value)
    {
        if (value == true)
        {
            if(AudioMixer.GetFloat("BGM", out float bgmVolume) == true)
            {
                _currentBGMVolume = bgmVolume;
            }
            AudioMixer.SetFloat("BGM", -80f);
        }
        else
        {
            AudioMixer.SetFloat("BGM", _currentBGMVolume);
        }
    }

    public void SetSFXVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        AudioMixer.SetFloat("SFX", dB);
    }

    // true == üũ�� �� ����
    public void ToggleSFX(bool value)
    {
        if (value == true)
        {
            if (AudioMixer.GetFloat("SFX", out float sfxVolume) == true)
            {
                _currentSFXVolume = sfxVolume;
            }
            AudioMixer.SetFloat("SFX", -80f);
        }
        else
        {
            AudioMixer.SetFloat("SFX", _currentSFXVolume);
        }
    }

    public void PlaySFX(string name)
    {
        if(SFXsDict.TryGetValue(name, out AudioClip clip) == true)
        {
            SFX_Player.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"[JAudioManager] SFX ' {name} ' not found.");
        }
    }
    #endregion
}
