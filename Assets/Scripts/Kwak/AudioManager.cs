using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource backgroundMusic;

    // 배경음악을 재생하는 메서드
    public void PlayBackgroundMusic()
    {
        backgroundMusic.Play();
    }

    // 배경음악을 일시정지하는 메서드
    public void PauseBackgroundMusic()
    {
        backgroundMusic.Pause();
    }

    // 배경음악을 정지하는 메서드
    public void StopBackgroundMusic()
    {
        backgroundMusic.Stop();
    }
}
