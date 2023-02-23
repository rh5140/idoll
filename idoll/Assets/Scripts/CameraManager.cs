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
        SetActiveCamera(cameras.PlayerCamera);
        impulse_source = transform.GetChild(0).GetComponent<CinemachineImpulseSource>();
    }

    private IEnumerator MoveHelper(Vector3 destination, float seconds)
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

    public bool Move(Vector2 destination, float seconds)
    {
        float start_z = CutsceneCamera.transform.position.z;
        Vector3 destination3D = new Vector3();
        destination3D.Set(destination.x, destination.y, start_z);
        StartCoroutine(MoveHelper(destination3D, seconds));
        return true;
    }
    public bool ResetPosition()
    {
        CutsceneCamera.transform.position = PlayerCamera.transform.position;
        return true;
    }

    public cameras GetActiveCamera()
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
    public bool SetActiveCamera(cameras camera)
    {
        if (camera == cameras.PlayerCamera)
        {
            try // Attempt to automatically link to the player when switching to the Player Camera
            {
                PlayerCamera.Follow = GameObject.FindGameObjectWithTag("Player").transform;
            }
            catch { Debug.Log("PlayerCamera could not find a gameObject with the Player Tag"); }

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
    public bool Shake()
    {
        impulse_source.GenerateImpulse(1f);
        return true;
    }
    private void Update()
    {
        if (Input.GetKeyDown("0"))
        {
            SetActiveCamera(cameras.PlayerCamera);
            // Debug.Log("Active Camera: Player");
            // GetActiveCamera();
        }
        else if (Input.GetKeyDown("1"))
        {
            SetActiveCamera(cameras.CutsceneCamera);
            // Debug.Log("Active Camera: Cutscene");
            // GetActiveCamera();
        }
        else if (Input.GetKeyDown("m"))
        {
            Vector2 destination = new Vector2();
            destination.Set((float)-3.21, (float)(3.34));
            Move(destination, 5);
        }
        else if (Input.GetKeyDown("r"))
        {
            ResetPosition();
        }
        else if (Input.GetKeyDown("i"))
        {
            Shake();
        }
    }
}
