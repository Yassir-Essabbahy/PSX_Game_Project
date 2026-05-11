using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToStore : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.instance.phase == 1)
            SceneManager.LoadScene("StoreScene");
    } 
}