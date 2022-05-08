using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoadSceneOnButton : MonoBehaviour
{
    [SerializeField]
    string SceneToLoad;


    void Start()
    {
        GetComponent<Button>().onClick.AddListener(LoadScene);
    }

    void LoadScene()
    {
        if(!string.IsNullOrEmpty(SceneToLoad))
            SceneManager.LoadScene(SceneToLoad);
    }
}
