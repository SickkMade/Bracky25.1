using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MiniGameScript))]
public class AirConReplacmentMinigame : MonoBehaviour
{
    

    [SerializeField] AirFilter currentFilter;
    [SerializeField] Canvas canvas;
    [SerializeField] Vector3[] filterSpawnLocs;
    [SerializeField] AirFilter filterPrefab;


    private bool isShowing = false;
    [SerializeField] float timeToShow = 2f;
    private float curShowTime = 0.0f;
    MiniGameScript gameScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameScript = GetComponent<MiniGameScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowFilter()
    {
        if (isShowing)
        {
            curShowTime -= Time.deltaTime;
            if(curShowTime < 0.0f)
            {
                gameScript.ExitGame();
            }
        }
        else
        {
            isShowing = true;
            curShowTime = timeToShow;
            canvas.enabled = true;
        }
    }

    public void SpawnFilters()
    {
        currentFilter.IsGood = false;
        int spawnIndex = Random.Range(0, filterSpawnLocs.Length);
        Instantiate(filterPrefab, filterSpawnLocs[(spawnIndex) % filterSpawnLocs.Length], Quaternion.identity);
    }

    public void CleanUpScript()
    {
        isShowing = false;
        canvas.enabled = false;
    }

    public void InteractWithGoodFilter()
    {
        if (currentFilter.Dropped)
        {
            AirFilter newFilter = Instantiate(filterPrefab, this.transform);
            newFilter.transform.localPosition += new Vector3(-0.02f, 0.2f, 1.42f);
            currentFilter = newFilter;
            GameManager.Instance.MinigameCompleted();
        }
    }
}
