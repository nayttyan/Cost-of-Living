using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryManager : MonoBehaviour
{
    public Image backgroundImage;

    public TMP_Text dialogueText;
    public Button nextButton;

    public GameObject choicesPanel;

    public ChoiceCard[] choiceCards; // 2 карточки (или сколько сделаешь)

    public PlayerStats stats;
    public StatUI happinessUI;
    public StatUI healthUI;
    public StatUI energyUI;
    public MoneyUI moneyUI;
    public PlayerFaceUI playerFaceUI;

    public StoryNode[] nodes;

    int nodeIndex = 0;
    int lineIndex = 0;

    StoryChoice pendingChoice = null;
    bool waitingAfterChoice = false;

    public GameObject mainUI;
    public GameObject calendarPanel;
    public Image calendarImage;
    public Button nextDayButton;

    int GetStatValue(StatType statType)
    {
        if (statType == StatType.Happiness)
            return stats.happiness;

        if (statType == StatType.Health)
            return stats.health;

        if (statType == StatType.Energy)
            return stats.energy;

        if (statType == StatType.Money)
            return stats.money;

        return 0;
    }
    void Start()
    {
        choicesPanel.SetActive(false);
        ShowNode(0);

        nextButton.onClick.AddListener(Next);
        nextDayButton.onClick.AddListener(ContinueFromCalendar);

        playerFaceUI.UpdateFace();
    }

    void ShowNode(int index)
    {
        nodeIndex = index;
        lineIndex = 0;

        // календарный экран
        if (nodes[index].isCalendarNode)
        {
            ShowCalendar(nodes[index].calendarSprite);
            return;
        }
        else
        {
            HideCalendar();
        }

        if (nodes[index].hasCondition)
        {
            if (StoryVariables.instance.GetBool(nodes[index].conditionFlag))
            {
                ShowNode(nodes[index].trueNodeIndex);
            }
            else
            {
                ShowNode(nodes[index].falseNodeIndex);
            }

            return;
        }
        if (nodes[index].hasStatCondition)
        {
            int value = GetStatValue(nodes[index].statType);

            if (value <= nodes[index].lowValue)
            {
                ShowNode(nodes[index].lowNodeIndex);
            }
            else if (value >= nodes[index].highValue)
            {
                ShowNode(nodes[index].highNodeIndex);
            }
            else
            {
                ShowNode(nodes[index].midNodeIndex);
            }

            return;
        }

        choicesPanel.SetActive(false);
        nextButton.gameObject.SetActive(true);

        if (backgroundImage != null && nodes[nodeIndex].background != null)
            backgroundImage.sprite = nodes[nodeIndex].background;

        dialogueText.text = nodes[nodeIndex].lines[lineIndex];
    }

    void Next()
    {
        // если мы ждём подтверждения после выбора карточки
        if (waitingAfterChoice)
        {
            waitingAfterChoice = false;

            if (choicesPanel != null)
                choicesPanel.SetActive(false);

            ApplyChoice(pendingChoice);
            pendingChoice = null;

            return;
        }

        lineIndex++;

        if (lineIndex < nodes[nodeIndex].lines.Length)
        {
            dialogueText.text = nodes[nodeIndex].lines[lineIndex];
            return;
        }

        // реплики закончились
        if (nodes[nodeIndex].hasChoices)
        {
            ShowChoices();
        }
        else
        {
            ShowNode(nodes[nodeIndex].nextNodeIndex);
        }
    }

    void ContinueFromCalendar()
    {
        ShowNode(nodes[nodeIndex].nextNodeIndex);
    }

    public void LockChoiceCards(ChoiceCard chosenCard)
    {
        for (int i = 0; i < choiceCards.Length; i++)
        {
            if (choiceCards[i] != null && choiceCards[i].button != null)
            {
                if (choiceCards[i] != chosenCard)
                    choiceCards[i].button.interactable = false;
            }
        }
    }

    public void UnlockChoiceCards()
    {
        for (int i = 0; i < choiceCards.Length; i++)
        {
            if (choiceCards[i] != null && choiceCards[i].button != null)
                choiceCards[i].button.interactable = true;
        }
    }

    void ShowChoices()
    {
        nextButton.gameObject.SetActive(false);
        choicesPanel.SetActive(true);
        UnlockChoiceCards();


        for (int i = 0; i < choiceCards.Length; i++)
        {
            if (i < nodes[nodeIndex].choices.Length)
            {
                choiceCards[i].gameObject.SetActive(true);

                // настроили карточку под выбор
                choiceCards[i].Setup(
                    nodes[nodeIndex].choices[i],
                    this
                );
            }
            else
            {
                choiceCards[i].gameObject.SetActive(false);
            }
        }
    }

    void ShowCalendar(Sprite sprite)
    {
        if (mainUI != null) mainUI.SetActive(false);

        if (calendarPanel != null) calendarPanel.SetActive(true);

        if (calendarImage != null) calendarImage.sprite = sprite;

        if (nextDayButton != null) nextDayButton.gameObject.SetActive(true);

        if (choicesPanel != null) choicesPanel.SetActive(false);

        if (nextButton != null) nextButton.gameObject.SetActive(true);

        // на календаре удобно писать короткую строку (не обязательно)
        if (dialogueText != null) dialogueText.text = "";
    }

    void HideCalendar()
    {
        if (calendarPanel != null) calendarPanel.SetActive(false);

        if (nextDayButton != null) nextDayButton.gameObject.SetActive(false);

        if (mainUI != null) mainUI.SetActive(true);
    }

    public void OnChoiceRevealed(StoryChoice choice)
    {
        pendingChoice = choice;
        waitingAfterChoice = true;

        // показываем кнопку Next
        nextButton.gameObject.SetActive(true);
    }

    // вызывается карточкой после выбора
    public void ApplyChoice(StoryChoice choice)
    {
        if (choice.setFlag != null && choice.setFlag != "")
        {
            StoryVariables.instance.SetBool(choice.setFlag, choice.setFlagValue);
        }

        if (choice.addIntKey != null && choice.addIntKey != "")
        {
            StoryVariables.instance.AddInt(choice.addIntKey, choice.addIntValue);
        }
        stats.AddHappiness(choice.happinessChange);
        stats.AddHealth(choice.healthChange);
        stats.AddEnergy(choice.energyChange);
        if (choice.moneyRange != null && choice.moneyRange != "")
        {
            string[] parts = choice.moneyRange.Split('-');

            if (parts.Length == 2)
            {
                int a = int.Parse(parts[0].Trim());
                int b = int.Parse(parts[1].Trim());

                int min = Mathf.Min(a, b);
                int max = Mathf.Max(a, b);

                int value = Random.Range(min, max + 1);
                stats.AddMoney(value);
            }
            else if (parts.Length == 1)
            {
                int value = int.Parse(parts[0].Trim());
                stats.AddMoney(value);
            }
        }
        else
        {
            stats.AddMoney(choice.moneyChange);
        }

        happinessUI.UpdateUI();
        healthUI.UpdateUI();
        energyUI.UpdateUI();
        moneyUI.UpdateUI();
        playerFaceUI.UpdateFace();

        ShowNode(choice.nextNodeIndex);
    }
}