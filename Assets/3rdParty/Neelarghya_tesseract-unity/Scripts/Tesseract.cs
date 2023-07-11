using UnityEngine;
using UnityEngine.UI;

public class Tesseract : MonoBehaviour
{
    public static Tesseract Instance;
    private TesseractDriver _tesseractDriver;
    private string _text = "";
    private Texture2D _texture;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _tesseractDriver = new TesseractDriver();
    }

    public void Recognize(Texture2D outputTexture)
    {
        _texture = outputTexture;
        _tesseractDriver.Setup(OnSetupCompleteRecognize);
    }

    private void OnSetupCompleteRecognize()
    {
        AddToTextDisplay(_tesseractDriver.Recognize(_texture));
    }

    private void ClearTextDisplay()
    {
        _text = "";
    }

    private void AddToTextDisplay(string text, bool isError = false)
    {
        UIHandler.Instance.DisplayResult(text, true);
        if (string.IsNullOrWhiteSpace(text)) return;
    }
}