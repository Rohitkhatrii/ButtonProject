using UnityEngine;
using TMPro;

public class keypadController : MonoBehaviour
{
    public TMP_Text displayText;
    private string currentInput = "";

    public void onNumberPressed(string number)
    {
        currentInput += number;
        UpdateDisplay();
        
    }
    public void onClearPressed()
    {
        currentInput = "";
        UpdateDisplay();
    }
    public void onEnterPressed()
    {
        Debug.Log("Code Entered:" + currentInput);

        currentInput = "";
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if (string.IsNullOrEmpty(currentInput))
        {
            displayText.text = "--";
        }
        else
        {
            displayText.text = currentInput;
        }
    }

}
