using Unity.Cinemachine;
using UnityEngine;

public class MiniGameScript : MonoBehaviour, IInteractable
{
    public bool isActive = true;
    private bool isBeingUsed = false;

    CinemachineCamera main;
    [SerializeField] CinemachineCamera miniGameCamera;

    public void OnInteract(IInteractor interactor)
    {
        if(!isActive) return;

        if (isBeingUsed){
            ExitGame();
            return;
        }

        isBeingUsed = true;

        PlayerManager.Instance.ChangeCharacterActive(false);
        //move to start after moving player data set to awake
        main = PlayerManager.Instance.playerData.playerCamera;

        miniGameCamera.Priority = 20;
        main.Priority = 10;
    }

    private void ExitGame(){
        miniGameCamera.Priority = 10;
        main.Priority = 20;
        PlayerManager.Instance.ChangeCharacterActive(true);
        isBeingUsed = false;
    }
}
