using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MessageButton : MonoBehaviour
{
    private Tablet UIController;
    [SerializeField] private Button btn;
    [SerializeField] private TextMeshProUGUI SenderName;
    [SerializeField] private Image msgReadIcon;
    [SerializeField] private Image msgBackGround;
    [SerializeField] private Sprite readSprite;
    [SerializeField] private Sprite unreadSprite;
    [SerializeField] private Sprite readBKGSprite;
    [SerializeField] private Sprite unreadBKGSprite;
    private StoryMessage storyMessage;

    bool _read = false;
    public bool Read
    {
        get => _read; 
        set{
            if (value)
            {
                msgReadIcon.sprite = readSprite;
                msgBackGround.sprite = readBKGSprite;
            }
            else
            {
                msgReadIcon.sprite = unreadSprite;
                msgBackGround.sprite = unreadBKGSprite;
            }

            _read = value;
        }
    }

    public void ButtonInit(Tablet tb, int index, StoryMessage message)
    {
        storyMessage = message;
        SenderName.text = message.name;
        Read = message.Opened;
        btn.onClick.AddListener(
            delegate
            {
                tb.ClickMessageButton(index);
            }
        );
    }
}
