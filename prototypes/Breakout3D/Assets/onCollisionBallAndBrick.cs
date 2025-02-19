using UnityEngine;

public class onCollisionBallAndBrick : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioSource hitSound;

    void OnCollisionEnter()
    {
        hitSound.Play();
    }
}
