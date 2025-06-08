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
    [Header("오디오 믹서")]
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

    [Header("초기 음량")]
    private float _bgmVolume = 0.3f;
    private float _sfxVolume = 0.8f;

    [Header("음량")]
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

        // BGM Source 초기화
        {
            BGM_Player.clip   = BGM;
            BGM_Player.loop   = true;
            BGM_Player.volume = _bgmVolume;

            if(BGM_Player.isPlaying == false)
            {
                BGM_Player.Play();
            }
        }
        // SFX Source 초기화
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

        // 여기서 줄여도
        // 아래에서 체크하고 해제하고 하면 0으로 돌아오고 -80되고 난리나는거잖아
    }

    // true == 체크가 된 상태
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

    // true == 체크가 된 상태
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
