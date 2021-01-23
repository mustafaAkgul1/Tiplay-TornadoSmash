using UnityEngine;

public class TornadoMovementController : MonoBehaviour
{
    public static TornadoMovementController _instance;

    [Header("General Variables")]
    public bool canMove = true;
    public ControlTypes controlType;
    public float moveSpeed; // for joystick type
    public float sensitivityMultiplier;
    public float deltaThreshold;
    Vector2 firstTouchPosition;
    float finalTouchX, finalTouchZ;
    Vector2 curTouchPosition;
    float minXPos;
    float maxXPos;
    float minZPos;
    float maxZPos;

    [Header("References")]
    public DynamicJoystick joystick;
    public Transform leftBoundryTransform;
    public Transform rightBoundryTransform;
    public Transform frontBoundryTransform;
    public Transform backBoundryTransform;
    public CapsuleCollider centerCollider;
    Rigidbody rbTornado;
    Camera mainCam;

    public enum ControlTypes
    {
        Slide,
        Joystick
    }

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        AttachReferences();
        ResetInputValues();
        AssignBoundryValues();

    } // Start()

    void Update()
    {
        if (canMove)
        {
            switch (controlType)
            {
                case ControlTypes.Slide:
                    HandleMovementWithSlide();
                    break;
                case ControlTypes.Joystick:
                    HandleMovementWithJoystick();
                    break;
                default:
                    break;
            }
        }

    } // Update()

    void AttachReferences()
    {
        rbTornado = GetComponent<Rigidbody>();
        mainCam = Camera.main;

    } // AttachReferences()

    void AssignBoundryValues()
    {
        minXPos = leftBoundryTransform.position.x;
        maxXPos = rightBoundryTransform.position.x;
        minZPos = backBoundryTransform.position.z;
        maxZPos = frontBoundryTransform.position.z;

    } // AssignBoundryValues()

    void ResetInputValues()
    {
        rbTornado.velocity = Vector3.zero;
        firstTouchPosition = Vector2.zero;
        finalTouchX = 0f;
        finalTouchZ = 0f;
        curTouchPosition = Vector2.zero;

    } // ResetInputValues()

    void HandleMovementWithSlide()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstTouchPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            curTouchPosition = Input.mousePosition;
            Vector2 touchDelta = (curTouchPosition - firstTouchPosition);

            if (firstTouchPosition == curTouchPosition)
            {
                rbTornado.velocity = Vector3.zero;
            }

            //Debug.Log("firstTPos : " + firstTouchPosition + " - curTPos : " + curTouchPosition + " - touchDelta : " + touchDelta);
            finalTouchX = transform.position.x;
            finalTouchZ = transform.position.z;

            if (Mathf.Abs(touchDelta.x) >= deltaThreshold)
            {
                finalTouchX = (transform.position.x + (touchDelta.x * sensitivityMultiplier));
            }

            if (Mathf.Abs(touchDelta.y) >= deltaThreshold)
            {
                finalTouchZ = (transform.position.z + (touchDelta.y * sensitivityMultiplier));
            }
            
            rbTornado.position = new Vector3(finalTouchX, transform.position.y, finalTouchZ);
            rbTornado.position = new Vector3(Mathf.Clamp(rbTornado.position.x, minXPos, maxXPos), rbTornado.position.y, Mathf.Clamp(rbTornado.position.z, minZPos, maxZPos));

            firstTouchPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            ResetInputValues();
        }

    } // HandleMovementWithSlide()

    void HandleMovementWithJoystick()
    {
        float vInput = joystick.Vertical;
        float hInput = joystick.Horizontal;

        Vector3 nInput = new Vector3(hInput, 0f, vInput).normalized;

        if (nInput.magnitude >= 0.1f)
        {
            Vector3 movementInput = nInput * moveSpeed * Time.deltaTime;
            rbTornado.velocity = new Vector3(movementInput.x, 0f, movementInput.z);
        }
        else
        {
            rbTornado.velocity = Vector3.zero;
        }

    } // HandleMovementWithJoystick()

    public void TriggerLevelFinished()
    {
        canMove = false;
        rbTornado.useGravity = false;
        rbTornado.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        rbTornado.isKinematic = true;

    } // TriggerLevelFinished()

} // class
