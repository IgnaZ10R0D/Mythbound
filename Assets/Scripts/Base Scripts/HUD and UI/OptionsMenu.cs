using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private InputField moveUpInput;
    [SerializeField] private InputField moveDownInput;
    [SerializeField] private InputField moveLeftInput;
    [SerializeField] private InputField moveRightInput;
    [SerializeField] private InputField shootInput;
    [SerializeField] private InputField focusFireInput;
    [SerializeField] private InputField activeSpellCardInput;
    [SerializeField] private InputField cycleSpellCardInput;

    private string currentAction; 
    private InputField currentInputField; 


    void Start()
    {
        LoadBindings();

        SetupInputField(moveUpInput, "MoveUp");
        SetupInputField(moveDownInput, "MoveDown");
        SetupInputField(moveLeftInput, "MoveLeft");
        SetupInputField(moveRightInput, "MoveRight");
        SetupInputField(shootInput, "Shoot");
        SetupInputField(focusFireInput, "FocusFireMode");
        SetupInputField(activeSpellCardInput, "ActiveSpellCard");
        SetupInputField(cycleSpellCardInput, "CycleSpellCard");
    }

    private void SetupInputField(InputField inputField, string action)
    {
        EventTrigger trigger = inputField.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };

        entry.callback.AddListener((eventData) => StartRebinding(action, inputField));
        trigger.triggers.Add(entry);
    }

    private void StartRebinding(string action, InputField inputField)
    {
        currentAction = action;
        currentInputField = inputField;
        inputField.text = "Press a Key...";
        StartCoroutine(WaitForKeyPress());
    }

    private System.Collections.IEnumerator WaitForKeyPress()
    {
        yield return null;

        while (!Input.anyKeyDown) 
        {
            yield return null;
        }

        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                InputManager.Instance.SetKey(currentAction, key);
                currentInputField.text = key.ToString(); 
                break;
            }
        }
    }

    public void LoadBindings()
    {
        moveUpInput.text = InputManager.Instance.GetKey("MoveUp").ToString();
        moveDownInput.text = InputManager.Instance.GetKey("MoveDown").ToString();
        moveLeftInput.text = InputManager.Instance.GetKey("MoveLeft").ToString();
        moveRightInput.text = InputManager.Instance.GetKey("MoveRight").ToString();
        shootInput.text = InputManager.Instance.GetKey("Shoot").ToString();
        focusFireInput.text = InputManager.Instance.GetKey("FocusFireMode").ToString();
        activeSpellCardInput.text = InputManager.Instance.GetKey("ActiveSpellCard").ToString();
        cycleSpellCardInput.text = InputManager.Instance.GetKey("CycleSpellCard").ToString();
    }

    public void SaveBindings()
    {
        InputManager.Instance.SaveControls();
    }
}





