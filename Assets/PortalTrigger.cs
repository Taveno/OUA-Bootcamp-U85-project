using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    public List<GameObject> asd;
    public CharacterLookAim ac;
    private int teleportTriggerIndex = -1;

    [SerializeField] Transform player;


    bool isPush;
    private void Start()
    {
        ac = FindObjectOfType<CharacterLookAim>();
        if (ac == null)
        {
            Debug.LogError("CharacterLookAim component could not be found.");
        }
        asd = ac.GetPortalList();

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
            if (isPush)
            {
                player.transform.position = new Vector3(player.transform.position.x, asd[teleportTriggerIndex].transform.position.y, asd[teleportTriggerIndex].transform.position.z);
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

            if (asd.IndexOf(this.gameObject) == 0)
            {
                Debug.Log("cuerrent index 0");
                teleportTriggerIndex = 1;
            }
            else if (asd.IndexOf(this.gameObject) == 1)
            {
                Debug.Log("cuerrent index 1");
                teleportTriggerIndex = 0;
            }
        }
    }
}
