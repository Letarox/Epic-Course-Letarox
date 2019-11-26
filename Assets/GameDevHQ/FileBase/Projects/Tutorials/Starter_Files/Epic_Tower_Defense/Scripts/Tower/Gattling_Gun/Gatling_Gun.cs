﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameDevHQ.FileBase.Gatling_Gun
{
    /// <summary>
    /// This script will allow you to view the presentation of the Turret and use it within your project.
    /// Please feel free to extend this script however you'd like. To access this script from another script
    /// (Script Communication using GetComponent) -- You must include the namespace (using statements) at the top. 
    /// "using GameDevHQ.FileBase.Gatling_Gun" without the quotes. 
    /// 
    /// For more, visit GameDevHQ.com
    /// 
    /// @authors
    /// Al Heck
    /// Jonathan Weinberger
    /// </summary>

    [RequireComponent(typeof(AudioSource))] //Require Audio Source component
    public class Gatling_Gun : MonoBehaviour, ITower
    {
        private Transform _gunBarrel; //Reference to hold the gun barrel
        public GameObject Muzzle_Flash; //reference to the muzzle flash effect to play when firing
        public ParticleSystem bulletCasings; //reference to the bullet casing effect to play when firing
        public AudioClip fireSound; //Reference to the audio clip

        private AudioSource _audioSource; //reference to the audio source component
        private bool _startWeaponNoise = true;

        [SerializeField]
        private List<GameObject> _attackQueue = new List<GameObject>();

        private GameObject _target = null;
        private IDamageble _targetDamagable;

        private float _canFire = -1f;

        public int Damage { get; set; }
        public int WarfundCost { get; set; }
        public float FireRate { get; set; }

        [SerializeField]
        private TowerType _towerType;

        enum TowerType
        {
            Gattling_Gun,
            Missile_Turret,
            Dual_Gattling_Gun,
            Dual_Missile_Turret
        }
        public void Shoot(GameObject target)
        {
            Vector3 direction = target.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            //transform.LookAt(target.transform);
            if(_canFire <= Time.time)
            {
                _canFire = Time.time + FireRate;
                _targetDamagable.Damage(this.gameObject, Damage);
            }
        }

        public int GetTowerType()
        {
            return (int)_towerType;
        }

        void Awake()
        {
            switch (_towerType)
            {
                case TowerType.Gattling_Gun:
                    Damage = 10;
                    WarfundCost = 100;
                    FireRate = 0.25f;
                    break;
                case TowerType.Missile_Turret:
                    Damage = 20;
                    WarfundCost = 150;
                    FireRate = 2f;
                    break;
                default:
                    break;
            }
        }

        void Start()
        {            
            _gunBarrel = GameObject.Find("Barrel_to_Spin").GetComponent<Transform>(); //assigning the transform of the gun barrel to the variable
            Muzzle_Flash.SetActive(false); //setting the initial state of the muzzle flash effect to off
            _audioSource = GetComponent<AudioSource>(); //ssign the Audio Source to the reference variable
            _audioSource.playOnAwake = false; //disabling play on awake
            _audioSource.loop = true; //making sure our sound effect loops
            _audioSource.clip = fireSound; //assign the clip to play
        }

        void Update()
        {

        }

        void Attack()
        {
            RotateBarrel(); //Call the rotation function responsible for rotating our gun barrel
            Muzzle_Flash.SetActive(true); //enable muzzle effect particle effect
            bulletCasings.Emit(1); //Emit the bullet casing particle effect  

            if (_startWeaponNoise == true) //checking if we need to start the gun sound
            {
                _audioSource.Play(); //play audio clip attached to audio source
                _startWeaponNoise = false; //set the start weapon noise value to false to prevent calling it again
            }
        }

        void StopAttacking()
        {
            Muzzle_Flash.SetActive(false); //turn off muzzle flash particle effect
            _audioSource.Stop(); //stop the sound effect from playing
            _startWeaponNoise = true; //set the start weapon noise value to true
        }

        // Method to rotate gun barrel 
        void RotateBarrel() 
        {
            _gunBarrel.transform.Rotate(Vector3.forward * Time.deltaTime * -500.0f); //rotate the gun barrel along the "forward" (z) axis at 500 meters per second
        }

        GameObject SetEnemyTarget()
        {
            foreach (var enemy in _attackQueue)
            {
                if (enemy.activeInHierarchy == true)
                {
                    _targetDamagable = enemy.GetComponent<IDamageble>();
                    if (_targetDamagable == null)
                        Debug.LogError("IDamagable is NULL on " + enemy.transform.name);
                    return enemy;
                }
            }
            return null;
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Enemy")
            {
                _attackQueue.Add(other.gameObject);
                if (_target == null)
                    _target = SetEnemyTarget();
            }
        }

        void OnTriggerStay(Collider other)
        {
            if(_target != null && _target.activeInHierarchy == true)
            {
                Attack();
                Shoot(_target);
            }
            else
            {
                _target = SetEnemyTarget();
                if(_target == null)
                    StopAttacking();
            }
        }

        void OnTriggerExit(Collider other)
        {
            if(other.tag == "Enemy")
            {
                _attackQueue.Remove(other.gameObject);
                if (other.gameObject.Equals(_target))
                {
                    _target = SetEnemyTarget();
                }
            }
        }
        public void CleanTarget()
        {
            _attackQueue.Remove(_target);
            _target = null;
        }
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
    }
}