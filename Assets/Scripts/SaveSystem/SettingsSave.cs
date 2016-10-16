using UnityEngine;
using System.Collections;

[System.Serializable]
public class SettingsSave : Save {

    [System.Serializable]
    public class Controls
    {
        public InputManager.Keybinding[] keyBindings = InputManager.SetDefaults();
        public int mouseSensitivity = 3;
        public float doubleClickSpeed = 0.5f;
    }

    [System.Serializable]
    public class Graphics
    {

    }

    public Controls controls = new Controls();
}
