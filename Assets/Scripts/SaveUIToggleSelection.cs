using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScriptableObjectArchitecture;

public class SaveUIToggleSelection : MonoBehaviour
{
    public BoolGameEvent _boolEvent;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Toggle>().isOn = IntToBool(PlayerPrefs.GetInt(this.gameObject.name));
        _boolEvent.Raise(this.gameObject.GetComponent<Toggle>().isOn);
    }

    public void OnToggleChanged()
    {
        bool state = this.gameObject.GetComponent<Toggle>().isOn;
        PlayerPrefs.SetInt(this.gameObject.name, BoolToInt(state));
        _boolEvent.Raise(this.gameObject.GetComponent<Toggle>().isOn);
    }

    private int BoolToInt(bool val)
    {
        if (val)
            return 1;
        else
            return 0;
    }

    private bool IntToBool(int val)
    {
        if (val == 1)
            return true;
        else
            return false;
    }
}
