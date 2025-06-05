using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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
    [Space(10)]
    [Header("BGM")]
    public AudioSource BGM_Player;
    public AudioClip   BGM;

    [Space(20)]
    [Header("SFX")]
    public  AudioSource                   SFX_Player;
    public  List<AudioClip>               SFXs;
    private Dictionary<string, AudioClip> SFXsDict = new Dictionary<string, AudioClip>();
    public  AudioMixer                    SFX_Mixer;

    [Header("�⺻ ����")]
    private float _bgmVolume = 0.1f;
    private float _sfxVolume = 0.5f;
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
        BGM_Player.volume = (volume - 0.5f) * 0.2f + 0.1f;
    }

    // true == üũ�� �� ����
    public void ToggleBGM(bool value)
    {
        if (value == true)
        {
            BGM_Player.Pause();
        }
        else
        {
            BGM_Player.UnPause();
        }
    }

    public void SetSFXVolume(float volume)
    {
        SFX_Player.volume = volume;
    }

    // true == üũ�� �� ����
    public void ToggleSFX(bool value)
    {
        if (value == true)
        {
            SFX_Mixer.SetFloat("SFX", -80f);
        }
        else
        {
            SFX_Mixer.SetFloat("SFX", 0f);
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
