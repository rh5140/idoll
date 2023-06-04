using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    private UnityEngine.Vector2 targetPos = new UnityEngine.Vector2(0.0f, 0.0f);
    private UnityEngine.Vector2 pointPos = new UnityEngine.Vector2(0.0f, 0.0f);
    [SerializeField]
    private UnityEngine.Vector2 winPos = new UnityEngine.Vector2(0.0f, 0.0f);
    private PlayerMovement PlrMovement;
    public bool game_active = true;
    public void MoveObjTo(UnityEngine.Vector2 movePos, UnityEngine.Vector2 plrPos, bool skip_adjust = false)
    {
        Debug.Log("NEW POS: ");
        Debug.Log(movePos.y);
        Debug.Log(transform.position.y);
        pointPos = new UnityEngine.Vector2(plrPos.x + movePos.x, plrPos.y + movePos.y);
        print(pointPos);
        if (winPos == targetPos)
            return;
        targetPos = new UnityEngine.Vector2(pointPos.x + movePos.x, pointPos.y + movePos.y);
        if (skip_adjust)
        {
            targetPos = pointPos;
            pointPos = plrPos;
        }
        print(targetPos);
    }
    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
        pointPos = transform.position;
        PlrMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlrMovement.broom_game)
            return;
        transform.position = UnityEngine.Vector3.MoveTowards(transform.position, targetPos, 5f * Time.deltaTime);
        if (transform.position.x == winPos.x && transform.position.y == winPos.y && game_active)
        {
            targetPos = winPos;
            PlrMovement.SetBroomGame(false);
            game_active = false;
            Debug.Log("You Won!");
        }
        ///if (transform.position.x == targetPos.x && transform.position.y == targetPos.y)
          //  pointPos = transform.position;
    }
}
