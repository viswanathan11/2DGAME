using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverui : MonoBehaviour
{

public void Menu(){
SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
public void Retry(){
SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
    
}
