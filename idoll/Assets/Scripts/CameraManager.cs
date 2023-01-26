using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera PlayerCamera;
    [SerializeField] CinemachineVirtualCamera CutsceneCamera;

    public CinemachineVirtualCamera active_camera = null;

    private void Start()
    {
        active_camera = PlayerCamera;
    }

    public void move()
    {

    }
    public void reset_position()
    {

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
        }
        else {
            PlayerCamera.Priority = 0;
            CutsceneCamera.Priority = 10;
        }
        return true;
    }
    public void shake()
    {

    }
    private void Update() {
        if (Input.GetKeyDown("0")) {
            set_active_camera(0);
            Debug.Log("Active Camera: Player");
        }
        else if (Input.GetKeyDown("1")) {
            set_active_camera(1);
            Debug.Log("Active Camera: Cutscene");
        }
    }
}
