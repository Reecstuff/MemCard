using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class QuitOnButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(QuitGame);
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
