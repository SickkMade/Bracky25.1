using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class MiniGameScript : MonoBehaviour, IInteractable
{
    private bool _isActive = true;
    public bool isActive
    {
        get { return _isActive; }
        set
        {
            _isActive = value;
            if (value)
            {
                InitMiniGame?.Invoke();
            }
            else
            {
                DeactivateMinigame?.Invoke();
            }
        }
    }
    [SerializeField] bool needsItem = false;
    [SerializeField] string neededID;
    private bool isBeingUsed = false;

    CinemachineCamera main;
    [SerializeField] CinemachineCamera miniGameCamera;
    public UnityEvent CallMiniGame; //calls in update when active
    public UnityEvent InitMiniGame; //calls when minigame is SET to Active.
    public UnityEvent CleanUpMiniGame; //calls when minigame is left.
    public UnityEvent DeactivateMinigame; //calls when minigame is Deactivated. Probably Useless?
    public UnityEvent InteractWithObj;

    public void OnInteract(IInteractor interactor)
    {
        if(!isActive) return;

        if (isBeingUsed){
            ExitGame();
            return;
        }
        if (needsItem)
        {
            if (interactor.HeldObject != null && interactor.HeldObject.ID == neededID)
            {
                interactor.HeldObject.UseWithObject();
                InteractWithObj?.Invoke();
                return;
            }
        }
        isBeingUsed = true;

        PlayerManager.Instance.ChangeCharacterActive(false);
        //move to start after moving player data set to awake
        main = PlayerManager.Instance.playerData.playerCamera;

        miniGameCamera.Priority = 20;
        main.Priority = 10;
    }

    void Update(){
        if(isBeingUsed){
            CallMiniGame?.Invoke();
        }
    }

    public void ExitGame(){
        miniGameCamera.Priority = 10;
        main.Priority = 20;
        PlayerManager.Instance.ChangeCharacterActive(true);
        isBeingUsed = false; 
        CleanUpMiniGame?.Invoke();
    }
}
