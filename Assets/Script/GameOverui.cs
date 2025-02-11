using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverui : MonoBehaviour
{

public void Menu(){
Debug.Log("Menu");

SceneManager.LoadScene("Menu");
}
public void Retry(){
Debug.Log("Retry");
SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
    
}
