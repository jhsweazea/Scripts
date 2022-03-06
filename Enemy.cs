using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private Player _player;
    //handle to animator component
    private Animator _anim;
    [SerializeField]
    private AudioClip _enemyDeath;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        //null check for player
        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }
        //assign component to anim
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("The animator is NULL");
        }
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("The Audio Source on the enemy is NULL");
        }
        else
        {
            _audioSource.clip = _enemyDeath;
        }
    }

    void Update()
    {
        CalculateMovement();
        if(Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }
    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7f, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(GetComponent<Enemy>());
            Destroy(this.gameObject, 2.8f);           
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.addScore(Random.Range(8,13));
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(GetComponent<Enemy>());
            Destroy(this.gameObject, 2.8f);            
        }
    }

}
