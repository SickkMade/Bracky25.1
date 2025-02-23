using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class Tablet : MonoBehaviour, IWeapon
{
    public GameObject WeaponObject => gameObject;

    [SerializeField] private Canvas tabletCanvas;
    [SerializeField] private CinemachineCamera tabletCam;


    [SerializeField] private GameObject mapScreenMenu;
    #region MAP MENU FIELDS
    [SerializeField] private TextMeshProUGUI ObjectiveTopText;
    [SerializeField] private GameObject Important;
    [SerializeField] private TextMeshProUGUI FlavorText;

    private static Color defaultMapButtonColor = new Color(1, 1, 1, 197 / 255);
    private static Color RedMapButtonColor = new Color(1, 0, 0, 255);
    [SerializeField] List<Image> _minigameButtons = new List<Image>();
    private Dictionary<Room, Image> minigameButtons = new Dictionary<Room, Image>();
    private static Dictionary<string, Room> roomDict = new Dictionary<string, Room>{
        { "Turbine", Room.Turbine },
        { "BoilerHouse", Room.BoilerHouse },
        { "Assembly", Room.Assembly },
        { "WaterTreatment", Room.WaterTreatment },
        { "Tokomak", Room.Tokomak },
        { "PumpHouse", Room.PumpHouse },
        { "General", Room.General },
        };
    #endregion

    [SerializeField] private GameObject messageMenu;
    #region MESSAGE FIELDS
    private List<StoryMessage> storyMessages = new List<StoryMessage>();
    private List<MessageButton> messageButtons = new List<MessageButton>();
    [SerializeField] private MessageButton messageButtonPrefab;
    [SerializeField] private TextMeshProUGUI Details;
    [SerializeField] private GameObject MessageScrollViewContainer;
    #endregion

    [SerializeField] private GameObject taskLogMenu;
    #region
    [SerializeField] private TextMeshProUGUI TaskName;
    [SerializeField] private TextMeshProUGUI TaskDescription;
    #endregion
    private CinemachineCamera mainCam;

    void Start()
    {
        Room[] rooms = roomDict.Values.ToArray();
        for (int i = 0; i < rooms.Length; i++)
        {
            minigameButtons.Add(rooms[i], _minigameButtons[i]);
        }
    }

    public void ActionOff()
    {
        tabletCanvas.enabled = false; 
        PlayerManager.Instance.ChangeCharacterActive(true);
        //move to start after moving player data set to awake
        mainCam = PlayerManager.Instance.playerData.playerCamera;
        tabletCam.Priority = 10;
        mainCam.Priority = 20;
    }

    public void ActionOn()
    {
        tabletCanvas.enabled = true;
        PlayerManager.Instance.ChangeCharacterActive(false);
        //move to start after moving player data set to awake
        mainCam = PlayerManager.Instance.playerData.playerCamera;
        tabletCam.Priority = 20;
        mainCam.Priority = 10;
    }

    public void MapPressed()
    {
        mapScreenMenu.SetActive(true);
        taskLogMenu.SetActive(false);
        messageMenu.SetActive(false);
    }

    public void TaskLogPressed()
    {
        mapScreenMenu.SetActive(false);
        taskLogMenu.SetActive(true);
        messageMenu.SetActive(false);

        // Change Task Fields to Reflect Current Task
    }


    public void MessagesPressed()
    {
        foreach (var item in messageButtons)
        {
            Destroy(item.gameObject);
        }
        mapScreenMenu.SetActive(false);
        taskLogMenu.SetActive(false);
        messageMenu.SetActive(true);

        for (int i = 0; i < storyMessages.Count; i++)
        {
            var msgButton = Instantiate(messageButtonPrefab);
            msgButton.ButtonInit(this, i, storyMessages[i]);
            msgButton.transform.SetParent(MessageScrollViewContainer.transform);
            msgButton.transform.localScale = Vector3.one;
        }
    }

    /// <summary>
    /// Call in GameManager.
    /// </summary>
    /// <param name="MinigameID"></param>
    /// <param name="needToDo"></param>
    public void UpdateMinigameStatus(Room room, bool needToDo)
    {
        if (minigameButtons.ContainsKey(room))
        {
            if (needToDo)
            {
                minigameButtons[room].color = RedMapButtonColor;
            }
            else
            {
                minigameButtons[room].color = defaultMapButtonColor;
            }
        }
    }

    public void ClickMapButton(string room)
    {
        Room btnPressed = roomDict[room];
        // Change Text, Set Important Visible, set FlavorText
    }

    public void AddMessage(StoryMessage msg)
    {
        storyMessages.Add(msg);
    }

    public void ClickMessageButton(int index)
    {
        StoryMessage msg = storyMessages[index];
        Details.text = msg.Content;
        msg.Opened = true;
        messageButtons[index].Read = true;
    }

}
