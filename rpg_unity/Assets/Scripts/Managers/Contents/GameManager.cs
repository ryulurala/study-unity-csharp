using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    // int <--> GameObject: Online
    // Dictionary<int, GameObject> _monsters = new Dictionary<int, GameObject>();
    // Dictionary<int, GameObject> _players = new Dictionary<int, GameObject>();

    GameObject _player;
    HashSet<GameObject> _monsters = new HashSet<GameObject>();

    // int: 늘어난 숫자
    public Action<int> OnSpawnEvent;

    public GameObject Player { get { return _player; } }

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = Manager.Resource.Instantiate(path, parent);
        switch (type)
        {
            case Define.WorldObject.Monster:
                _monsters.Add(go);
                if (OnSpawnEvent != null)
                    OnSpawnEvent.Invoke(1);
                break;
            case Define.WorldObject.Player:
                _player = go;
                break;
        }
        return go;
    }

    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();
        if (bc == null)
            return Define.WorldObject.Unknown;

        return bc.WorldObjectType;
    }

    public void Despawn(GameObject go)
    {
        Define.WorldObject type = GetWorldObjectType(go);

        switch (type)
        {
            case Define.WorldObject.Monster:
                if (_monsters.Contains(go))
                {
                    _monsters.Remove(go);
                    if (OnSpawnEvent != null)
                        OnSpawnEvent.Invoke(-1);
                }
                break;
            case Define.WorldObject.Player:
                if (_player == go)
                    _player = null;
                break;
        }

        Manager.Resource.Destroy(go);
    }
}
