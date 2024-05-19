using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float _tiltAmount = 5;
    [SerializeField] private float _rotationSpeed = 0.5f;
    [SerializeField] private float _rayDistance;

    [SerializeField] private float _sensX;
    [SerializeField] private float _sensY;

    [SerializeField] private Transform _orentation;
    [SerializeField] private Animator _anim;

    private float xRotation;
    private float yRotation;

    private bool _canLook;

    private Ray ray;
    private RaycastHit hit;

    private Movement _movement;

    //[Header("debug")]
    //[SerializeField] private string _canLookDebug;

    void Start()
    {
        _canLook = true;
        Cursor.lockState = CursorLockMode.Locked;
        _anim.enabled = false;

        _movement = gameObject.GetComponentInParent<Movement>();
    }

    void Update()
    {
        //_canLookDebug = _canLook.ToString();
        
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (_canLook)
        {
            Tilt();
            float mouseX = Input.GetAxisRaw("Mouse X") * _sensX * Time.deltaTime;
            float mouseY = Input.GetAxisRaw("Mouse Y") * _sensX * Time.deltaTime;

            yRotation += mouseX;    

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            Vector3 v = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(xRotation, yRotation, v.z);
            _orentation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
       
        Debug.DrawRay(transform.position, ray.direction * _rayDistance);
        RaycastToInseract();
    }

    private void Tilt()
    {
        float rotationZ = -Input.GetAxis("Horizontal") * _tiltAmount;

        Quaternion Rot = Quaternion.Euler(xRotation, yRotation, rotationZ);
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Rot, _rotationSpeed);
    }

    private void RaycastToInseract()
    {
        if (Physics.Raycast(ray, out hit, _rayDistance) && Input.GetKeyDown(KeyCode.E) && _canLook)
        {
            switch (hit.collider.gameObject.tag)
            {
                case "PC":
                    _canLook = false;
                    _anim.enabled = true;
                    _movement.canMove = false;

                    _anim.SetBool("UsePC", true);

                    Cursor.lockState = CursorLockMode.Confined;
                    break;

                case "Lightswitch":
                    hit.collider.gameObject.GetComponent<LightswitchController>().InteractSwitch();
                    break;
            }
        }

        if (Input.GetKey(KeyCode.S) && !_canLook)
        {
            _anim.SetBool("UsePC", false);
            Cursor.lockState = CursorLockMode.Locked;
            Invoke(nameof(DisableAnimator), 0.2f);
        }
    }

    private void DisableAnimator()
    {
        yRotation += transform.rotation.x;

        xRotation -= transform.rotation.y;

        _anim.enabled = false;
        _canLook = true;
        _movement.canMove = true;
    }

}
