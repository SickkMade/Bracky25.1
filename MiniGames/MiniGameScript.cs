using Unity.Cinemachine;
using UnityEngine;

public class MiniGameScript : MonoBehaviour, IInteractable
{
    public bool isActive = true;

    CinemachineCamera main;
    [SerializeField] CinemachineCamera miniGameCamera;

    public void OnInteract(IInteractor interactor)
    {
        if(!isActive) return;

        PlayerManager.Instance.ChangeCharacterActive(false);
        //move to start after moving player data set to awake
        main = PlayerManager.Instance.playerData.playerCamera;

        miniGameCamera.Priority = 20;
        main.Priority = 10;
    }
}
