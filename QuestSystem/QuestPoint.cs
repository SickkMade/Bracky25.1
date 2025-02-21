using UnityEngine;

public class QuestPoint : MonoBehaviour
{
    //impliment to tablet.
    //need to call startquest and endquest
    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoForPoint;

    private string questId;
    private QuestState currentQuestState;

    void Awake()
    {
        questId = questInfoForPoint.id;
    }

    void OnEnable()
    {
        GameManager.Instance.questEvents.OnQuestStateChange += QuestStateChange;
    }
    void OnDisable()
    {
        GameManager.Instance.questEvents.OnQuestStateChange -= QuestStateChange;
    }

    private void QuestStateChange(Quest quest){
        if(quest.info.id.Equals(questId)){
            currentQuestState = quest.state;
            Debug.Log($"Quest with id: {questId} updated to state {currentQuestState}");
        }
    }

}
