using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterLookAim : MonoBehaviour
{
    [SerializeField] Transform crosshair; // Assign the crosshair object in the Inspector
    [SerializeField] GameObject objectToThrow;
    [SerializeField] float distance;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float throwSpeed = 10f;
    [SerializeField] private bool isGone = false;
    [SerializeField] GameObject portalPrefab;
    public List<GameObject> currentPortals;
    public int count = 0;
    private Camera mainCamera;
    private float distanceFromCamera;
    bool isClick;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        // Calculate the initial distance between the crosshair and the camera
        distanceFromCamera = Vector3.Distance(crosshair.position, mainCamera.transform.position);
    }

    private void Update()
    {
        // Get the mouse position on the screen
        Vector3 mouseScreenPos = Input.mousePosition;
        isClick = Input.GetMouseButtonDown(0);
        // Set the crosshair position to the mouse screen position
        crosshair.position = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, distanceFromCamera + distance));
        Vector3 targetPosition = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, distanceFromCamera + distance));
        if (isClick)
        {
            GameObject thrownObject = Instantiate(objectToThrow, spawnPoint.position, Quaternion.identity);
            Rigidbody thrownObjectRigidbody = thrownObject.AddComponent<Rigidbody>();
            thrownObjectRigidbody.useGravity = false;
            StartCoroutine(ThrowObject(thrownObject, targetPosition));
        }

        if (isGone && Vector3.Distance(objectToThrow.transform.position, targetPosition) < 0.1f)
        {
            isGone = false;
            // Ýþlem tamamlandýktan sonra yapýlacak iþlemler
        }

    }

    public List<GameObject> GetPortalList()
    {
        return currentPortals;
    }

    private IEnumerator ThrowObject(GameObject thrownObject, Vector3 targetPosition)
    {
        Vector3 startPosition = thrownObject.transform.position;
        float elapsedTime = 0f;
        float throwDuration = Vector3.Distance(startPosition, targetPosition) / throwSpeed;

        while (elapsedTime < throwDuration)
        {
            thrownObject.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / throwDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        thrownObject.transform.position = targetPosition;
        Vector3 eulerRotation = new Vector3(90, 90, 0);
        Quaternion rotationQuaternion = Quaternion.Euler(eulerRotation);
        var portal = Instantiate(portalPrefab, new Vector3(-1.3f, thrownObject.transform.position.y, thrownObject.transform.position.z), rotationQuaternion);
        thrownObject.GetComponent<Rigidbody>().isKinematic = true;
        portal.AddComponent<PortalTrigger>();

        Destroy(thrownObject);
        count++;

        if (currentPortals.Count == 2)
        {
            if (count % 2 == 1)
            {
                Destroy(currentPortals[0]);
                currentPortals.RemoveAt(0);
                currentPortals.Insert(0, portal);
            }
            else
            {
                Destroy(currentPortals[1]);
                currentPortals.RemoveAt(1);
                currentPortals.Insert(1, portal);
            }
        }
        else
        {
            currentPortals.Add(portal);
        }
        isGone = true;
    }
}
