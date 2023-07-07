using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public void Capture()
    {
        TransparentWindow.Instance.AllowClickThrough(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

}
