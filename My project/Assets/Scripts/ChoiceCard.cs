using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceCard : MonoBehaviour
{
    public Button button;

    public GameObject front;
    public GameObject back;

    public TMP_Text titleText;   // на фронте
    public Image frontCardImage;
    public TMP_Text resultText;  // на бэке

    StoryChoice choice;
    StoryManager manager;

    bool used = false;

    public void Setup(StoryChoice newChoice, StoryManager storyManager)
    {
        used = false;

        choice = newChoice;
        manager = storyManager;

        if (front != null) front.SetActive(true);
        if (back != null) back.SetActive(false);

        if (titleText != null) titleText.text = choice.title;

        if (frontCardImage != null)
        {
            frontCardImage.sprite = choice.frontImage;

            if (choice.frontImage != null)
                frontCardImage.gameObject.SetActive(true);
            else
                frontCardImage.gameObject.SetActive(false);
        }

        if (resultText != null)
        {
            if (choice.resultText != null && choice.resultText != "")
            {
                resultText.text = choice.resultText;
            }
            else
            {
                string result = "";

                if (choice.happinessChange != 0)
                {
                    string h = (choice.happinessChange > 0 ? "+" : "") + choice.happinessChange;
                    result += "Happiness " + h + "\n";
                }

                if (choice.healthChange != 0)
                {
                    string he = (choice.healthChange > 0 ? "+" : "") + choice.healthChange;
                    result += "Health " + he + "\n";
                }

                if (choice.energyChange != 0)
                {
                    string e = (choice.energyChange > 0 ? "+" : "") + choice.energyChange;
                    result += "Energy " + e + "\n";
                }

                if (choice.moneyRange != null && choice.moneyRange != "")
                {
                    result += choice.moneyRange + "$";
                }
                else if (choice.moneyChange != 0)
                {
                    string m = (choice.moneyChange > 0 ? "+" : "") + choice.moneyChange;
                    result += m + "$";
                }


                resultText.text = result;
            }
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Choose);
    }

    void Choose()
    {
        if (used) return;
        used = true;

        manager.LockChoiceCards(this);

        StartCoroutine(FlipAndApply());
    }

    IEnumerator FlipAndApply()
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 6;
            float scale = Mathf.Lerp(1, 0, t);
            transform.localScale = new Vector3(scale, 1, 1);
            yield return null;
        }

        if (front != null) front.SetActive(false);
        if (back != null) back.SetActive(true);

        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 6;
            float scale = Mathf.Lerp(0, 1, t);
            transform.localScale = new Vector3(scale, 1, 1);
            yield return null;
        }

        // после переворота применяем и идём дальше
        manager.OnChoiceRevealed(choice);
    }
}