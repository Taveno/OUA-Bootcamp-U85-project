using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
public class PortalTrigger : MonoBehaviour
{
    [SerializeField] List<GameObject> portalClone;
    [SerializeField] CharacterLookAim crosshair;
    [SerializeField] Transform player;
    [SerializeField] float cooldownDuration = 3f;

    [SerializeField] Button pressFButton; 
    [SerializeField] TMP_Text tmpF; 

    private int teleportTriggerIndex = -1;
    bool isPush;
    bool hasCooldown;

    CharacterWeaponManager characterWeaponManager;


    private void Start()
    {
        characterWeaponManager = FindObjectOfType<CharacterWeaponManager>();

        pressFButton = GameObject.Find("Press F").GetComponent<Button>();
        tmpF = GameObject.Find("TMPF").GetComponent<TMP_Text>();
        //Baslangicta sifirla for 'F'
        pressFButton.image.enabled = false;
        tmpF.text = "";

        crosshair = FindObjectOfType<CharacterLookAim>();
        if (crosshair == null)
        {
            Debug.LogError("CharacterLookAim component could not be found.");
        }
        portalClone = crosshair.GetPortalList();

        player = FindObjectOfType<CharacterController>().transform;
    }

    private void Update()
    {  
         isPush = Input.GetKey(KeyCode.F);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isPush && characterWeaponManager.hasCoolDown)
            {
                isPush = false;
                player.transform.position =
                    new Vector3(player.transform.position.x, portalClone[teleportTriggerIndex].transform.position.y, portalClone[teleportTriggerIndex].transform.position.z);
                characterWeaponManager.DisableCoolDown();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pressFButton.image.enabled = false;
            tmpF.text = "";

            teleportTriggerIndex = -1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pressFButton.image.enabled = true;
            tmpF.text = "F";
            if (portalClone.IndexOf(this.gameObject) == 0)
            {
                Debug.Log("current index 0");
                teleportTriggerIndex = 1;
            }
            else if (portalClone.IndexOf(this.gameObject) == 1)
            {
                Debug.Log("current index 1");
                teleportTriggerIndex = 0;
            }
        }
    }
}