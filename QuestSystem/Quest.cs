using UnityEngine;

public class Quest 
{
    public QuestInfoSO info;

    public QuestState state;
    private int currentQuestStepIndex;

    public Quest(QuestInfoSO questInfo){
        this.info = questInfo;
        this.state = QuestState.REQUIREMENTS_NOT_MET;
        this.currentQuestStepIndex = 0;
    }

    public void MoveToNextStep(){
        currentQuestStepIndex++;
    }

    public bool CurrentStepExists(){
        return currentQuestStepIndex < info.questStepPrefabs.Length;
    }

    public void InstatiateCurrentQuestStep(Transform parentTransform){
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if(questStepPrefab){
            QuestStep queststep = Object.Instantiate<GameObject>(questStepPrefab, parentTransform)
                .GetComponent<QuestStep>();
            queststep.InitializeQuestStep(info.id);
        }
    }

    private GameObject GetCurrentQuestStepPrefab(){
        GameObject questStep = null;
        if(CurrentStepExists()){
            questStep = info.questStepPrefabs[currentQuestStepIndex];
        }
        else{
            Debug.LogWarning("Quest Step Out Of Range");
        }
        return questStep;
    }
}
