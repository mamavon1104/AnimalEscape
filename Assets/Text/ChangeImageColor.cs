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
            Debug.Log("image‚ª‘¶İ‚µ‚Ä‚¢‚Ü‚¹‚ñB");
        }

        Assert.IsNotNull(myRig, "myRig‚ª‚ ‚è‚Ü‚¹‚ñB");
        Assert.IsNotNull(gameObj, "gameObj‚ª‚ ‚è‚Ü‚¹‚ñB");
    }    
}
