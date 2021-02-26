using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    #region Pool
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
