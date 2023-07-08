using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PortalTrigger : MonoBehaviour
{
    [SerializeField] List<GameObject> portalClone;
    [SerializeField] CharacterLookAim crosshair;
    [SerializeField] Transform player;
    [SerializeField] float cooldownDuration = 3f;
    private int teleportTriggerIndex = -1;
    bool isPush;
    bool hasCooldown;
    private void Start()
    {
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
        if(!hasCooldown) isPush = Input.GetKeyDown(KeyCode.F);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isPush)
            {
                isPush = false;
                StartCoroutine(PortalCooldown());
                player.transform.position = 
                    new Vector3(player.transform.position.x, portalClone[teleportTriggerIndex].transform.position.y, portalClone[teleportTriggerIndex].transform.position.z);               
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            teleportTriggerIndex = -1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

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

    private IEnumerator PortalCooldown()
    {
        hasCooldown = true;
        Debug.Log("Can't Teleport");
        yield return new WaitForSeconds(cooldownDuration);
        Debug.Log("Can Teleport");
        hasCooldown = false;
    }
}
