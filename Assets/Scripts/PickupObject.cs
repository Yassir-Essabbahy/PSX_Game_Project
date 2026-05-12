using UnityEngine;

public class PickupObject : MonoBehaviour
{
    [Header("Pickup Settings")]
    public float pickupDistance = 3f;
    public Transform holdPoint;

    [Header("Player State")]
    public bool hasMoney = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            TryPickup();
    }

    void TryPickup()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupDistance))
        {
            if (hit.collider.CompareTag("Money"))
            {
            GameManager.instance.hasMoney = true;
            Destroy(hit.collider.gameObject);
            Debug.Log("Money picked up!");
            }
            
            else if (hit.collider.CompareTag("Pickup"))
            {
                GameObject pickedObject = hit.collider.gameObject;
                pickedObject.transform.SetParent(holdPoint);
                pickedObject.transform.localPosition = Vector3.zero;
                pickedObject.transform.localRotation = Quaternion.identity;
            }
        }
    }
}