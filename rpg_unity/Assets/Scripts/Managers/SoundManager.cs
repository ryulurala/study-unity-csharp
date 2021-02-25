using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    public void init()
    {
        // root gameObject
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            // root gameObject가 없으면 생성
            root = new GameObject { name = "@Sound" };

            // Sound는 계속 존재.
            Object.DontDestroyOnLoad(root);

            // C# Reflection 이용해서 Sound name 모음
            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));
            for (int i = 0; i < soundNames.Length - 1; i++)     // maxCount 제외
            {
                // new AudioSource()가 불가능 하므로 GameObject 생성
                GameObject go = new GameObject { name = soundNames[i] };

                // _audioSource에 추가
                _audioSources[i] = go.AddComponent<AudioSource>();

                // root gameobject를 parent로 설정
                go.transform.parent = root.transform;
            }

            // BGM은 Loop로 실행
            _audioSources[(int)Define.Sound.BGM].loop = true;
        }
    }

    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.SFX, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        if (type == Define.Sound.BGM)
        {
            // 배경음일 때
            AudioSource audioSource = _audioSources[(int)Define.Sound.BGM];

            // Play 중인 음악 Stop
            if (audioSource.isPlaying)
                audioSource.Stop();

            // 재생속도, Clip 설정
            audioSource.pitch = pitch;
            audioSource.clip = audioClip;

            // Play(Loop)
            audioSource.Play();
        }
        else
        {
            // 효과음일 때
            AudioSource audioSource = _audioSources[(int)Define.Sound.SFX];

            audioSource.pitch = pitch;

            // PlayOneShot(): 한 번만 Play
            audioSource.PlayOneShot(audioClip);
        }
    }

    public void Play(string path, Define.Sound type = Define.Sound.SFX, float pitch = 1.0f)
    {
        // Load Audio Clip
        AudioClip audioClip = GetOrAddAudioClip(path, type);

        // Play
        Play(audioClip, type, pitch);
    }

    AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.SFX)
    {
        // "Sounds/" path가 없을 경우
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;
        if (type == Define.Sound.BGM)
        {
            audioClip = GameManager.Resource.Load<AudioClip>(path);
        }
        else
        {
            // Caching
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                // 캐싱해도 없을 경우
                audioClip = GameManager.Resource.Load<AudioClip>(path);

                // Dictionary에 추가
                _audioClips.Add(path, audioClip);
            }
        }
        if (audioClip == null)
            Debug.Log($"AudioClip Mission ! {path}");

        return audioClip;
    }

    // Scene이 이동할 때 Clear() 호출
    public void Clear()
    {
        // AudioSource의 Clip 모두 null
        // 진행하던 Player 중지(Stop)
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }

        // Scene마다 모든 AudioClip을 들고 있으면 메모리가 초과되므로
        _audioClips.Clear();
    }
}
