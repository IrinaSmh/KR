using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class Options : MonoBehaviour
{

    public static int speed;
    public static bool isChanged = false;
   
    public void setString()
    {
        speed = Convert.ToInt32(gameObject.GetComponent<TMP_InputField>().text);
        isChanged = true;
    }

}
