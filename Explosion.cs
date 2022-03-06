using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private AudioClip _explosion;
    private AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 3.0f);
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("The Audio Source on the explosion is NULL");
        }
        else
        {
            _audioSource.clip = _explosion;
        }
    }

}
