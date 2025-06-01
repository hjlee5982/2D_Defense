using System.Collections.Generic;
using UnityEngine;

public class JAudioManager : MonoBehaviour
{
    #region SINGLETON
    public static JAudioManager Instance { get; private set; }

    private void SingletonInitialize()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
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
    #endregion




    #region MONOBEHAVIOUR
    private void Awake()
    {
        SingletonInitialize();
        
        // BGM Source 초기화
        {
            BGM_Player.clip   = BGM;
            BGM_Player.loop   = true;
            BGM_Player.volume = 0.1f;

            if(BGM_Player.isPlaying == false)
            {
                BGM_Player.Play();
            }
        }
        // SFX Source 초기화
        {
            SFX_Player.clip   = null;
            SFX_Player.loop   = false;
            SFX_Player.volume = 0.5f;

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
        BGM_Player.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        SFX_Player.volume = volume;
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
