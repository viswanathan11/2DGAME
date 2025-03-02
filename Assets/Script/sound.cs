using UnityEngine;
using UnityEngine.Audio;
public class sound : MonoBehaviour

{
    public AudioSource backgroundMusic;
    public AudioSource coinsound;
    public AudioSource sword;
   public void Coinsound(){
    coinsound.Play();
   }
    public void backgroundmusic(){
        backgroundMusic.Play();
    }
    public void Sword(){
        sword.Play();
    }

}
