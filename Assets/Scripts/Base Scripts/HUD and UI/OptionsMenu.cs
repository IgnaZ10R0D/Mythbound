using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class OptionsMenu : MonoBehaviour
{
    [Header("Campos de entrada (solo lectura)")]
    [SerializeField] private InputField moveUpInput;
    [SerializeField] private InputField moveDownInput;
    [SerializeField] private InputField moveLeftInput;
    [SerializeField] private InputField moveRightInput;
    [SerializeField] private InputField shootInput;
    [SerializeField] private InputField focusFireInput;
    [SerializeField] private InputField activeSpellCardInput;
    [SerializeField] private InputField cycleSpellCardInput;

    [Header("Botones asociados a cada campo")]
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button shootButton;
    [SerializeField] private Button focusFireButton;
    [SerializeField] private Button activeSpellCardButton;
    [SerializeField] private Button cycleSpellCardButton;

    [Header("Selección por defecto")]
    [SerializeField] private GameObject defaultSelectedButton;

    private string currentAction;
    private InputField currentInputField;
    private Button currentButton;

    void Start()
    {
        SetReadOnly(moveUpInput, moveDownInput, moveLeftInput, moveRightInput,
                    shootInput, focusFireInput, activeSpellCardInput, cycleSpellCardInput);

        // Cargar valores iniciales
        LoadBindings();

        SetupButton(moveUpButton, moveUpInput, "MoveUp");
        SetupButton(moveDownButton, moveDownInput, "MoveDown");
        SetupButton(moveLeftButton, moveLeftInput, "MoveLeft");
        SetupButton(moveRightButton, moveRightInput, "MoveRight");
        SetupButton(shootButton, shootInput, "Shoot");
        SetupButton(focusFireButton, focusFireInput, "FocusFireMode");
        SetupButton(activeSpellCardButton, activeSpellCardInput, "ActiveSpellCard");

        SetupButton(cycleSpellCardButton, cycleSpellCardInput, "CycleSpell");
    }

    private void SetReadOnly(params InputField[] fields)
    {
        foreach (var f in fields)
        {
            if (f != null)
            {
                f.readOnly = true;
                f.interactable = false; 
            }
        }
    }

    private void SetupButton(Button button, InputField targetField, string action)
    {
        if (button == null || targetField == null) return;
        button.onClick.AddListener(() => StartRebinding(action, targetField, button));
    }

    private void StartRebinding(string action, InputField inputField, Button button)
    {
        currentAction = action;
        currentInputField = inputField;
        currentButton = button;

        inputField.text = "Press a Key...";
        StartCoroutine(WaitForKeyPress());
    }

    private IEnumerator WaitForKeyPress()
    {
        yield return null;

        while (!Input.anyKeyDown)
            yield return null;

        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                InputManager.Instance.SetKey(currentAction, key);
                currentInputField.text = key.ToString();
                break;
            }
        }

        if (EventSystem.current != null && currentButton != null)
            EventSystem.current.SetSelectedGameObject(currentButton.gameObject);
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

        cycleSpellCardInput.text = InputManager.Instance.GetKey("CycleSpell").ToString();
    }

    public void SaveBindings()
    {
        InputManager.Instance.SaveControls();
    }

    private void OnEnable()
    {
        if (EventSystem.current != null && defaultSelectedButton != null)
            EventSystem.current.SetSelectedGameObject(defaultSelectedButton);
    }
}




