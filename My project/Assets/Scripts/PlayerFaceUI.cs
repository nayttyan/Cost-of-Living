using UnityEngine;
using UnityEngine.UI;

public class PlayerFaceUI : MonoBehaviour
{
    public Image faceImage;
    public PlayerStats stats;

    public Sprite happySprite;
    public Sprite sleepySprite;
    public Sprite sickSprite;
    public Sprite cryingSprite;

    public void UpdateFace()
    {
        if (stats.health <= 50)
        {
            faceImage.sprite = sickSprite;
        }
        else if (stats.happiness <= 50)
        {
            faceImage.sprite = cryingSprite;
        }
        else if (stats.energy <= 50)
        {
            faceImage.sprite = sleepySprite;
        }
        else
        {
            faceImage.sprite = happySprite;
        }
    }

    void Start()
    {
        UpdateFace();
    }
}