using UnityEngine;

public class OnTrigger : MonoBehaviour
{
    [SerializeField] ChangeSceneMaster m_sceneMaster;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            m_sceneMaster.RoadScene();
    }
}
