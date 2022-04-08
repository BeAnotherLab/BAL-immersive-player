using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveUIToggleSelection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Toggle>().isOn = IntToBool(PlayerPrefs.GetInt(this.gameObject.name));
    }

    public void OnToggleChanged()
    {
        bool state = this.gameObject.GetComponent<Toggle>().isOn;
        PlayerPrefs.SetInt(this.gameObject.name, BoolToInt(state));
        Debug.Log("suposed to save preferences for " + this.gameObject.name + " " + BoolToInt(state));
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
