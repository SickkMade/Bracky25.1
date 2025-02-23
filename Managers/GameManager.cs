using System;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    #region questevents
        public delegate void ButtonDelegate(string buttonId);
        public static event ButtonDelegate ButtonPressedEvent;
        public void InvokeButtonPressedEvent(string id){
            ButtonPressedEvent?.Invoke(id);
        }

        public static event Action<string> EnterCodeEvent;
        public void InvokeEnterCodeEvent(string code){
            EnterCodeEvent?.Invoke(code);
        }

        public QuestEvents questEvents;

    #endregion

    #region misc events
        public static event Action<bool> ItemBeingHeldEvent;
        public void InvokeItemBeingHeldEvent(bool isBeingHeld){
            ItemBeingHeldEvent?.Invoke(isBeingHeld);
        }
    #endregion

    private void Awake()
    {
        // Check if Instance already exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject); // Enforce singleton pattern
        }


        questEvents = new();
    }



    [Header("Game Difficulty Settings")]
    [SerializeField, Range(30f, 300f)] float minBreakTime;
    [SerializeField, Range(90f, 300f)] float maxBreakTime;
    private float curBreakTime;

    [SerializeField] float baseTimeToFix;
    private float curTimeToFix;

    // Reduces down time if minigames are solved quickly, or rewards proactive players.
    [SerializeField, Range(0.5f, 3f)] float speedUpAfterComplete;

    [SerializeField] int targetMinigames;

    public Room curActive;
    [SerializeField] List<MiniGameScript> UnfinishedMinigames = new();
    private List<MiniGameScript> FinishedMinigames = new();

    bool activeMinigame = false;
    bool completedMinigame = false;

    [SerializeField] AudioClip audGameLoad;
    [SerializeField] AudioClip audGameSuccess;
    //[SerializeField] AudioClip audGameFail; // Can you fail minigame, or just take more time?

    //not implimented yet

    // Random Timer in range- Counts down till minigame needed.
    // Notify w/ warning on Tablet, then once below 50% Time remaining

    /// <summary>
    /// Used at the start of the game to fill out the Minigames List.
    /// </summary>
    /// <param name="minigame"></param>
    public void AddMingame(MiniGameScript minigame)
    {
        UnfinishedMinigames.Add(minigame);
    }

    public void Start()
    {
        foreach (MiniGameScript m in UnfinishedMinigames)
        {
            m.isActive = false;
        }
    }

    public void Update()
    {
        if (activeMinigame)
        {
            if (!completedMinigame)
            {
                curTimeToFix -= Time.deltaTime;
                if (curTimeToFix < 0)
                {
                    LoseCondition();
                    return;
                }
            }
            else
            {
                curTimeToFix -= Time.deltaTime * speedUpAfterComplete;
                if (curTimeToFix < 0)
                {
                    activeMinigame = false;
                    curBreakTime = UnityEngine.Random.Range(minBreakTime, maxBreakTime);
                }
            }
        }
        else
        {
            curBreakTime -= Time.deltaTime;
            if (curBreakTime < 0)
            {
                BreakMinigame();
            }
        }
    }

    private void BreakMinigame()
    {
        activeMinigame = true;
        completedMinigame = false;
        // Resets if only 1 possible minigame
        if (UnfinishedMinigames.Count <= 1)
        {
            UnfinishedMinigames.AddRange(FinishedMinigames);
            FinishedMinigames.Clear();
        }

        // Randomly Call a new Minigame
        int minigameIndex = UnityEngine.Random.Range(0, UnfinishedMinigames.Count);
        MiniGameScript nextGame = UnfinishedMinigames[minigameIndex];

        ActivateMinigaame(nextGame);

        // Moves to finished, so it won't be called again.
        UnfinishedMinigames.RemoveAt(minigameIndex);
        FinishedMinigames.Add(nextGame);

        curTimeToFix = baseTimeToFix;
    }

    public void ActivateMinigaame(MiniGameScript m)
    {
        m.isActive = true;
    }

    public void MinigameCompleted()
    {
        completedMinigame = true;
    }

    public void LoseCondition()
    {
        throw new NotImplementedException();
    }
}
