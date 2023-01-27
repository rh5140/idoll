# CameraSystem

Prefab for camera implementation. Can be switched between a player-follow camera (```PlayerCamera```) or a cutscene camera (```CutsceneCamera```). 

On Scene initialization, the default camera state is ```PlayerCamera```.

```PlayerCamera``` must be initialized with a target GameObject to follow under the ```Follow``` SerializeField. 

→ To follow the player, ```PlayerSprite``` must be used, NOT the parent object ```Player``` itself.


## CameraManager.cs

| Methods             |
| ------------------- |
| get_active_camera() |
| set_active_camera() |
| move()              |
| reset_position()    |
| shake()             |

### Methods

```
int get_active_camera()
```
Returns the ID of the currently active camera.

Camera 0: ```Player```

Camera 1: ```Cutscene```

```
bool set_active_camera(int camera_number)
```

Sets the currently active camera.

Camera 0: ```Player```

Camera 1: ```Cutscene```

```
bool move(Vector2 destination, float seconds)
```

Moves the cutscene camera to the specified Vector2 position in the specified amount of time (seconds).


```
bool reset_position()
```

Moves the ```cutscene``` camera's position to the ```player``` camera position.



```
bool shake()
```
Generates camera shake. Will add more customizability.


