using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{

    [SerializeField] private string printName;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse2)) 
        {
            Debug.Log(Application.persistentDataPath);
            ScreenCapture.CaptureScreenshot(printName +".png");
        }
        
    }
}
