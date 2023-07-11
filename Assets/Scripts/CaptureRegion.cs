using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CaptureRegion : MonoBehaviour
{

    private bool dragSelect;        // Are we dragging?

    private Vector3 selectOrigin;   // Origin when selecting
    private Rect selectionRect;     // Rect created from dragging
    [SerializeField] private RawImage winTex;         // This is the current monitor texture

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


        Texture winb = winTex.mainTexture;                                   // Take the current monitor texture

        // Create a temporary RenderTexture of the same size as the texture
        RenderTexture tmp = RenderTexture.GetTemporary( 
                            winb.width,
                            winb.height,
                            0,
                            RenderTextureFormat.Default,
                            RenderTextureReadWrite.Linear);

        Graphics.Blit(winb, tmp);                                           // Blit the pixels on texture to the RenderTexture
        RenderTexture previous = RenderTexture.active;                      // Backup the currently set RenderTexture
        RenderTexture.active = tmp;                                         // Set the current RenderTexture to the temporary one we created
        Texture2D myTexture2D = new Texture2D(winb.width, winb.height);     // Create a new readable Texture2D to copy the pixels to it


        // Copy the pixels from the RenderTexture to the new Texture
        myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
        myTexture2D.Apply();
        
        RenderTexture.active = previous;                                    // Reset the active RenderTexture
        RenderTexture.ReleaseTemporary(tmp);                                // Release the temporary RenderTexture


        Texture2D ftex = new Texture2D((int)selectionRect.width, (int)selectionRect.height, TextureFormat.ARGB32, false);                           // Create a new texture using the selection rect
        Color[] ccc = myTexture2D.GetPixels((int)selectionRect.x, (int)selectionRect.y, (int)selectionRect.width, (int)selectionRect.height, 0);    // Get the pixels from the render texture we jsut created
        System.Array.Reverse(ccc, 0, ccc.Length);                                                                                                   // Flip pixels vertically
        ftex.SetPixels(ccc, 0);
        ftex.Apply();

        ftex = FlipTexture(ftex);

        // We save the captured image to disk (This is just for testing, we don't need to do this)
        Texture2D s1 = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        s1.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        s1.Apply();

        byte[] byteArray = ftex.EncodeToPNG();
        string filePath = "C:/Users/limka/Desktop/scrs/cam.png";
        System.IO.File.WriteAllBytes(filePath, byteArray);

        yield return new WaitUntil(()=>System.IO.File.Exists(filePath));

        // We pass the captured texture to tesseract for processing
        Tesseract.Instance.Recognize(ftex);

        TransparentWindow.Instance.AllowClickThrough(true);     // Allow clickthrough after
    }

    
    // Flips texture pixels horizontally
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
