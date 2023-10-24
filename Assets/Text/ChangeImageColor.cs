using UnityEngine;
using UnityEngine.Assertions;

public class ChangeImageColor : MonoBehaviour
{
    [SerializeField] Rigidbody myRig;
    [SerializeField] GameObject gameObj;

    void Start()
    {
        var image = GetComponent<UnityEngine.UI.Image>();
        try
        {
            image.color = Color.black;
        }
        catch(System.NullReferenceException)
        {
            Debug.Log("image�����݂��Ă��܂���B");
        }

        Assert.IsNotNull(myRig, "myRig������܂���B");
        Assert.IsNotNull(gameObj, "gameObj������܂���B");
    }    
}
