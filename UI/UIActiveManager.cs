using UnityEngine;

public enum UIState {
    Death,
    Playing,
    Paused,
}
public class UIActiveManager : MonoBehaviour
{
    private UIState currentState = UIState.Playing;

    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject DeathMenu;
    [SerializeField] GameObject PlayingUI;

    void Start()
    {
        ChangeState();
        PauseManager.OnUIStateChange += SetState;
    }

    public void SetState(UIState newState){
        currentState = newState;
        ChangeState();
    }

    private void ChangeState(){ //change to a list if grows
        PauseMenu.SetActive(false);
        DeathMenu.SetActive(false);
        PlayingUI.SetActive(false);

        switch(currentState){
            case UIState.Death:
                DeathMenu.SetActive(true);
                break;
            case UIState.Playing:
                PlayingUI.SetActive(true);
                break;
            case UIState.Paused:
                PauseMenu.SetActive(true);
                break;
        }
    }

}
