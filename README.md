# CameraSystem

Prefab for camera implementation. Can be switched between a player-follow camera (```PlayerCamera```) or a cutscene camera (```CutsceneCamera```). 

```PlayerCamera``` must be initialized with a target GameObject to follow under the ```Follow``` SerializeField.


## CameraManager.cs

| Attributes    | Methods             |
| ------------- | ------------------- |
| active_camera | move()              |
|               | reset_position()    |
|               | set_active_camera() |
|               | shake()             |

### Attributes

```
GameObject active_camera -> GameObject {Player, Cutscene}
```

The currently active camera.

### Methods

```
move(Vector2 position, float milliseconds) -> bool
```

Moves the cutscene camera to the specified Vector2 in the specified amount of time in milliseconds.

Note: **active_camera** must be set to **cutscene**.


```
reset_position() -> bool
```

Moves the ```cutscene``` camera to the ```player``` camera position.

```
set_active_camera(int camera_number) -> bool
```

Sets the currently active camera.

Camera 0: ```Player```

Camera 1: ```Cutscene```

```
shake(float intensity) -> bool
```
Generates camera shake with specified intensity (must be between 0-100).


