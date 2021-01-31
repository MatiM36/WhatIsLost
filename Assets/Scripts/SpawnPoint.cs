using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;

    public Canvas pauseCanvas;

    private GameObject _player;
    private CameraBehaviour _camera;

    // Start is called before the first frame update
    void Awake()
    {
        _camera = FindObjectOfType<CameraBehaviour>();
        InstantiatePlayer();
        pauseCanvas.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleCanvas();
    }

    public void ToggleCanvas()
    {
        pauseCanvas.enabled = !pauseCanvas.enabled;
    }

    public void RestartLevel()
    {
        SceneLoader.Instance.LoadSceneWithFade(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        SceneLoader.Instance.LoadSceneWithFade("Menu");
    }

    public void InstantiatePlayer()
    {
        if (_player != null)
            Destroy(_player);

        _player = Instantiate(_playerPrefab);
        _player.transform.position = transform.position;
        _player.transform.rotation = transform.rotation;

        SetCamera();
    }

    private void SetCamera()
    {
        if (_camera)
        {
            _camera.SetObject(_player);
        }
    }
}
