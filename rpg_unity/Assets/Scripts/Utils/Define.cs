using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Layer
    {
        Wall = 8,
        Block = 9,
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
    }

    public enum CameraMode
    {
        QuarterView,
    }
}
