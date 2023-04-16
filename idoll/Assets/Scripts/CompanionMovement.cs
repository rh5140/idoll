using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionMovement : MonoBehaviour
{
    [SerializeField] private GameObject target;
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

        switch (player_direction)
        {
            case 0:
                if (this.transform.position.y < player_y)
                {
                }
                else
                {
                    this.transform.position = new Vector2(player_x, player_y + 1);
                }
                break;
            case 1:
                if (this.transform.position.y > player_y)
                {
                }
                else
                {
                    this.transform.position = new Vector2(player_x, player_y - 1);
                }
                break;
            case 2:
                if (this.transform.position.x < player_x)
                {
                }
                else
                {
                    this.transform.position = new Vector2(player_x + 1, player_y);
                }
                break;
            case 3:
                if (this.transform.position.x > player_x)
                {
                }
                else
                {
                    this.transform.position = new Vector2(player_x - 1, player_y);
                }
                break;
            default:
                break;
        }
    }
}
