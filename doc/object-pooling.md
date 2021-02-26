---
title: "Object Pooling"
category: Unity-Framework
tags:
  [
    unity,
    object-pooling,
    pool-manager,
    poolable,
    dontdestroyonload,
    stack,
    push,
    pop,
  ]
date: "2021-02-26"
---

## Object Pooling

- Object Pooling(오브젝트 풀링)

  > Object 생성(`Instantiate`, `Load`), 삭제(`Destroy`) 시 부하가 증가하므로 `Garbage Collector`(aka. G.C) 호출 빈도가 증가  
  > 따라서, `Object`를 미리 만들고 `SetActive(true)` / `SetActive(false)`로 재사용하여 G.C 호출 빈도를 낮추는 최적화

- Preview
  - ![object-pooling](/uploads/object-pooling/object-pooling.gif)

### Pool Manager

#### `Poolable.cs`: Component

- 해당 컴포넌트가 연결되면 `Pooling Object`임을 인지.

```cs
// 해당 컴포넌트가 있는 GameObject는 Pooling Object로 인지.
public class Poolable : MonoBehaviour
{
    // Pooling된 상태인 지
    public bool IsUsing;
}
```

#### `PoolManager.cs`: No Component

- `Pooling` 관리

##### class Pool

> Pooling Objects 관리

- `GameObject Original`
  > Original GameObject
- `Transform Root`
  > DontDestroyOnLoad 하위에서 같은 종류의 Pooling Objects의 Root
- `Stack<Poolable> poolStack`
  > Pooling Object가 들어있음
- `init()`
  > Original, Root 설정  
  > Object를 initialCount만큼 생성 후 Stack에 Push
- `Create()`
  > GameObject 실제로 생성(Instantiate)
- `Push()`
  > Object Pooling의 Destroy  
  > SetActive(false)  
  > Stack Push()
- `Pop()`
  > Object Pooling의 Instantiate  
  > SetActive(true)  
  > Stack Pop() or 남은게 없으면 Create()

```cs
class Pool
{
    public GameObject Original { get; private set; }
    public Transform Root { get; set; }

    // Poolable 컴포넌트를 사용하는 Pooling Objects
    Stack<Poolable> _poolStack = new Stack<Poolable>();

    public void init(GameObject original, int initCount)
    {
        // Original, Root 설정
        Original = original;
        Root = new GameObject().transform;
        Root.name = $"{original.name}_Root";

        // initCount만큼 미리 생성
        for (int i = 0; i < initCount; i++)
            Push(Create());
    }

    Poolable Create()
    {
        // GameObject를 실제로 생성
        GameObject go = Object.Instantiate<GameObject>(Original);
        go.name = Original.name;

        // Polable 컴포넌트를 연결해서 리턴
        return go.GetOrAddComponent<Poolable>();
    }

    public void Push(Poolable poolable)
    {
        // Poolable 컴포넌트 없으면 Pooling Object가 아니다.
        if (poolable == null)
            return;

        // SetActive: false
        poolable.transform.parent = Root;
        poolable.gameObject.SetActive(false);
        poolable.IsUsing = false;

        // Destroy
        _poolStack.Push(poolable);
    }

    public Poolable Pop(Transform parent)
    {
        Poolable poolable;

        // Instantiate
        if (_poolStack.Count > 0)
            poolable = _poolStack.Pop();    // 남은 Object가 있으면 재사용
        else
            poolable = Create();    // 없으면 만들기

        // DontDestoryOnLoad 해제 용도: 한 번은 Scene Hierarchy에 옮김
        if (parent == null)
            poolable.transform.parent = GameManager.Scene.CurrentScene.transform;

        // SetActive: true
        poolable.gameObject.SetActive(true);
        poolable.IsUsing = true;
        poolable.transform.parent = parent;

        return poolable;
    }
}
```

##### PoolManager: the others

- `Dictionary<string, Pool> pool`
  > Pool 관리  
  > Key: Original GameObject Name  
  > Value: Pool 객체(= Pooling Objects)
- `Transform root`
  > 모든 종류의 Pooling Objects의 Root transform
- `init()`
  > 모든 종류의 Pooling Objects의 Root transform 설정  
  > DontDestoryOnLoad 설정
- `CreatePool()`
  > Pooling Object로 설정
- `Push()`
  > Object Pooling의 Destroy  
  > 해당 Pool의 Stack에 Push()
- `Pop()`
  > Object Pooling의 Instantiate  
  > 해당 Pool의 Stack에서 Pop()
- `GetOriginal()`
  > Pool에 있는 Original을 리턴  
  > ResourceManager에서 Load할 때 Original GameObject 재사용.
- `Clear()`
  > 모든 Pool을 비움.  
  > Scene마다 Pooling Object 관리가 다를 수 있기 때문에

```cs
public class PoolManager
{
    #region Pool
      // class Pool{ ... }
    #endregion

    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    Transform _root;    // Pooling Objects의 제일 상위

    public void init()
    {
        // root 설정
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    public void CreatePool(GameObject original, int initCount = 5)
    {
        // Pool 생성: Pooling Object로 설정
        Pool pool = new Pool();
        pool.init(original, initCount);
        pool.Root.parent = _root;

        _pool.Add(original.name, pool);
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;
        if (_pool.ContainsKey(name) == false)
        {
            // 만약 지정된 Pooling Object가 아니면 실제로 Destroy()
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        // 재사용을 위해 SetActive: false
        _pool[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        // 혹시나 Pop하려다 pool에 없으면 생성
        if (_pool.ContainsKey(original.name) == false)
            CreatePool(original);

        // Instantiate
        return _pool[original.name].Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        // ResourceManager에서 Load할 때 Original 재사용
        if (_pool.ContainsKey(name) == false)
            return null;

        return _pool[name].Original;
    }

    public void Clear()
    {
        // Scene마다 Pooling Object 관리가 다를 경우 비움
        foreach (Transform child in _root)
            GameObject.Destroy(child.gameObject);

        _pool.Clear();
    }
}
```

#### ResourceManager의 Load()

- GameObject의 Load 부하를 줄이기 위한 Code Refactoring
- Pooling Object의 Original GameObject를 재사용.

```cs
public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            // GameObject type을 Load할 때
            string name = path;
            int index = name.LastIndexOf("/");
            if (index >= 0)
                name = name.Substring(index + 1);

            // Pooling Object의 Original GameObject를 재사용
            GameObject go = GameManager.Pool.GetOriginal(name);
            if (go != null)
                return go as T;
        }

        return Resources.Load<T>(path);
    }
}
```

---
