using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera PlayerCamera;
    [SerializeField] CinemachineVirtualCamera CutsceneCamera;

    private CinemachineVirtualCamera active_camera = null;

    private void Start()
    {
        active_camera = PlayerCamera;
    }

    private IEnumerator move_helper(Vector3 destination, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = CutsceneCamera.transform.position;
        while (elapsedTime < seconds)
        {
            CutsceneCamera.transform.position = Vector3.Lerp(startingPos, destination, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        CutsceneCamera.transform.position = destination;
    }

    public bool move(Vector2 destination, float seconds)
    {
        float start_z = CutsceneCamera.transform.position.z;
        Vector3 destination3D = new Vector3();
        destination3D.Set(destination.x, destination.y, start_z);
        StartCoroutine(move_helper(destination3D, seconds));
        return true;
    }
    public void reset_position()
    {

    }

    public int get_active_camera()
    {
        if (active_camera == PlayerCamera)
        {
            Debug.Log("Active Camera: 0");
            return 0;
        }
        else
        {
            Debug.Log("Active Camera: 1");
            return 1;
        }
    }
    public bool set_active_camera(int camera_number)
    {
        if (camera_number != 0 && camera_number != 1)
        {
            return false;
        }
        if (camera_number == 0)
        {
            CutsceneCamera.Priority = 0;
            PlayerCamera.Priority = 10;
            active_camera = PlayerCamera;
        }
        else
        {
            PlayerCamera.Priority = 0;
            CutsceneCamera.Priority = 10;
            active_camera = CutsceneCamera;
        }
        return true;
    }
    public void shake()
    {

    }
    private void Update()
    {
        if (Input.GetKeyDown("0"))
        {
            set_active_camera(0);
            Debug.Log("Active Camera: Player");
            get_active_camera();
        }
        else if (Input.GetKeyDown("1"))
        {
            set_active_camera(1);
            Debug.Log("Active Camera: Cutscene");
            get_active_camera();
        }
        else if (Input.GetKeyDown("m"))
        {
            Vector2 destination = new Vector2();
            destination.Set((float)-3.21, (float)(3.34));
            move(destination, 10);
        }
    }
}
