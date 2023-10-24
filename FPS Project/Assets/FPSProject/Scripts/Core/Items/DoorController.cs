using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    private Animator m_Animator;
    private const string OPEN_TRIGGER_KEY = "Open";
    public void Open()
    {
        m_Animator.SetTrigger(OPEN_TRIGGER_KEY);
    }
}
