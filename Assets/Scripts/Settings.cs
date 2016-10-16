using UnityEngine;
using System.Collections;
using System;

public class Settings : MonoBehaviour
{
    private static int defaultFOV = 60;
    SettingsSave config;

    #region Properties

    public static int DefaultFOV
    {
        get
        {
            return defaultFOV;
        }

        set
        {
            defaultFOV = value;
        }
    }

    #endregion


    void Awake()
    {
        //if config file exists load else create new default configs file
        SettingsSave loadedConfig = SaveSystem.Load("config", SaveType.set) as SettingsSave;
        if(loadedConfig != null)
        {
            config = loadedConfig;
        }
        else
        {
            config = new SettingsSave();
            SaveSystem.Save(config, "config", SaveType.set);
        }
        LoadSettings();
    }

    /*
    public void SetPlayerName(string name)
    {
        config.playerName = name;
        SaveSystem.Save(config, "config", SaveType.set);
    }
    */

    public void SaveBindings()
    {
        config.controls.keyBindings = InputManager.Bindings;
        SaveSystem.Save(config, "config", SaveType.set);
    }

    private void LoadSettings()
    {
        EventHandler.TriggerOnSettingsChanged(config);
        /*
        LoadBindings();
        LoadControlValues();
        */
    }

    private void LoadBindings()
    {
        if(config == null)
        {
            Debug.Log("config is null");
        }
        else
        {
            if (config.controls.keyBindings != null)
            {
                InputManager.Bindings = config.controls.keyBindings;
            }
            else
            {
                SaveBindings();
            }
        }

    }

    private void LoadControlValues()
    {

    }

    public void SaveControlValues()
    {

    }
}
