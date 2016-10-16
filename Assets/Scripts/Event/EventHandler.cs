using UnityEngine;
using System.Collections;

public class EventHandler : MonoBehaviour {

    #region PauseEvent
    public delegate void PauseEvent();
    public static event PauseEvent OnPause;
    public static event PauseEvent OnResume;

    public static void TriggerOnPause()
    {
        if (OnPause != null)
        {
            OnPause();
        }
    }

    public static void TriggerOnResume()
    {
        if (OnResume != null)
        {
            OnResume();
        }
    }
    #endregion

    #region SettingsEvent
    public delegate void SettingsEvent(SettingsSave config);
    public static event SettingsEvent OnSettingsChanged;

    public static void TriggerOnSettingsChanged(SettingsSave config)
    {
        if(OnSettingsChanged != null)
        {
            OnSettingsChanged(config);
        }
    }
    #endregion
}
