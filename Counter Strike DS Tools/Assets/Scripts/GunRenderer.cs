using System.Collections.Generic;
using UnityEngine;

public class GunRenderer : MonoBehaviour
{
    [Header("SET YOUR UNITY GAME SCREEN RESOLUTION TO 96*96")]
    public List<GameObject> AllGuns = new List<GameObject>();
    public string savePath;
    int i;
    int LastIndex = 0;


    private void Awake()
    {
        Screen.SetResolution(96, 96, true);
    }

    private void Update()
    {
        //Reduce the screenshooting speed because Unity can't take screenshots to rapidly
        if (Time.time >= i / 10f && i < AllGuns.Count)
        {
            //Disactive the old gun
            AllGuns[LastIndex].SetActive(false);
            LastIndex = i;
            //Active the new one
            AllGuns[i].SetActive(true);

            ScreenCapture.CaptureScreenshot(savePath+"\\gunSprite" + i +".png");
            i++;
        }
    }
}
