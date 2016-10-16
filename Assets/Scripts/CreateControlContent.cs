using UnityEngine;
using System.Collections;

public class CreateControlContent : MonoBehaviour
{

    [SerializeField]
    private GameObject controlPrefab;
    [SerializeField]
    private float yStartOffset = -75;
    [SerializeField]
    private float yOffset = -70;

    void Start()
    {
        int counter = 0;
        InputManager.Keybinding[] bindings = InputManager.Bindings;
        //TODO
        //add special cases for function keys like ctrl shift etc

        for (int i = 0; i < bindings.Length - 1; i++)
        {
            Debug.Log(bindings[i].name + " at " + i);
            GameObject prefabInstance = Instantiate(controlPrefab);
            prefabInstance.transform.SetParent(transform, false);
            counter++;

            prefabInstance.GetComponent<ControlPrefab>().label.text = bindings[i].name;
            if (bindings[i].mainKey != KeyCode.None)
            {
                prefabInstance.GetComponent<ControlPrefab>().mainText.text = bindings[i].mainKey.ToString();
            }
            else
            {
                //needs to be changed to not yet set button image
                prefabInstance.GetComponent<ControlPrefab>().mainText.text = "";
            }
            if (bindings[i].altKey != KeyCode.None)
            {
                prefabInstance.GetComponent<ControlPrefab>().altText.text = bindings[i].altKey.ToString();
            }
            else
            {
                //needs to be changed to not yet set button image
                prefabInstance.GetComponent<ControlPrefab>().altText.text = "";
            }

            prefabInstance.name = "Control Item " + bindings[i].name;
            prefabInstance.transform.localPosition = new Vector3(0, yStartOffset - ((counter - 1) * yOffset), 0);
            Debug.Log(prefabInstance.transform.position);
        }

        //     }

    }

}
