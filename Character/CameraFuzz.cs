using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class CameraFuzz : MonoBehaviour
{
    [SerializeField] NavMeshAgent enemy;

    [SerializeField] CinemachineCamera cam;


    [SerializeField] SpriteRenderer fuzz;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (enemy == null)
            return;

        if (!PlayerManager.Instance.playerData.isCharacterActive)
        {
            fuzz.color = Color.clear;
            return;
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, enemy.transform.position - transform.position, out hit))
        {
            Debug.Log(hit.collider.gameObject.name);
            if (!hit.collider.CompareTag("Enemy"))
            {
                fuzz.color = Color.clear;
                return;
            }

            float Dist = Mathf.Min(12 / (hit.distance * hit.distance), 1);
            float angle = Vector3.Dot(cam.transform.forward.normalized, (enemy.transform.position - cam.transform.position).normalized);

            float shaderVal = Mathf.Min(Dist * Mathf.Min(angle + 0.25f, 1), 0.95f);
            fuzz.color = new Color(1, 1, 1, shaderVal); // Use to MapShader
        }
    }
}
