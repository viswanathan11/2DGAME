using UnityEngine;

public class PlayHeartAnimation : MonoBehaviour
{
    public Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.Play("heart animation new"); // Replace with your animation name
    }
}