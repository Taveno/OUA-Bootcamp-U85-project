using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    private Animator _anim;
    private Rigidbody rb;
    [SerializeField] private float playerSpeed = 275f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float climbSpeed = 300f;
    [SerializeField] private float raycastDistance = 1f;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Transform head;
    public Transform crosshair;
    public bool isClimbable;
    CapsuleCollider capsuleCollider;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        _anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        LookMouse();
        Crouch();
        Climb();
    }

    void LookMouse()
    {
        head.LookAt(new Vector3(0f, crosshair.transform.position.y, crosshair.transform.position.z));
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector3 inputMove = context.ReadValue<Vector3>();
        Vector3 move = new Vector3(rb.velocity.x, rb.velocity.y, inputMove.z * playerSpeed * Time.deltaTime);
        rb.velocity = move;
        if(inputMove.z != 0)
        {
            _anim.SetBool("isMoving", true);
        }
        else
        {
            _anim.SetBool("isMoving", false);
        }
        Debug.Log("velo: " + rb.velocity.z);
        Debug.DrawRay(transform.position, Vector3.down * raycastDistance, Color.blue);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        // Yere doðru raycast ýþýnýný oluþtur
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        // Raycast'i gerçekleþtir ve isabet kontrol et
        if (Physics.Raycast(ray, out hit, raycastDistance, layerMask))
        {
            // Ýsabet eden nesnenin etiketini kontrol et
            if (hit.collider.CompareTag("Ground") && context.performed)
            {
                rb.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
                Debug.DrawRay(transform.position, Vector3.down * raycastDistance, Color.red);
                // Hedefe isabet etti
                Debug.Log("Hedefe isabet etti!");
                // Hedefe yapýlacak iþlemleri burada gerçekleþtirin
            }
        }

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

    void Crouch()
    {
        bool isCrouch = Keyboard.current.leftCtrlKey.isPressed;
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
        else if (canStand && !isCrouch)
        {
            capsuleCollider.height = 1f;
            capsuleCollider.center = new Vector3(0, 0.5f, 0);
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
