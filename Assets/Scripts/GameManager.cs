using UnityEngine;

public class GameManager : MonoBehaviour
{
    static private GameManager _instance;

    static public GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                _instance = go.AddComponent<GameManager>();
            }

            return _instance;
        }
        private set => _instance = value;
    }

    void Start()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }        

        _instance = this;
    }

    public void HideMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
