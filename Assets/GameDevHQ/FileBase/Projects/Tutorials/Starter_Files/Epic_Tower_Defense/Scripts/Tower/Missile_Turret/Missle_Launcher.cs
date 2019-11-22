﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevHQ.FileBase.Missle_Launcher.Missle;

namespace GameDevHQ.FileBase.Missle_Launcher
{
    public class Missle_Launcher : MonoBehaviour, ITower
    {
        [SerializeField]
        private GameObject _missilePrefab; //holds the missle gameobject to clone
        [SerializeField]
        private GameObject[] _misslePositions; //array to hold the rocket positions on the turret
        [SerializeField]
        private float _fireDelay; //fire delay between rockets
        [SerializeField]
        private float _launchSpeed; //initial launch speed of the rocket
        [SerializeField]
        private float _power; //power to apply to the force of the rocket
        [SerializeField]
        private float _fuseDelay; //fuse delay before the rocket launches
        [SerializeField]
        private float _reloadTime; //time in between reloading the rockets
        [SerializeField]
        private float _destroyTime = 10.0f; //how long till the rockets get cleaned up
        private bool _launched; //bool to check if we launched the rockets

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

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        public void Shoot(GameObject target)
        {
            
            //transform.LookAt(target.transform);
            if (_canFire <= Time.time)
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

        private void Update()
        {
            /* if (Input.GetKeyDown(KeyCode.Space) && _launched == false) //check for space key and if we launched the rockets
            {
                _launched = true; //set the launch bool to true
                StartCoroutine(FireRocketsRoutine()); //start a coroutine that fires the rockets. 
            } */
        }

        IEnumerator FireRocketsRoutine()
        {
            for (int i = 0; i < _misslePositions.Length; i++) //for loop to iterate through each missle position
            {
                GameObject rocket = Instantiate(_missilePrefab) as GameObject; //instantiate a rocket

                rocket.transform.parent = _misslePositions[i].transform; //set the rockets parent to the missle launch position 
                rocket.transform.localPosition = Vector3.zero; //set the rocket position values to zero
                rocket.transform.localEulerAngles = new Vector3(-90, 0, 0); //set the rotation values to be properly aligned with the rockets forward direction
                rocket.transform.parent = null; //set the rocket parent to null

                rocket.GetComponent<GameDevHQ.FileBase.Missle_Launcher.Missle.Missle>().AssignMissleRules(_launchSpeed, _power, _fuseDelay, _destroyTime); //assign missle properties 

                _misslePositions[i].SetActive(false); //turn off the rocket sitting in the turret to make it look like it fired

                yield return new WaitForSeconds(_fireDelay); //wait for the firedelay
            }

            for (int i = 0; i < _misslePositions.Length; i++) //itterate through missle positions
            {
                yield return new WaitForSeconds(_reloadTime); //wait for reload time
                _misslePositions[i].SetActive(true); //enable fake rocket to show ready to fire
            }

            _launched = false; //set launch bool to false
        }

        void Attack()
        {
            Vector3 direction = _target.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            _launched = true;
            StartCoroutine(FireRocketsRoutine());
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
            if (other.tag == "Enemy")
            {
                _attackQueue.Add(other.gameObject);
                if (_target == null)
                    _target = SetEnemyTarget();
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (_target != null && _target.activeInHierarchy == true && _launched == false)
            {
                Attack();
                Shoot(_target);
            }
            else
            {
                _target = SetEnemyTarget();
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.tag == "Enemy")
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
    }
}

