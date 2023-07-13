using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 120f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float climbSpeed = 100f;
    [SerializeField] private float raycastDistance = 10f;
    [SerializeField] Transform head;
    [SerializeField] LayerMask layerMask;
    [SerializeField] CapsuleCollider capsuleCollider;
    private Rigidbody rb;
    bool isClimbable;    
    private Camera mainCamera;    
    public Transform crosshair;
    public Animator anim;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        mainCamera = Camera.main;
        anim = GetComponent<Animator>();
    }

 
    void FixedUpdate()
    {
        Move();
        Climb();
        Crouch(); 
        LookMouse();
    }

    void Update()
    {
        bool isPressed = Input.GetKey(KeyCode.Space);
        if (isPressed)
            Jump();
    }

    void LookMouse()
    {
        head.LookAt(new Vector3(0f, crosshair.transform.position.y, crosshair.transform.position.z));
    }


    void Move()
    {
        

        float horizontal = Input.GetAxis("Horizontal");


        Vector3 move = new Vector3(rb.velocity.x, rb.velocity.y, playerSpeed * horizontal * Time.fixedDeltaTime);

        rb.velocity = move;

        if (horizontal < 0)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, 180f, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            anim.SetBool("isMove", true);
        }
        else if (horizontal > 0)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            anim.SetBool("isMove", true);
        }
        else
        {
            anim.SetBool("isMove", false);
        }
        
        

        Debug.DrawRay(transform.position, Vector3.down * raycastDistance, Color.blue);
        
    }

    void Climb()
    {
        if (!isClimbable)
        {
            rb.useGravity = true;
            return;
        }

        rb.useGravity = false;
        float vertical = Input.GetAxis("Vertical");
        Vector3 climb = new Vector3(rb.velocity.x, climbSpeed * Time.deltaTime * vertical, rb.velocity.z);
        rb.velocity = climb;
    }

    void Jump()
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
                rb.AddForce(new Vector3(0f, jumpForce * Time.deltaTime, 0f), ForceMode.Impulse);
                //rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.y);
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
            capsuleCollider.center = new Vector3(0, 0.5f, 0);
        }
        else if(canStand && !isCrouch)
        {
            capsuleCollider.height = 1f;
            capsuleCollider.center = new Vector3(0, 0.5f, 0);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Stairs"))
            isClimbable = true;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Stairs"))
            isClimbable = false;
    }

}
