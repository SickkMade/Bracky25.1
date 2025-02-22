using UnityEngine;
using TMPro;

public class EnterCodeMission : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    public void CheckInput()
    {
        string input = inputField.text.ToLower().Trim();
        GameManager.Instance.InvokeEnterCodeEvent(input);
    }
}
