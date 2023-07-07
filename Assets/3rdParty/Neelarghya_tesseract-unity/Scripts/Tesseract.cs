using UnityEngine;
using UnityEngine.UI;

public class Tesseract : MonoBehaviour
{
    public static Tesseract Instance;
    [SerializeField] private Texture2D imageToRecognize;
    [SerializeField] private InputField outputText;
    [SerializeField] private RawImage outputImage;
    private TesseractDriver _tesseractDriver;
    private string _text = "";
    private Texture2D _texture;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Texture2D texture = new Texture2D(imageToRecognize.width, imageToRecognize.height, TextureFormat.ARGB32, false);
        // texture.SetPixels32(imageToRecognize.GetPixels32());
        // texture.Apply();

        _tesseractDriver = new TesseractDriver();
        //Recognize(texture);
    }

    public void Recognize(Texture2D outputTexture)
    {
        _texture = outputTexture;
        //ClearTextDisplay();
        //AddToTextDisplay(_tesseractDriver.CheckTessVersion());
        _tesseractDriver.Setup(OnSetupCompleteRecognize);
    }

    private void OnSetupCompleteRecognize()
    {
        AddToTextDisplay(_tesseractDriver.Recognize(_texture));
        //AddToTextDisplay(_tesseractDriver.GetErrorMessage(), true);
        SetImageDisplay();
    }

    private void ClearTextDisplay()
    {
        _text = "";
    }

    private void AddToTextDisplay(string text, bool isError = false)
    {
        outputText.text = text;
        if (string.IsNullOrWhiteSpace(text)) return;
        Debug.Log(text + " " + isError);
        // _text += (string.IsNullOrWhiteSpace(displayText.text) ? "" : "\n") + text;

        // if (isError)
        //     Debug.LogError(text);
        // else
        //     Debug.Log(text);
    }

    private void SetImageDisplay()
    {
        // RectTransform rectTransform = outputImage.GetComponent<RectTransform>();
        // rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
        //     rectTransform.rect.width * _tesseractDriver.GetHighlightedTexture().height / _tesseractDriver.GetHighlightedTexture().width);
        // outputImage.texture = _tesseractDriver.GetHighlightedTexture();
    }
}