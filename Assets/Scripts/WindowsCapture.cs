using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Ruccho.GraphicsCapture;

public class WindowsCapture : MonoBehaviour
{
  [SerializeField]
    private RawImage previewImage = default; 

    private Capture capture = default;

    private int defaultTarget = 0;
    IEnumerable<ICaptureTarget> targets;
    
    public List<ICaptureTarget> t;

    private void Start()
    {
        previewImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
        previewImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);

        //IEnumerable<ICaptureTarget> targets = Utils.GetTargets();
        targets = Utils.GetTargets();
        //var target = targets.First();
        var target = targets.ElementAt(1);
        IEnumerable<ICaptureTarget> mo = Utils.GetMonitors();
        capture = new Capture(mo.First());

        Debug.Log(mo.First().Description);
        // foreach(var x in targets)
        // {
        //     Debug.Log(x.Description);

        // }

        capture.Start();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.RightArrow))
        {
            defaultTarget++;
            capture = new Capture(targets.ElementAtOrDefault(defaultTarget));
            Debug.Log(targets.ElementAt(defaultTarget).Description);
            capture.Start();
        }

        if(capture != null)
        {
            //Call GetTexture() every frame to update Unity's texture from native texture.
            previewImage.texture = capture.GetTexture();
        }
    }

    private void GetWindows()
    {
        // IEnumerable t = Utils.get
        // for(int i = 0; i < Utils.ge))
        // {
        //     if(targets.ElementAt())
        // }
    }

    private void OnDestroy()
    {
        //Don't forget to stop capturing manually.
        capture?.Dispose();
    }
}
