using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _rightEngine, _leftEngine, _thruster;

    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;

    private Animator _anim;
    //[SerializeField]
    //private AudioClip _death;
    //private AudioSource _deathAudioSource;

    [SerializeField]
    private GameObject _explosionPrefab;

    [SerializeField]
    private int _score;

    private UIManager _uimanager;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }
        _uimanager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uimanager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("The Audio Source on the player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
        //_anim = GetComponent<Animator>();
        //if (_anim == null)
        //{
        //    Debug.LogError("The animator is NULL");
        //}
        //_deathAudioSource = GetComponent<AudioSource>();
        //if (_deathAudioSource == null)
        //{
        //    Debug.LogError("The Audio Source on the player is NULL");
        //}
        //else
        //{
        //    _audioSource.clip = _death;
        //}
    }
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
        
    }
    void FireLaser() 
    { 
        _canFire = Time.time + _fireRate;
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(.46f, 0, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        _audioSource.Play();
    }
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
        
    }
    public void SpeedActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;        
        StartCoroutine(SpeedPowerDownRoutine());

    }
    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }
    void CalculateMovement() 
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        
        transform.Translate(direction * _speed * Time.deltaTime);
     

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x >= 9)
        {
            transform.position = new Vector3(9, transform.position.y, 0);
        }
        else if (transform.position.x <= -9)
        {
            transform.position = new Vector3(-9, transform.position.y, 0);
        }
    }
    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        
        _lives--;

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        _uimanager.UpdateLives(_lives);

        if (_lives < 1)
        {
            //_anim.SetTrigger("OnPlayerDeath");
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _rightEngine.SetActive(false);
            _leftEngine.SetActive(false);
            _thruster.SetActive(false);
            
            //_deathAudioSource.Play();
            
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject, .1f);
        }
    }
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;

    }
    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(10.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }
    public void addScore(int points)
    {
        _score += points;
        _uimanager.UpdateScore(_score);
    }
}
