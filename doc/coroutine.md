---
title: "Coroutine"
category: Unity-Framework
tags: [unity, coroutine, yield return, yield break, ienumerator, c#]
date: "2021-02-27"
---

## Coroutine

- `Coroutine`은 `C#` 문법
- 복잡한 구문 or 엄청 오래 걸리는 작업을 Frame을 나눠 실행할 때 사용한다.
- 함수 상태를 저장(Save) / 복원(Restore) 가능
- 함수를 일시 정지 / 시작 / 종료 가능

  - 일시정지: `yield return` (`System.Object type`);
    > System.Object type으로 return 되므로 어떤 type이든지 가능
  - 시작: Frame or Time 양보 후 자동 실행
  - 종료: `yield break;`

### Coroutine 사용 예시

- Client가 Create Item 경우

```cs
IEnumerator CreateItem()
{
    // 아이템 만들기
    // DB 저장

    // yield return: 프레임 양보, DB 저장될 때까지

    // 다음 로직
}
```

### State Saved & Restore

- 일반적인 상태 저장

  > 전역 변수 필요

  ```cs
  public class CoroutineTest
  {
      void Start()
      {
          StateSaved();
      }

      // 상태 저장을 위해 전역 변수 할당
      int i = 0;
      void StateSaved()
      {
          for (; i < 10000; i++)
          {
              // 500으로 나누어 떨어질 때만 Logging
              if (i % 500 == 0)
                  Debug.Log(i);
          }
      }
  }
  ```

- Coroutine 사용 [O]

  > Frame 양보하여 비동기적 실행

  ```cs
  public class CoroutineTest
  {
      void Start()
      {
          StartCoroutine("StateSaved");
          // StopCoroutine("StateSaved"); // Coroutine 강제 종료
      }

      IEnumerator StateSaved()
      {
          for (int i = 0; i < 10000; i++)
          {
              yield return null;

              // 1 Frame 양보 후, 상태는 계속 그대로
              if (i % 500 == 0)
                  Debug.Log(i);
          }
      }
  }
  ```

### Wait for seconds

- 일반적인 Frame마다 Time Check
  > Frame마다 계속 실행해야 하므로 성능 낮음.

```cs
public class CoroutineTest
{
    // Frame마다 검사해야 함.
    void Update()
    {
        WaitSeconds(5.0f);
    }

    // 상태 저장을 위해 전역 변수
    float deltaTime = 0;
    void WaitSeconds(float seconds)
    {
        deltaTime += Time.deltaTime;
        if(deltaTime == seconds)
        {
            Debug.Log($"{seconds}초 후 실행");
        }
    }
}
```

- Coroutine으로 Time Check
  > 한 번만 호출하므로 성능 높음.

```cs
public class CoroutineTest
{
    void Start()
    {
        StartCoroutine("WaitSeconds", 5.0f);
    }

    IEnumerator StateSaved(float seconds)
    {
        yield return new WaitForSecondss(seconds);

        // seconds 초 후 실행
        Debug.Log($"{second}초 후 실행");
    }
}
```

---
