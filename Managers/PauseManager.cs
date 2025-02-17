using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    public delegate void uiStateDelegate(UIState state);
    public static event uiStateDelegate OnUIStateChange;

    //used to check if Player should be set active //will put in playerdata if becomes problem but i think its fine
    private bool isCharacterActive = true;

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
        isCharacterActive = PlayerManager.Instance.playerData.isCharacterActive;
        PlayerManager.Instance.ChangeCharacterActive(false);
        Time.timeScale = 0;
    }
    public void UnPause(){
        OnUIStateChange(UIState.Playing);
        if(isCharacterActive){
            PlayerManager.Instance.ChangeCharacterActive(true);
            isCharacterActive = true;
        }
        Time.timeScale = 1;
    }
}
