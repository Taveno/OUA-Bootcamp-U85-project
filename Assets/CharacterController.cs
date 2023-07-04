using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{

    enum FM
    {
        Force = ForceMode.Force,
        Impulse = ForceMode.Impulse,
        Acceleration = ForceMode.Acceleration,
        VelocityChange = ForceMode.VelocityChange
    }

    private Rigidbody rb;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float stopSpeed = 2.0f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float climbSpeed = 3f;
    [SerializeField] private float raycastDistance = 10f;

    [SerializeField] LayerMask layerMask;

    [SerializeField] private FM forceMode = FM.Force;

    public bool isClimbable;

    [SerializeField] CapsuleCollider capsuleCollider;

    private Camera mainCamera;

    [SerializeField] Transform head;

    public Transform crosshair;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        mainCamera = Camera.main;
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        Climb();
        Crouch(); 
        LookMouse();
    }

    void LookMouse()
    {
        head.LookAt(crosshair);
    }

    private void Move()
    {
        bool isPressed = Input.GetKey(KeyCode.Space);

        float horizontal = Input.GetAxis("Horizontal");


        rb.AddForce(Vector3.forward * horizontal * Time.deltaTime * playerSpeed, (ForceMode)forceMode);
        
        if(horizontal < 0)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, 180f, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        else if(horizontal > 0)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        Debug.Log("velo: " + rb.velocity.z);
        if(rb.velocity.z > 0.01)
        {
            if(horizontal == 0)
                rb.AddForce(-Vector3.forward  * Time.deltaTime * stopSpeed, (ForceMode)forceMode);
            Debug.Log("+ yönü");
            
        }  
        else if(rb.velocity.z < -0.01)
        {
            if (horizontal == 0)
                rb.AddForce(Vector3.forward * Time.deltaTime * stopSpeed, (ForceMode)forceMode);
            Debug.Log("- yönü");
            
        }
        if (isPressed)
            Jump();//rb.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);

        Debug.DrawRay(transform.position, Vector3.down * raycastDistance, Color.blue);
    }

    void Climb()
    {
        if (isClimbable)
        {
            rb.useGravity = false;
            float vertical = Input.GetAxis("Vertical");
            rb.AddForce(Vector3.up * Time.deltaTime * climbSpeed * vertical, (ForceMode)forceMode);

            if (rb.velocity.y > 0.01)
            {
                if (vertical == 0)
                    rb.AddForce(-Vector3.up * Time.deltaTime * stopSpeed, (ForceMode)forceMode);
                Debug.Log("+ yönü");
            }
            else if (rb.velocity.y < -0.01)
            {
                if (vertical == 0)
                    rb.AddForce(Vector3.up * Time.deltaTime * stopSpeed, (ForceMode)forceMode);
                Debug.Log("- yönü");
            }
        }
        else
        {
            rb.useGravity = true;
        }

    }

    private void Jump()
    {
        // Yere doðru raycast ýþýnýný oluþtur
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        // Raycast'i gerçekleþtir ve isabet kontrol et
        if (Physics.Raycast(ray, out hit, raycastDistance, layerMask))
        {
            // Ýsabet eden nesnenin etiketini kontrol et
            if (hit.collider.CompareTag("Ground"))
            {
                rb.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
                Debug.DrawRay(transform.position, Vector3.down * raycastDistance, Color.red);
                // Hedefe isabet etti
                Debug.Log("Hedefe isabet etti!");
                // Hedefe yapýlacak iþlemleri burada gerçekleþtirin
            }
        }
       
    }
    void Crouch()
    {
        bool isCrouch = Input.GetKey(KeyCode.LeftControl);
        bool canStand = true;

        Debug.DrawRay(transform.position, Vector3.up * 1.2f, Color.green);
        if (Physics.Raycast(transform.position, Vector3.up, 1.2f, layerMask))
        {
            canStand = false;
            Debug.Log("Kalkabilir");
        }
            
        else
            canStand = true;

        if (isCrouch)
        {
            capsuleCollider.height = capsuleCollider.height / 2;
            capsuleCollider.center = new Vector3(0, -0.5f, 0);
        }
        else if(canStand && !isCrouch)
        {
            capsuleCollider.height = 2f;
            capsuleCollider.center = new Vector3(0, 0, 0);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Stairs"))
            isClimbable = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Stairs"))
            isClimbable = false;
    }
}
