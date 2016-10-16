using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    [System.Serializable]
    public class Keybinding
    {
        public string name;
        public KeyCode mainKey;
        public KeyCode altKey;

        public Keybinding(string name, KeyCode mainKey, KeyCode altKey)
        {
            this.name = name;
            this.mainKey = mainKey;
            this.altKey = altKey;
        }
    }

    public InputManager instance;

    void OnEnable()
    {
        EventHandler.OnSettingsChanged += OnsettingsChanged;
    }

    void OnDisable()
    {
        EventHandler.OnSettingsChanged -= OnsettingsChanged;
    }

    private void OnsettingsChanged(SettingsSave config)
    {
        if(bindings != config.controls.keyBindings)
        {
            bindings = config.controls.keyBindings;
        }
    }

    private static Keybinding[] bindings = { new Keybinding("Forward", KeyCode.W, KeyCode.UpArrow),
                                             new Keybinding("Backward", KeyCode.S, KeyCode.DownArrow),
                                             new Keybinding("Left", KeyCode.A, KeyCode.LeftArrow),
                                             new Keybinding("Right", KeyCode.D, KeyCode.RightArrow),
                                             new Keybinding("Jump", KeyCode.Space, KeyCode.None),
                                             new Keybinding("Action", KeyCode.Mouse0, KeyCode.None),
                                             new Keybinding("Camera", KeyCode.Mouse1, KeyCode.None),
                                             new Keybinding("Sprint", KeyCode.LeftShift, KeyCode.RightShift),
                                             new Keybinding("Pause", KeyCode.Escape, KeyCode.None)
    };

    public static Keybinding[] Bindings
    {
        get
        {
            return bindings;
        }

        set
        {
            bindings = value;
        }
    }

/*
    /// <summary>
    /// Returns the main Keycode of the given action.
    /// </summary>
    /// <param name="actionName"></param>
    /// <returns></returns>
    public static KeyCode GetKeyCode(string actionName)
    {
        foreach (Keybinding binding in bindings)
        {
            if (binding.name.ToLower() == actionName.ToLower())
            {
                return binding.mainKey;
            }
        }
        return KeyCode.None;
    }

    */

    /// <summary>
    /// Returns the Keycode for the given action. Main / alt Key depends on the boolean.
    /// </summary>
    /// <param name="name">Actionname as string</param>
    /// <param name="mainKey">true : main key; false : alt key</param>
    /// <returns></returns>
    public static KeyCode GetKeyCode(string name, bool mainKey)
    {
        foreach (Keybinding binding in bindings)
        {
            if (binding.name.ToLower() == name.ToLower())
            {
                if (mainKey)
                {
                    return binding.mainKey;
                }
                else
                {
                    return binding.altKey;
                }
            }
        }
        return KeyCode.None;
    }

    /// <summary>
    /// Resets all changed keybindings.
    /// </summary>
    public static Keybinding[] SetDefaults()
    {
        bindings = new Keybinding[] { new Keybinding("Forward", KeyCode.W, KeyCode.UpArrow),
                                      new Keybinding("Backward", KeyCode.S, KeyCode.DownArrow),
                                      new Keybinding("Left", KeyCode.A, KeyCode.LeftArrow),
                                      new Keybinding("Right", KeyCode.D, KeyCode.RightArrow),
                                      new Keybinding("Jump", KeyCode.Space, KeyCode.None),
                                      new Keybinding("Action", KeyCode.Mouse0, KeyCode.None),
                                      new Keybinding("Camera", KeyCode.Mouse1, KeyCode.None),
                                      new Keybinding("Sprint", KeyCode.LeftShift, KeyCode.RightShift),
                                      new Keybinding("Pause", KeyCode.Escape, KeyCode.None)
        };

        return bindings;
    }

    public static void SetBinding(int index, KeyCode newKey, bool mainKey)
    {
        if (mainKey)
        {
            bindings[index].mainKey = newKey;
        }
        else
        {
            bindings[index].altKey = newKey;
        }
    }
    /// <summary>
    /// Returns true while the main or alt key for the given action is pressed
    /// </summary>
    /// <param name="name">Name of the action</param>
    /// <returns></returns>
    public static bool GetKey(string name)
    {
        bool value = false;

        if (Input.GetKey(GetKeyCode(name, true)) || Input.GetKey(GetKeyCode(name, false)))
        {
            value = true;
        }

        return value;
    }

    /// <summary>
    /// Returns true when the alt or main key for the given action is pressed
    /// </summary>
    /// <param name="name">Name of the action</param>
    /// <returns></returns>
    public static bool GetKeyDown(string name)
    {
        bool value = false;

        if (Input.GetKeyDown(GetKeyCode(name, true)) || Input.GetKeyDown(GetKeyCode(name, false)))
        {
            value = true;
        }

        return value;
    }

    /// <summary>
    /// Returns true when the alt or main key for the given action is released
    /// </summary>
    /// <param name="name">Name of the action</param>
    /// <returns></returns>
    public static bool GetKeyUp(string name)
    {
        bool value = false;

        if (Input.GetKey(GetKeyCode(name, true)) || Input.GetKeyUp(GetKeyCode(name, false)))
        {
            value = true;            
        }

        return value;
    }
}