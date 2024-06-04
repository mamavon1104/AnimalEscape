using UnityEngine;

public class EndPressButtonCS : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKeyDown)
            RoadScene();
    }
    private void RoadScene() => GetComponent<ChangeSceneMaster>().RoadScene();
}
