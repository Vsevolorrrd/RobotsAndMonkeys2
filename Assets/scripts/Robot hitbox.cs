using UnityEngine;

public class Robothitbox : MonoBehaviour
{
    Animator animator;
    bool PlayingAnimation;
    void Start()
    {
        PlayingAnimation = false;
        animator = GetComponent<Animator>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "bullet")
        {
            Destroy(this);
            Debug.Log("MONKEY IS DYING");
        }

        if(other.gameObject.tag == "bullet" && !PlayingAnimation)
        {
            animator.SetTrigger("BulletDeath");
            PlayingAnimation = true;
        }

        if (other.gameObject.tag == "Player" && !PlayingAnimation)
        {
            animator.SetTrigger("RobotDeath");
            PlayingAnimation = true;
        }
    }
}