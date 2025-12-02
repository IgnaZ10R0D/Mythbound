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

    public DialogueLine[] dialogueLines;
    public DialogueImage[] leftImages;
    public DialogueImage[] rightImages;

    [Header("Portraits and Dialogue")]
    public Image leftImageUI;
    public Image rightImageUI;
    public Text dialogueText;

    private int currentIndex = 0;
    private bool dialogueStarted = false;

    public void BeginDialogue()
    {
        Time.timeScale = 0;

        if (dialogueText == null)
        {
            return;
        }

        currentIndex = 0;
        dialogueStarted = true;

        ShowDialogue(currentIndex);
    }

    private void Update()
    {
        if (!dialogueStarted) return;

        if (InputManager.Instance != null && Input.GetKeyDown(InputManager.Instance.GetKey("Shoot")))
        {
            NextDialogue();
        }
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
            Debug.LogWarning($"Hey Line number {index} is null you fucking jackass. Skipping...");
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

    private void NextDialogue()
    {
        currentIndex++;
        ShowDialogue(currentIndex);
    }

    private void EndDialogue()
    {
        Debug.Log("Fin del di�logo, activando boss...");
        Time.timeScale = 1;
        dialogueStarted = false;

        WaveManager waveManager = FindFirstObjectByType<WaveManager>();
        if (waveManager != null)
        {
            waveManager.StartBossFight();
        }

        gameObject.SetActive(false);
    }
}


