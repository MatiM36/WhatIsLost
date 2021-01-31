using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;

    private GameObject _player;
    private CameraBehaviour _camera;

    // Start is called before the first frame update
    void Awake()
    {
        _camera = FindObjectOfType<CameraBehaviour>();
        InstantiatePlayer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            SceneLoader.Instance.LoadSceneWithFade(SceneManager.GetActiveScene().name);

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
