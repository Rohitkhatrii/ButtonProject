using UnityEngine;
using TMPro;



/// <summary>
/// Handles the UI keypad input and physical keyboard input, updating the display text.
/// </summary>


public class KeypadController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text displayText;
    
    [Header("Game References")]
    [SerializeField] private PuzzleManager puzzleManager;

    private string currentInput = "";

    private void Update()
    {
        HandleKeyboardInput();
    }

    
    /// Processes physical keyboard presses to mirror the UI buttons.
    
    private void HandleKeyboardInput()
    {
        // Numbers
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i) || Input.GetKeyDown(KeyCode.Keypad0 + i))
            {
                OnNumberPressed(i.ToString());
            }
        }

        // Actions
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) OnEnterPressed();
        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete)) OnClearPressed();
    }

    /// <summary>
    /// Appends a number to the current input string. Called via UI Buttons.
    /// </summary>
    /// <param name="number">The string value of the number pressed.</param>
    public void OnNumberPressed(string number)
    {
        if(currentInput.Length < 2)
        {
            currentInput += number;
            UpdateDisplay(); 
        }
        
    }

    /// <summary>
    /// Clears the current input string. Called via UI Button or PuzzleManager.
    /// </summary>
    public void OnClearPressed()
    {
        currentInput = "";
        UpdateDisplay();
    }

    /// <summary>
    /// Submits the input to the PuzzleManager. Called via UI Button.
    /// </summary>
    public void OnEnterPressed()
    {
        if (string.IsNullOrEmpty(currentInput)) return;

        Debug.Log($"Code Entered: {currentInput}");
        puzzleManager.StartPuzzle(currentInput);
    }

    private void UpdateDisplay()
    {
        displayText.text = string.IsNullOrEmpty(currentInput) ? "--" : currentInput;
    }
}