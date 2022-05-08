using UnityEngine;

public class SetDontDestroyOnLoad : MonoBehaviour
{

    public static SetDontDestroyOnLoad instance;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }


    }
}
