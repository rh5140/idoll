using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Tooltip("Drag in 'PlayerSprite (Animator)'")]
    public Animator animator;

    private void SpriteAnimation()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);

        //send value to anitatior parameters
        animator.SetFloat("xAxis", movement.x);
        animator.SetFloat("yAxis", movement.y);
        animator.SetFloat("magnitude", movement.magnitude);
    }

    void Update()
    {
        SpriteAnimation();
    }

}
