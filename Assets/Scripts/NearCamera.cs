using UnityEngine;

public class NearCamera : MonoBehaviour
{
    [SerializeField]
    private BaseCharacter _ship;

    [SerializeField]
    private AudioClip _backgroundMusic;

    private AudioSource _as;

    private void OnEnable()
    {
        _as = GetComponent<AudioSource>();
        //play Audio
        _as.PlayOneShot(_backgroundMusic);
        if (_ship != null)
        {
            transform.position = new Vector3(_ship.transform.position.x, _ship.transform.position.y, transform.position.z);
        }
    }

    private void Update()
    {
        if (_ship != null)
        {
            transform.position = new Vector3(_ship.transform.position.x, _ship.transform.position.y, transform.position.z);
        }
    }
}
