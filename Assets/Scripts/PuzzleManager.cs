using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the transition from the keypad to the puzzle, spawns the numbers, and checks for win conditions.
/// </summary>
public class PuzzleManager : MonoBehaviour
{
    [Header("UI Screens")]
    [SerializeField] private GameObject keypadScreen;
    [SerializeField] private GameObject puzzleScreen;
    [SerializeField] private GameObject restartButton;

    [Header("Containers")]
    [SerializeField] private Transform targetsContainer;
    [SerializeField] private Transform draggablesContainer;

    [Header("Prefabs (Index 0-9)")]
    [Tooltip("Outlines for numbers 0-9 in order.")]
    [SerializeField] private GameObject[] targetPrefabs; 
    
    [Tooltip("Colored numbers for 0-9 in order.")]
    [SerializeField] private GameObject[] draggablePrefabs; 

    private List<DraggableNumber> activeDraggables = new List<DraggableNumber>();
    private KeypadController keypadController;

    private void Awake()
    {
        keypadController = FindObjectOfType<KeypadController>();
    }

    /// <summary>
    /// Generates the puzzle board based on the user's numeric input.
    /// </summary>
    public void StartPuzzle(string inputNumbers)
    {
        keypadScreen.SetActive(false);
        puzzleScreen.SetActive(true);
        restartButton.SetActive(false);

        float spacing = 150f;
        float startX = -((inputNumbers.Length - 1) * spacing) / 2f;

        for (int i = 0; i < inputNumbers.Length; i++)
        {
            // Convert character to integer index (e.g., '5' becomes 5)
            int digitIndex = int.Parse(inputNumbers[i].ToString());

            // 1. Spawn the static outline
            GameObject targetObj = Instantiate(targetPrefabs[digitIndex], targetsContainer);
            RectTransform targetRect = targetObj.GetComponent<RectTransform>();
            targetRect.anchoredPosition = new Vector2(startX + (i * spacing), 0);

            // 2. Spawn the draggable number in a random location
            GameObject dragObj = Instantiate(draggablePrefabs[digitIndex], draggablesContainer);
            RectTransform dragRect = dragObj.GetComponent<RectTransform>();
            dragRect.anchoredPosition = new Vector2(Random.Range(-400f, 400f), Random.Range(-250f, 250f));

            // 3. Initialize the draggable script with its matching outline
            DraggableNumber dragScript = dragObj.GetComponent<DraggableNumber>();
            dragScript.Initialize(targetRect);
            activeDraggables.Add(dragScript);
        }
    }

    
    /// <summary>
    /// Checks if all active draggables have been successfully snapped to their targets.
    /// </summary>
    
    public void CheckWinCondition()
    {
        foreach (DraggableNumber draggable in activeDraggables)
        {
            if (!draggable.IsSnapped) return; // Exit early if any piece is unfinished
        }

        restartButton.SetActive(true);
    }

    /// <summary>
    /// Resets the game state and returns to the keypad screen.
    /// </summary>
    public void RestartGame()
    {
        ClearPuzzleBoard();

        puzzleScreen.SetActive(false);
        keypadScreen.SetActive(true);
        
        keypadController.OnClearPressed(); // Reset the text display
    }

    private void ClearPuzzleBoard()
    {
        foreach (Transform child in targetsContainer) Destroy(child.gameObject);
        foreach (Transform child in draggablesContainer) Destroy(child.gameObject);
        activeDraggables.Clear();
    }
}