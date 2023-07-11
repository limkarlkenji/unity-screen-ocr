using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private InputField result;

    public static UIHandler Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void Capture()
    {
        DisplayResult("null", false); // Hide result if it's active
        TransparentWindow.Instance.AllowClickThrough(false);
    }

    public void DisplayResult(string text, bool show)
    {
        result.gameObject.SetActive(show);
        result.text = text;
    }

    // Referenced via canvas
    public void Exit()
    {
        Application.Quit();
    }

}
