using UnityEngine;
using UnityEngine.InputSystem;

public class Pointer : MonoBehaviour
{
    public LineRenderer laserPointer;
    public Material grabbablePointerMaterial;

    public InputActionProperty touchAction;
    public InputActionProperty grabAction;

    Material lineRendererMaterial;
    Transform grabPoint;
    Grabbable grabbedObject;
    Transform initialParent;

    // Start is called before the first frame update
    void Start()
    {
        laserPointer.enabled = false;
        lineRendererMaterial = laserPointer.material;

        grabPoint = new GameObject().transform;
        grabPoint.name = "Grab Point";
        grabPoint.parent = this.transform;
        grabbedObject = null;
        initialParent = null;

        grabAction.action.performed += Grab;
        grabAction.action.canceled += Release;

        touchAction.action.performed += TouchDown;
        touchAction.action.canceled += TouchUp;
    }

    private void OnDestroy()
    {
        grabAction.action.performed -= Grab;
        grabAction.action.canceled -= Release;

        touchAction.action.performed -= TouchDown;
        touchAction.action.canceled -= TouchUp;
    }

    // Update is called once per frame
    void Update()
    {
        if (laserPointer.enabled)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                laserPointer.SetPosition(1, new Vector3(0, 0, hit.distance));

                if (hit.collider.GetComponent<Grabbable>())
                {
                    laserPointer.material = grabbablePointerMaterial;
                }
                else
                {
                    laserPointer.material = lineRendererMaterial;
                }
            }
            else
            {
                laserPointer.SetPosition(1, new Vector3(0, 0, 100));
                laserPointer.material = lineRendererMaterial;
            }
        }
    }

    void Grab(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            if (hit.collider.GetComponent<Grabbable>())
            {
                grabPoint.localPosition = new Vector3(0, 0, hit.distance);

                grabbedObject = hit.collider.GetComponent<Grabbable>();

                if (grabbedObject.GetComponent<Rigidbody>())
                {
                    grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                    grabbedObject.GetComponent<Rigidbody>().useGravity = false;
                }

                initialParent = grabbedObject.transform.parent;
                grabbedObject.transform.parent = grabPoint;
            }
        }
    }

    void Release(InputAction.CallbackContext context)
    {
        if (grabbedObject)
        {
            if (grabbedObject.GetComponent<Rigidbody>())
            {
                grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                grabbedObject.GetComponent<Rigidbody>().useGravity = true;
            }

            grabbedObject.transform.parent = initialParent;
            grabbedObject = null;
        }
    }

    void TouchDown(InputAction.CallbackContext context)
    {
        laserPointer.enabled = true;
    }

    void TouchUp(InputAction.CallbackContext context)
    {
        laserPointer.enabled = false;
    }
}
