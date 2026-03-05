using UnityEngine;

[System.Serializable]
public class StoryChoice
{
    public string title;

    public int happinessChange;
    public int healthChange;
    public int energyChange;
    public int moneyChange;

    public int nextNodeIndex;

    public string resultText; // что показать на обратной стороне карточки (если хочешь готовую фразу)
}

[System.Serializable]
public class StoryNode
{
    public string id;              // чтобы тебе было удобно (Day1_Kitchen и т.п.)
    public Sprite background;      // фон
    [TextArea(2, 6)]
    public string[] lines;         // реплики по порядку

    public bool hasChoices;
    public StoryChoice[] choices;  // 2 карточки или больше

    public int nextNodeIndex;      // куда идти, если выборов нет

    public bool isCalendarNode;
    public Sprite calendarSprite;
}