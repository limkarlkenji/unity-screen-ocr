using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptureRegion : MonoBehaviour
{
    public RawImage img;

    private bool dragSelect;

    private Vector3 selectOrigin;
    private Rect selectionRect;
    public RawImage winTex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            selectOrigin = Input.mousePosition;
        }

        if(Input.GetMouseButton(0))
        {
            dragSelect = true;
        }

        if(Input.GetMouseButtonUp(0))
        {
            if(dragSelect) // if we are selecting
            {
                StartCoroutine(Capture());
                dragSelect = false;
            }

        }
    }

    IEnumerator Capture()
    {

        yield return new WaitForEndOfFrame();

        // Texture2D screenshotTexture = new Texture2D((int)selectionRect.width, (int)selectionRect.height, TextureFormat.ARGB32, false);
        // screenshotTexture.ReadPixels(selectionRect, 0, 0);
        // screenshotTexture.Apply();

        //byte[] byteArray = screenshotTexture.EncodeToPNG();
        //System.IO.File.WriteAllBytes("C:/Users/limka/Desktop/scrs/cam.png", byteArray);

        // img.texture = screenshotTexture;
        // img.SetNativeSize();

        Texture winb = winTex.mainTexture;


// Create a temporary RenderTexture of the same size as the texture
RenderTexture tmp = RenderTexture.GetTemporary( 
                    winb.width,
                    winb.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);


// Blit the pixels on texture to the RenderTexture
Graphics.Blit(winb, tmp);


// Backup the currently set RenderTexture
RenderTexture previous = RenderTexture.active;


// Set the current RenderTexture to the temporary one we created
RenderTexture.active = tmp;


// Create a new readable Texture2D to copy the pixels to it
Texture2D myTexture2D = new Texture2D(winb.width, winb.height);


// Copy the pixels from the RenderTexture to the new Texture
myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
myTexture2D.Apply();


// Reset the active RenderTexture
RenderTexture.active = previous;


// Release the temporary RenderTexture
RenderTexture.ReleaseTemporary(tmp);


Texture2D ftex = new Texture2D((int)selectionRect.width, (int)selectionRect.height, TextureFormat.ARGB32, false);
Color[] ccc = myTexture2D.GetPixels((int)selectionRect.x, (int)selectionRect.y, (int)selectionRect.width, (int)selectionRect.height, 0);
System.Array.Reverse(ccc, 0, ccc.Length);
ftex.SetPixels(ccc, 0);
ftex.Apply();

ftex = FlipTexture(ftex);


// "myTexture2D" now has the same pixels from "texture" and it's re





        int tWidth = Screen.width;
        int tHeight = Screen.height;

        Texture2D s1 = new Texture2D(tWidth, tHeight, TextureFormat.ARGB32, false);
        //Texture2D s1 = new Texture2D((int)selectionRect.width, (int)selectionRect.height, TextureFormat.ARGB32, false);
        s1.ReadPixels(new Rect(0, 0, tWidth, tHeight), 0, 0);
        s1.Apply();

        byte[] byteArray = ftex.EncodeToPNG();
        string filePath = "C:/Users/limka/Desktop/scrs/cam.png";
        System.IO.File.WriteAllBytes(filePath, byteArray);

        yield return new WaitUntil(()=>System.IO.File.Exists(filePath));

        // byte[] c = System.IO.File.ReadAllBytes(filePath);
        // Texture2D loadTexture = new Texture2D(tWidth, tHeight, TextureFormat.ARGB32, false);
        // loadTexture.LoadRawTextureData(c);
        // loadTexture.Apply();

        Debug.Log(Screen.width + " : " + Screen.height);
        Debug.Log(winTex.mainTexture.width + " : " + winTex.mainTexture.height);
        Debug.Log(selectionRect);
        //Debug.Log(nrect);

        Tesseract.Instance.Recognize(ftex);
        TransparentWindow.Instance.AllowClickThrough(true);
        //winTex.texture = loadTexture;


    }

    
 public Texture2D FlipTexture(Texture2D original)
    {
        int textureWidth = original.width;
        int textureHeight = original.height;
    
        Color[] colorArray = original.GetPixels();
                   
        for (int j = 0; j < textureHeight; j++)
        {
            int rowStart = 0;
            int rowEnd = textureWidth - 1;
    
            while (rowStart < rowEnd)
            {
                Color hold = colorArray[(j * textureWidth) + (rowStart)];
                colorArray[(j * textureWidth) + (rowStart)] = colorArray[(j * textureWidth) + (rowEnd)];
                colorArray[(j * textureWidth) + (rowEnd)] = hold;
                rowStart++;
                rowEnd--;
            }
        }
                  
        Texture2D finalFlippedTexture = new Texture2D(original.width, original.height);
        finalFlippedTexture.SetPixels(colorArray);
        finalFlippedTexture.Apply();
    
        return finalFlippedTexture;
    }

    private void OnGUI()
    {

        if(dragSelect)
        {
            selectionRect = SelectUtils.GetScreenRect(selectOrigin, Input.mousePosition);
            SelectUtils.DrawScreenRect(selectionRect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            SelectUtils.DrawScreenRectBorder(selectionRect, 2, Color.green);
        }

    }
}
