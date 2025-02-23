using UnityEngine;

public class MinigameAlarm : MonoBehaviour
{
    [SerializeField] Material baseMat;
    [SerializeField] Material litMat;
    [SerializeField] MeshRenderer m_Renderer;
    public void ToggleAlarm(bool on)
    {
        if (on) 
        {
            m_Renderer.material = litMat;
        }
        else
        {
            m_Renderer.material = baseMat;
        }
        
    }
}
