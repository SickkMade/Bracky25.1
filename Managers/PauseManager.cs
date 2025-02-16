using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    public delegate void uiStateDelegate(UIState state);
    public static event uiStateDelegate OnUIStateChange;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(Time.timeScale > 0.01){ //if not paused
                OnUIStateChange(UIState.Paused);
                Pause();
            }
            else {
                UnPause();
            }
        }
    }

    private void Pause(){
        Time.timeScale = 0;
    }
    public void UnPause(){
        OnUIStateChange(UIState.Playing);
        Time.timeScale = 1;
    }
}
