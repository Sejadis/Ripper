using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Reflection;

public class MenuSetup : MonoBehaviour {

    private object parameter = null;

    public void InvokeMethod(string methodName)
    {
        MethodInfo mi = MenuManager.instance.GetType().GetMethod(methodName);
        if(parameter == null)
        {
            mi.Invoke(MenuManager.instance, null);
        }
        else
        {
            object[] param = new object[] { parameter };
            mi.Invoke(MenuManager.instance, param);
        }
        parameter = null;
    }

    public void SetParameter(Object param)
    {
        parameter = param;
    }
    public void SetParameter(string param)
    {
        parameter = param;
    }
    public void SetParameter(float param)
    {
        parameter = param;
    }
}
