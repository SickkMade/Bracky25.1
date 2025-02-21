using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Quest> questMap;

    private void Awake()
    {
        questMap = CreateQuestMap();
    }

    void OnEnable()
    {
        GameManager.Instance.questEvents.OnStartQuest += StartQuest;
        GameManager.Instance.questEvents.OnAdvanceQuest += AdvanceQuest;
        GameManager.Instance.questEvents.OnFinishQuest += FinishQuest;
    }

    void OnDisable()
    {
        GameManager.Instance.questEvents.OnStartQuest -= StartQuest;
        GameManager.Instance.questEvents.OnAdvanceQuest -= AdvanceQuest;
        GameManager.Instance.questEvents.OnFinishQuest -= FinishQuest;
    }

    void Start()
    {
        foreach(Quest quest in questMap.Values){
            GameManager.Instance.questEvents.QuestStateChange(quest);
        }
    }

    void Update()
    {
        foreach(Quest quest in questMap.Values){
            if(quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest)){
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }
    }

    private void ChangeQuestState(string id, QuestState state){
        Quest quest = GetQuestById(id);
        quest.state = state;
        GameManager.Instance.questEvents.QuestStateChange(quest);
    }

    private bool CheckRequirementsMet(Quest quest){
        bool meetsRequirements = true;

        foreach(QuestInfoSO prereqQuestInfo in quest.info.questPrerequisites){
            if(GetQuestById(prereqQuestInfo.id).state != QuestState.FINISHED){
                meetsRequirements = false;
            }
        }

        return meetsRequirements;
    }

    private void StartQuest(string id){
        Quest quest = GetQuestById(id);
        quest.InstatiateCurrentQuestStep(this.transform);
        ChangeQuestState(id, QuestState.IN_PROGRESS);
    }
    private void AdvanceQuest(string id){
        Quest quest = GetQuestById(id);

        quest.MoveToNextStep();

        if(quest.CurrentStepExists()){
            quest.InstatiateCurrentQuestStep(this.transform);
        }
        else{
            FinishQuest(id);
        }
    }
    private void FinishQuest(string id){
        Quest quest = GetQuestById(id);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);


        //DO SOMETHING LOL

    }

    private Dictionary<string, Quest> CreateQuestMap(){
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");

        Dictionary<string, Quest> idToQuestMap = new();
        foreach(QuestInfoSO questInfoSO in allQuests){
            if(idToQuestMap.ContainsKey(questInfoSO.id)){
                Debug.LogWarning("Adding Duplicate quest ID to quest map");
            }   
            idToQuestMap.Add(questInfoSO.id, new Quest(questInfoSO));
        }
        return idToQuestMap;
    }

    private Quest GetQuestById(string Id){
        Quest quest = questMap[Id];
        if (quest == null){
            Debug.LogError("Id not found in the quest map");
        }
        return quest;
    }
}
