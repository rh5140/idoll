using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private Animator animator;
    private IEnumerator movement_coroutine;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int player_direction = target.GetComponent<PlayerMovement>().GetFaceDirection();
        float player_x = target.transform.position.x;
        float player_y = target.transform.position.y;
        float companion_x = this.transform.position.x;
        float companion_y = this.transform.position.y;

        switch (player_direction)
        {
            case 0:
                if (companion_y < player_y)
                {
                    // StopCoroutine(movement_coroutine);
                }
                else
                {
                    // this.transform.position = new Vector2(player_x, player_y + 1);
                    movement_coroutine = MoveHelper(new Vector2(player_x, player_y + 1), 0.025f);
                    StartCoroutine(movement_coroutine);
                }
                break;
            case 1:
                if (companion_y > player_y)
                {
                    // StopCoroutine(movement_coroutine);
                }
                else
                {
                    // this.transform.position = new Vector2(player_x, player_y - 1);
                    movement_coroutine = MoveHelper(new Vector2(player_x, player_y - 1), 0.025f);
                    StartCoroutine(movement_coroutine);
                }
                break;
            case 2:
                if (companion_x < player_x)
                {
                    // StopCoroutine(movement_coroutine);
                }
                else
                {
                    // this.transform.position = new Vector2(player_x + 1, player_y + 0.5f);
                    movement_coroutine = MoveHelper(new Vector2(player_x + 1, player_y + 0.5f), 0.025f);
                    StartCoroutine(movement_coroutine);
                }
                break;
            case 3:
                if (companion_x > player_x)
                {
                    // StopCoroutine(movement_coroutine);
                }
                else
                {
                    // this.transform.position = new Vector2(player_x - 1, player_y + 0.5f);
                    movement_coroutine = MoveHelper(new Vector2(player_x - 1, player_y + 0.5f), 0.025f);
                    StartCoroutine(movement_coroutine);
                }
                break;
            default:
                break;
        }
    }

    private IEnumerator MoveHelper(Vector2 destination, float seconds)
    {
        float elapsedTime = 0;
        Vector2 startingPos = this.transform.position;
        while (elapsedTime < seconds)
        {
            this.transform.position = Vector2.Lerp(startingPos, destination, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        this.transform.position = destination;

        int player_direction = target.GetComponent<PlayerMovement>().GetFaceDirection();
        Vector2 movement = new Vector2(0f, -1f);
        switch(player_direction)
        {
            case 0: // Down
                break;
            case 1: // Up
                movement = new Vector2(0f, 1f);
                break;
            case 2: // Left
                movement = new Vector2(-1f, 0f);
                break;
            case 3: // Right
                movement = new Vector2(1f, 0f);
                break;
            default:
                break;
        }

        //send value to animator parameters
        animator.SetFloat("xAxis", movement.x);
        animator.SetFloat("yAxis", movement.y);
        animator.SetFloat("magnitude", 1);
    }
}
