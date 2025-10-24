using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;

    public static InputManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("InputManager");
                instance = obj.AddComponent<InputManager>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    private Dictionary<string, KeyCode> controls = new Dictionary<string, KeyCode>();

    private const string ControlsKeyPrefix = "Control_";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadControls();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public KeyCode GetKey(string action)
    {
        return controls.ContainsKey(action) ? controls[action] : KeyCode.None;
    }

    public void SetKey(string action, KeyCode newKey)
    {
        if (controls.ContainsKey(action))
        {
            controls[action] = newKey;
        }
        else
        {
            controls.Add(action, newKey);
        }
    }

    public void SaveControls()
    {
        foreach (var control in controls)
        {
            PlayerPrefs.SetString(ControlsKeyPrefix + control.Key, control.Value.ToString());
        }
        PlayerPrefs.Save();
    }

    public void LoadControls()
    {
        controls.Clear();
        SetKey("MoveUp", KeyCode.UpArrow);
        SetKey("MoveDown", KeyCode.DownArrow);
        SetKey("MoveLeft", KeyCode.LeftArrow);
        SetKey("MoveRight", KeyCode.RightArrow);
        SetKey("Shoot", KeyCode.Z);
        SetKey("FocusFireMode", KeyCode.LeftShift);
        SetKey("ActiveSpellCard", KeyCode.X); 
        SetKey("CycleSpell", KeyCode.C);

        var keys = new List<string>(controls.Keys);
        foreach (var action in keys)
        {
            string savedKey = PlayerPrefs.GetString(ControlsKeyPrefix + action, controls[action].ToString());
            if (System.Enum.TryParse(savedKey, out KeyCode key))
            {
                controls[action] = key;
            }
        }
    }
}

