using UnityEngine;
using TMPro;

/// <summary>
/// Use Information from MemCardController to show UI
/// </summary>
public class MemCardHUD : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Display the current status of the game")]
    TMP_Text HeaderText;

    [SerializeField]
    [Tooltip("Object to active on Win")]
    GameObject WinPanel;
    
    [SerializeField]
    TMP_Text WinGameText;

    [SerializeField]
    MemCardController memCardController;

    [SerializeField]
    GameObject EscapeMenu;

    void Start()
    {
        memCardController.OnCardCountChange += CardCountChanged;
        memCardController.OnWinGame += WinGame;
        WinPanel?.gameObject.SetActive(false);
        EscapeMenu?.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscapeMenu?.SetActive(!EscapeMenu.activeSelf);
        }
    }

    void CardCountChanged(int failedCount, int solvedCount)
    {
        if (!HeaderText)
            return;

        HeaderText.text = (string.Concat("<color=#03a100> ", solvedCount / 2, "</color><color=#ffffff> | </color>", "<color=#c20b08>", failedCount,"</color>"));
    }


    void WinGame(int failedCount)
    {
        if (failedCount > 0)
        {
            WinGameText?.SetText(string.Concat("You have Won the Game in <color=#c20b08>", failedCount, " </color>Steps"));
        }
        else
        {
            WinGameText?.SetText("<color=#fcec03>Perfect Match!</color>");
        }

        WinPanel?.gameObject.SetActive(true);
    }


}
