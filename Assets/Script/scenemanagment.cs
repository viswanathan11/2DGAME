using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class scenemanagment : MonoBehaviour
{
void Start()
    {
    }

    public void play()
{
    SceneManager.LoadScene(1);
}
public void exit(){
    Debug.Log("Exit");
    Application.Quit();
}
 public void Menu(){

 }
 public void NextBtn(){

 }
}
