using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;
using UnityEngine.UI;

public class ManualOverrideCode : MonoBehaviour
{
    const string glyphs = "ABCDFGHIJKLMNOPQRSTUVWXYZ01234567890123456789";
    [SerializeField] const int codeLength = 5;

    private bool isActive = false;
    [SerializeField] private LayerMask layerToIgnore;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI CodeDisplay;

    [SerializeField] private Image[] progressIcons;
    [SerializeField] private Sprite[] progSprites;
    bool generating = true;
    float generateTime = 2.5f;
    float curGenerateTime = 2.5f;
    int progress = 0;

    void Start()
    {
    }
    public void OnUpdate() //checking even when not playing, neeed to fix
    {
        if (isActive)
        {
            if (generating)
            {
                if(curGenerateTime < 0.0f)
                {
                    generating = false;
                    curGenerateTime = generateTime;
                }
                else
                {
                    string myString = "";
                    for (int i = 0; i < codeLength; i++)
                    {
                        myString += glyphs[UnityEngine.Random.Range(0, glyphs.Length)];
                    }
                    CodeDisplay.text = myString;
                    curGenerateTime -= Time.deltaTime;
                }
            }
            return;
        }
        isActive = true;
        canvas.enabled = true;
    }

    public void CheckInput()
    {
        string input = inputField.text.ToLower().Trim();
        string code = CodeDisplay.text.ToLower().Trim();
        if (code.Equals(input))
        {
            if (progress < progSprites.Length)
            {
                inputField.text = "";
                generating = true;
                progressIcons[progress].sprite = progSprites[1];
                progress++;
                //AudioManager.Instance.PlayOneShot(AudioManager.SmallSuccess);
            }
            else
            {
                progressIcons[progress].sprite = progSprites[1];
                CodeDisplay.text = "Manual Override Engaged";
            }
        }
        else
        {
            //AudioManager.Instance.PlayOneShot(AudioManager.Failure);
            Debug.Log(input.Length); 
            Debug.Log(code.Length);
        }
    }

    public void CleanUp()
    {
        progress = 0;
        foreach(Image i in progressIcons)
        {
            i.sprite = progSprites[0];
        }
        canvas.enabled = false;
    }
}
