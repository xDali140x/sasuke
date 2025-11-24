using UnityEngine;
using UnityEngine.SceneManagement;

public class Cambalache : MonoBehaviour
{
   public string level;
   private void OnTriggerEnter(Collider other)
   {
       if (other.CompareTag("Player"))
       {
           SceneManager.LoadScene(level);
       }
   }
}
