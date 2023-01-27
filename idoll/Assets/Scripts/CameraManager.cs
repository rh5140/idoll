using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera PlayerCamera;
    [SerializeField] CinemachineVirtualCamera CutsceneCamera;
    private CinemachineImpulseSource impulse_source;

    private CinemachineVirtualCamera active_camera = null;

    public enum cameras
    {
        PlayerCamera,
        CutsceneCamera,
    }

    private void Start()
    {
        active_camera = PlayerCamera;
        impulse_source = transform.GetChild(0).GetComponent<CinemachineImpulseSource>();
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
    public bool reset_position()
    {
        CutsceneCamera.transform.position = PlayerCamera.transform.position;
        return true;
    }

    public cameras get_active_camera()
    {
        if (active_camera == PlayerCamera)
        {
            //Debug.Log("Active Camera: 0");
            return cameras.PlayerCamera;
        }
        else
        {
            //Debug.Log("Active Camera: 1");
            return cameras.CutsceneCamera;
        }
    }
    public bool set_active_camera(cameras camera)
    {
        if (camera == cameras.PlayerCamera)
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
    public bool shake()
    {
        impulse_source.GenerateImpulse(1f);
        return true;
    }
    private void Update()
    {
        if (Input.GetKeyDown("0"))
        {
            set_active_camera(cameras.PlayerCamera);
            // Debug.Log("Active Camera: Player");
            // get_active_camera();
        }
        else if (Input.GetKeyDown("1"))
        {
            set_active_camera(cameras.CutsceneCamera);
            // Debug.Log("Active Camera: Cutscene");
            // get_active_camera();
        }
        else if (Input.GetKeyDown("m"))
        {
            Vector2 destination = new Vector2();
            destination.Set((float)-3.21, (float)(3.34));
            move(destination, 5);
        }
        else if (Input.GetKeyDown("r"))
        {
            reset_position();
        }
        else if (Input.GetKeyDown("i"))
        {
            shake();
        }
    }
}
