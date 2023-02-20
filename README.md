# CameraSystem

Prefab for camera implementation. Can be switched between a player-follow camera (```PlayerCamera```) or a cutscene camera (```CutsceneCamera```). 

On Scene initialization, the default camera is ```PlayerCamera```.

```PlayerCamera``` must be initialized with a target GameObject to follow under the ```Follow``` SerializeField. 

→ To follow the player, ```PlayerSprite``` must be used, NOT the parent object ```Player``` itself.


## CameraManager.cs

|Attributes | Methods             |
|---| ------------------- |
|cameras| get_active_camera() |
|| set_active_camera() |
|| move()              |
|| reset_position()    |
|| shake()             |

### Attributes
```
enum cameras {
    PlayerCamera,
    CutsceneCamera,
}
```
enum representing the camera modes. To be used as arguments for set_active_camera().

### Methods

```
cameras get_active_camera()
```
Returns the ID of the currently active camera.

```
bool set_active_camera(cameras target_camera)
```

Sets the currently active camera to target_camera.

```
bool move(Vector2 destination, float seconds)
```

Moves ```CutsceneCamera``` to the specified Vector2 position in the specified amount of time (seconds).


```
bool reset_position()
```

Moves ```CutsceneCamera```  to the ```PlayerCamera``` position.



```
bool shake()
```
Generates camera shake. 

Will add more customizability.


