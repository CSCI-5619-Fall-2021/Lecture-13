using UnityEngine;

public class Grabbable : MonoBehaviour
{

    private Grabber currentGrabber;

    // Start is called before the first frame update
    void Start()
    {
        currentGrabber = null;
    }

    public void SetCurrentGrabber(Grabber grabber)
    {
        currentGrabber = grabber;
    }

    public Grabber GetCurrentGrabber()
    {
        return currentGrabber;
    }
}
