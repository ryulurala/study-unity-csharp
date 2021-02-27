---
title: "Data Managing"
category: Unity-Framework
tags: [unity, data, data-manage, json, xml]
date: "2021-02-27"
---

## Data Managing

- 게임에 존재하는 모든 고정 수치를 Data 파일로 관리

  > 데이터 초기화가 코드 내에 있을 경우, 매번 수정할 때마다 재빌드해야 한다.  
  > ex) HP, MP, EXP 등

- 웹 통신을 통해 같은 Data 파일 `.json` or `.xml`을 Parsing하여 고정 수치를 Load

  - `.json`

    ```json
    {
      "stats": [
        {
          "level": "1",
          "hp": "100",
          "attack": "10"
        },
        {
          "level": "2",
          "hp": "150",
          "attack": "15"
        },
        {
          "level": "3",
          "hp": "200",
          "attack": "20"
        }
      ]
    }
    ```

  - `.xml`

    ```xml
    <data>
      <stats>
        <stat>
          <level>1</level>
          <hp>100</hp>
          <attack>10</attack>
        </stat>
        <stat>
          <level>2</level>
          <hp>150</hp>
          <attack>15</attack>
        </stat>
        <stat>
          <level>3</level>
          <hp>200</hp>
          <attack>0</attack>
        </stat>
      </stats>
    </data>
    ```

### Data Manager

- `ILoader<Key, Value>`

  > 모든 Data의 인터페이스  
  > Data를 `<Key, Value>`로 읽어들여 사용

  - `MakeDict()`
    > `Dictionary<>`를 초기화한다.

- DataManager
  - `Dictionary<>` N 개: Data마다
  - `Init()`
    > N 개의 `Dictionary<>` 초기화
  - `LoadJson()`
    > `.json` 파일에서 Text로 읽기  
    > JSON을 Parsing해서 Loader type으로 읽기

```cs
// Data File의 인터페이스
public interface ILoader<Key, Value>
{
    // Dictionary<> 만드는 함수
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    // Dictionary <> ...Dict

    public void Init()
    {
        // Dictionary<> 초기화
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        // .json 파일에서 Text 파일로 읽어들인다.
        TextAsset textAsset = GameManager.Resource.Load<TextAsset>($"Data/{path}");

        // 인게임 내에서 Load로 Json 파일을 읽어들인다.
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}

```

### Data Contents

- [Serializable] 필요
  > JsonUtility.FromJson 사용 가능
- 반드시, Packet의 멤버 변수는 public or [SerializeField] 설정
- Name, Type도 동일

```cs
#region Stat

[Serializable]
public class Stat
{
    public int level;
    public int hp;
    public int attack;
}

[Serializable]
public class StatData : ILoader<int, Stat>
{
    public List<Stat> stats = new List<Stat>();

    public Dictionary<int, Stat> MakeDict()
    {
        Dictionary<int, Stat> dict = new Dictionary<int, Stat>();
        foreach (Stat stat in stats)
            dict.Add(stat.level, stat);

        return dict;
    }
}

#endregion
```

---
