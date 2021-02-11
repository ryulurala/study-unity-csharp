using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager gm = GameManager.Instance;  // property로 가져옴
        if (gm == null) Debug.Log("null");
        else Debug.Log("not null");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
