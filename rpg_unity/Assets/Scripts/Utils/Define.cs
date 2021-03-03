using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum State
    {
        Idle,
        Moving,
        Die,
        Attack,
    }
    public enum Scene
    {
        UnKnown,    // default
        Login,
        Lobby,
        Game,
    }
    public enum Sound
    {
        BGM,
        SFX,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag,

    }
    public enum MouseEvent
    {
        Press,  // ex. 디아블로
        Click,  // ex. LOL
        PointDown,
        PointUp,
    }

    public enum CameraMode
    {
        QuarterView,
    }
}
