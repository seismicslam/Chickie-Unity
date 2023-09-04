using UnityEngine;

public class ScreenController : MonoBehaviour
{
    private int screenWidth = 0;
    private int screenHeight = 0;
    private const int desiredScreenWidth = 400;
    private const int desiredScreenHeight = 700;

    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    void OnGUI()
    {
        if (Screen.width != screenWidth || Screen.height != screenHeight)
        {
            int newWidth = Screen.width;
            int newHeight = Screen.height;

            float screenRatio = (float)newWidth / (float)newHeight;
            float desiredRatio = (float)desiredScreenWidth / (float)desiredScreenHeight;

            if (screenRatio != desiredRatio)
            {
                int newWidthClamped = Mathf.Clamp(newHeight * desiredScreenWidth / desiredScreenHeight, desiredScreenWidth, int.MaxValue);
                int newHeightClamped = Mathf.Clamp(newWidth * desiredScreenHeight / desiredScreenWidth, desiredScreenHeight, int.MaxValue);

                Screen.SetResolution(newWidthClamped, newHeightClamped, true);

                screenWidth = newWidthClamped;
                screenHeight = newHeightClamped;
            }
            else
            {
                screenWidth = newWidth;
                screenHeight = newHeight;
            }
        }
    }
}




