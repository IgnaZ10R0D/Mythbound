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

    [Header("Dialogue Data")]
    public DialogueLine[] dialogueLines;
    public DialogueImage[] leftImages;
    public DialogueImage[] rightImages;

    [Header("UI References")]
    public Image leftImageUI;
    public Image rightImageUI;
    public Text dialogueText;

    private int currentIndex = 0;
    private bool isActive = false;

    public void BeginDialogue()
    {
        if (dialogueText == null || dialogueLines == null || dialogueLines.Length == 0)
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
        if (index >= dialogueLines.Length)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = dialogueLines[index];
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

        if (leftImageUI != null && index < leftImages.Length && leftImages[index] != null)
        {
            leftImageUI.sprite = leftImages[index].sprite;
            Color lc = leftImageUI.color;
            lc.a = leftImages[index].isTransparent ? 0.3f : 1f;
            leftImageUI.color = lc;
        }

        if (rightImageUI != null && index < rightImages.Length && rightImages[index] != null)
        {
            rightImageUI.sprite = rightImages[index].sprite;
            Color rc = rightImageUI.color;
            rc.a = rightImages[index].isTransparent ? 0.3f : 1f;
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



