using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class DialogueSequence : MonoBehaviour
{
    public TMP_Text dialogueText;
    public Button nextButton;

    public string[] lines;

    public bool loadSceneAfterEnd = true;
    public string nextSceneName;

    int index = 0;

    void Start()
    {
        index = 0;

        if (lines != null && lines.Length > 0)
        {
            dialogueText.text = lines[index];
        }

        if (nextButton != null)
        {
            nextButton.onClick.AddListener(NextLine);
        }
    }

    public void NextLine()
    {
        index++;

        if (index >= lines.Length)
        {
            if (loadSceneAfterEnd && nextSceneName != "")
            {
                SceneManager.LoadScene(nextSceneName);
            }
            return;
        }

        dialogueText.text = lines[index];
    }
}