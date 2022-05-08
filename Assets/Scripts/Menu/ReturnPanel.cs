using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnPanel : MonoBehaviour
{
    [SerializeField]
    Button Confirm;

    [SerializeField]
    Button Decline;

    [SerializeField]
    string ReturnToThisScene;


    // Start is called before the first frame update
    void Start()
    {
        if (!Confirm || !Decline)
        {
            Confirm = GetComponentsInChildren<Button>()[0];
            Decline = GetComponentsInChildren<Button>()[1];
        }
        Confirm.onClick.AddListener(ConfirmClick);
        Decline.onClick.AddListener(DeclineClick);
    }

    void ConfirmClick()
    {
        if (!string.IsNullOrEmpty(ReturnToThisScene))
        {
            SceneManager.LoadScene(ReturnToThisScene);
        }
    }

    void DeclineClick()
    {
        gameObject.SetActive(false);
    }
}
