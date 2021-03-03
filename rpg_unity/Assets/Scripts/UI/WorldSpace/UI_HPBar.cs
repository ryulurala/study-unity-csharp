using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    void Update()
    {
        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * parent.GetComponent<Collider>().bounds.size.y;
        transform.rotation = Camera.main.transform.rotation;

        float ratio = (float)_stat.Hp / _stat.MaxHp;
        SetRatio(ratio);
    }

    Stat _stat;

    enum GameObjects
    {
        HPBar
    }

    public override void init()
    {
        Bind<GameObject>(typeof(GameObjects));
        _stat = transform.parent.GetComponent<Stat>();
    }

    public void SetRatio(float ratio)
    {
        GetObject((int)GameObjects.HPBar).GetComponent<Slider>().value = ratio;
    }
}
