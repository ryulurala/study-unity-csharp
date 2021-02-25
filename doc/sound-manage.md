---
title: "Sound Managing"
category: Unity-Framework
tags:
  [
    unity,
    sound,
    sound-manager,
    audio-listener,
    audio-source,
    audio-clip,
    play,
    play-one-shot,
    play-clip-at-point,
    3d-sound,
  ]
date: "2021-02-25"
---

## Sound Managing

- `Unity`의 Sound 컴포넌트

|                       `AudioListener`                        |                      `AudioSource`                       |                      `AudioClip`                       |
| :----------------------------------------------------------: | :------------------------------------------------------: | :----------------------------------------------------: |
| ![audio-listener](/uploads/sound-manager/audio-listener.png) | ![audio-source](/uploads/sound-manager/audio-source.png) | ![audio-clips](/uploads/sound-manager/audio-clips.png) |
|                         = 사람의 귀                          |                       = MP3 Player                       |                   = 음원(음악 자체)                    |
|                    `Scene`에 하나만 존재                     |                    음원을 Play, Stop                     |                  BGM: `.mp3`, `.ogg`                   |
|              대부분 `Player` or `Camera`에 부착              |                    2D / 3D Sound 설정                    |                      SFX: `.wav`                       |
|                                                              |       Volume(음량), Pitch(음 높이, 재생속도?) 설정       |                                                        |
|                                                              |                                                          |                                                        |

### Sound Manager

- Sound를 관리

  - init()
    > AudioSource 배열 초기화
  - GetOrAddAudioClip()
    > AudioClip을 path로 Load  
    > Dictionary를 이용한 캐싱
  - Play()
    > Path or AudioClip으로 AudioClip을 실행
  - Clear()
    > Scene이 이동할 때 메모리 초기화

- 필드 변수

```cs
public class SoundManager
{
    // AudioSources: 각 AudioClip의 type에 따라 실행하는 컴포넌트 모음
    // init()할 때 모두 설정
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];

    // 해당 Scene의 AudioClip Caching을 위해서
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
}
```

#### init()

- AudioSource 배열 초기화

```cs
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
```

#### GetOrAddAudioClip()

- AudioClip을 path로 Load
- Dictionary를 이용한 캐싱

```cs
AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.SFX)
{
    // "Sounds/" path가 없을 경우
    if (path.Contains("Sounds/") == false)
        path = $"Sounds/{path}";

    AudioClip audioClip = null;
    if (type == Define.Sound.BGM)
    {
        // 배경음일 때
        audioClip = GameManager.Resource.Load<AudioClip>(path);
    }
    else
    {
        // 효과음일 때, 효과음 개수는 많으니 성능을 위해서 Caching
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
```

#### Play()

- Path or AudioClip으로 AudioClip을 실행

```cs
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
```

#### Clear()

- Scene이 이동할 때 메모리 초기화

```cs
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
```

### 3D Sound Settings

|           1. AudioSource에서 Spatial Blend 설정            |                     2. 3D Sound Settings 설정                      | 3. `AudioSource.PlayClipAtPoint(AudioClip, Vector3)` 이용 |
| :--------------------------------------------------------: | :----------------------------------------------------------------: | :-------------------------------------------------------: |
| ![spatial-blend](/uploads/sound-manager/spatial-blend.png) | ![3d-sound-settings](/uploads/sound-manager/3d-sound-settings.png) | AudioClip과 Position(Vector3)으로 해당 지점에서 `Play()`  |

---
