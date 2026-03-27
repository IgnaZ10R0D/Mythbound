using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public string text;
        public Color color = Color.white;
        [Range(0f, 1f)] public float alphaOverride = 1f;
    }

    [System.Serializable]
    public class DialogueImage
    {
        public Sprite sprite;
        public bool isTransparent;
    }

    [Header("Level Info")]
    public string levelID;

    [Header("Dialogue Data - Normal")]
    public DialogueLine[] dialogueLines;
    public DialogueImage[] leftImages;
    public DialogueImage[] rightImages;

    [Header("Dialogue Data - Alternate")]
    public DialogueLine[] altDialogueLines;
    public DialogueImage[] altLeftImages;
    public DialogueImage[] altRightImages;

    [Header("UI References")]
    public Image leftImageUI;
    public Image rightImageUI;
    public Text dialogueText;

    private int currentIndex = 0;
    private bool isActive = false;

    // Active Sets
    private DialogueLine[] activeLines;
    private DialogueImage[] activeLeft;
    private DialogueImage[] activeRight;

    public void BeginDialogue()
    {
        if (dialogueText == null)
            return;

        bool useAlt = false;

        if (GameManager.Instance != null && !string.IsNullOrEmpty(levelID))
        {
            useAlt = GameManager.Instance.HasSomethingChanged(levelID);
        }

        // Choose the dialogue set
        if (useAlt)
        {
            activeLines = altDialogueLines;
            activeLeft = altLeftImages;
            activeRight = altRightImages;
        }
        else
        {
            activeLines = dialogueLines;
            activeLeft = leftImages;
            activeRight = rightImages;
        }

        if (activeLines == null || activeLines.Length == 0)
            return;

        currentIndex = 0;
        isActive = true;
        gameObject.SetActive(true);

        ShowDialogue(currentIndex);
    }

    private void Update()
    {
        if (!isActive)
            return;

        if (InputManager.Instance != null &&
            Input.GetKeyDown(InputManager.Instance.GetKey("Shoot")))
        {
            NextDialogue();
        }
    }

    private void NextDialogue()
    {
        currentIndex++;
        ShowDialogue(currentIndex);
    }

    private void ShowDialogue(int index)
    {
        if (index >= activeLines.Length)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = activeLines[index];
        if (line == null)
        {
            currentIndex++;
            ShowDialogue(currentIndex);
            return;
        }

        dialogueText.text = line.text;

        Color c = line.color;
        c.a = line.alphaOverride;
        dialogueText.color = c;

        if (leftImageUI != null && index < activeLeft.Length && activeLeft[index] != null)
        {
            leftImageUI.sprite = activeLeft[index].sprite;
            Color lc = leftImageUI.color;
            lc.a = activeLeft[index].isTransparent ? 0.3f : 1f;
            leftImageUI.color = lc;
        }

        if (rightImageUI != null && index < activeRight.Length && activeRight[index] != null)
        {
            rightImageUI.sprite = activeRight[index].sprite;
            Color rc = rightImageUI.color;
            rc.a = activeRight[index].isTransparent ? 0.3f : 1f;
            rightImageUI.color = rc;
        }
    }

    private void EndDialogue()
    {
        isActive = false;
        gameObject.SetActive(false);

        if (GameStateController.Instance != null)
        {
            GameStateController.Instance.EndDialogue();
        }
    }
}



