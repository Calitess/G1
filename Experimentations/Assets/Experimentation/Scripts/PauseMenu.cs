using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private AudioMixerGroup masterMixer, voicelineMixer, musicMixer, sfxMixer, footstepsMixer;
    [SerializeField] private Slider masterSlider,voicelineSLider,musicSlider,sfxSlider;

    private void Start()
    {
        SetMusicVolume();
    }

    public void SetMusicVolume()
    {
        float masterVolume = masterSlider.value;
        masterMixer.audioMixer.SetFloat("MasterVolume",Mathf.Log10(masterVolume) *20);

        float voicelineVolume = voicelineSLider.value;
        voicelineMixer.audioMixer.SetFloat("VoicelinesVolume", Mathf.Log10(voicelineVolume) * 20);

        float musicVolume = musicSlider.value;
        musicMixer.audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);

        float sfxVolume = sfxSlider.value;
        footstepsMixer.audioMixer.SetFloat("FootstepsVolume", Mathf.Log10(sfxVolume) * 20);
        sfxMixer.audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
    }

    public void resumeGame()
    {
        GameManager.Instance.UpdateGameState(GameState.Play);
        
    }
}
