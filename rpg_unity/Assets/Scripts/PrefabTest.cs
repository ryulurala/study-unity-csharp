using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabTest : MonoBehaviour
{
    GameObject tank;

    void Start()
    {
        tank = GameManager.Resource.Instantiate("Tank");

        Destroy(tank, 3.0f);
    }
}
