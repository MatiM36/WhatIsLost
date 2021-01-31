using Mati36.Vinyl;
using UnityEngine;

public class EnvironmentMusic : MonoBehaviour
{
    public VinylAsset environmentMusic;

    // Start is called before the first frame update
    void Start()
    {
        environmentMusic.Play();
    }
}
