using UnityEngine;
using UnityEngine.EventSystems;

// summary 
//Allows a UI element to be dragged and snapped to a specific target outline.
//summary
public class DraggableNumber : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Settings")]
    [SerializeField] private float snapDistance = 75f;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private RectTransform targetOutline;
    private PuzzleManager puzzleManager;
    
    public bool IsSnapped { get; private set; }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        puzzleManager = FindObjectOfType<PuzzleManager>();
    }

    /// <summary>
    /// Assigns the target outline this number must snap to.
    /// </summary>
    public void Initialize(RectTransform target)
    {
        targetOutline = target;
        IsSnapped = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsSnapped) return;

        transform.SetAsLastSibling(); // Bring to front
        canvasGroup.blocksRaycasts = false; // Allow passing clicks through to the target underneath
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsSnapped) return;

        // Move the UI element based on mouse delta, adjusted for canvas scaling
        rectTransform.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsSnapped) return;
        
        canvasGroup.blocksRaycasts = true;

        if (targetOutline == null) return;

        // Calculate distance and snap if close enough
        float distance = Vector2.Distance(rectTransform.anchoredPosition, targetOutline.anchoredPosition);
        
        if (distance <= snapDistance)
        {
            SnapToTarget();
        }
    }

    private void SnapToTarget()
    {
        rectTransform.anchoredPosition = targetOutline.anchoredPosition;
        IsSnapped = true;
        puzzleManager.CheckWinCondition(); 
    }
}