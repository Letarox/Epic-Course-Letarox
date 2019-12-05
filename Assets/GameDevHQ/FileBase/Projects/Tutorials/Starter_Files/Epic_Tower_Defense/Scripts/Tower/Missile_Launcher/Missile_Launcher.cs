using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevHQ.FileBase.Missile_Launcher.Missile;

/*
 *@author GameDevHQ 
 * For support, visit gamedevhq.com
 */

namespace GameDevHQ.FileBase.Missile_Launcher
{
    public class Missile_Launcher : MonoBehaviour, ITower
    {
        public enum MissileType
        {
            Normal,
            Homing
        }

        [SerializeField]
        private GameObject _missilePrefab; //holds the missle gameobject to clone
        [SerializeField]
        private MissileType _missileType; //type of missle to be launched
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
        private Transform _target; //Who should the rocket fire at?

        [SerializeField]
        private GameObject[] _misslePositionsLeft; //array to hold the rocket positions on the turret
        [SerializeField]
        private GameObject[] _misslePositionsRight; //array to hold the rocket positions on the turret

        private GameObject _rocket, _rocketLeft, _rocketRight;

        [SerializeField]
        private List<GameObject> _attackQueue = new List<GameObject>();

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
            IDamageble targetScript = target.GetComponent<IDamageble>();
            if(targetScript != null)
            {
                targetScript.Damage(this.gameObject,Damage);
            }
        }

        public int GetTowerType()
        {
            return (int)_towerType;
        }

        void Awake()
        {
            GameManager.Instance.SetTowerStats(this.gameObject);
        }

        IEnumerator FireRocketsRoutine()
        {
            if(_towerType == TowerType.Missile_Turret)
            {
                //GameObject rocket = Instantiate(_missilePrefab) as GameObject; //instantiate a rocket
                _rocket = SpawnManager.Instance.RequestMissile(this.gameObject);

                _rocket.transform.parent = _misslePositions[0].transform; //set the rockets parent to the missle launch position 
                _rocket.transform.localPosition = Vector3.zero; //set the rocket position values to zero
                _rocket.transform.localEulerAngles = new Vector3(-90, 0, 0); //set the rotation values to be properly aligned with the rockets forward direction
                _rocket.transform.parent = null; //set the rocket parent to null

                _rocket.GetComponent<GameDevHQ.FileBase.Missile_Launcher.Missile.Missile>().AssignMissleRules(_missileType, _target, _launchSpeed, _power, _fuseDelay, _destroyTime, this.gameObject); //assign missle properties 

                _misslePositions[0].SetActive(false); //turn off the rocket sitting in the turret to make it look like it fired

                yield return new WaitForSeconds(_fireDelay); //wait for the firedelay

                yield return new WaitForSeconds(_reloadTime); //wait for reload time
                _misslePositions[0].SetActive(true); //enable fake rocket to show ready to fire

                _launched = false; //set launch bool to false
            }
            else if(_towerType == TowerType.Dual_Missile_Turret)
            {
                _rocketLeft = SpawnManager.Instance.RequestMissile(this.gameObject); //instantiate a rocket
                _rocketRight = SpawnManager.Instance.RequestMissile(this.gameObject); //instantiate a rocket

                _rocketLeft.transform.parent = _misslePositionsLeft[0].transform; //set the rockets parent to the missle launch position 
                _rocketRight.transform.parent = _misslePositionsRight[0].transform; //set the rockets parent to the missle launch position 

                _rocketLeft.transform.localPosition = Vector3.zero; //set the rocket position values to zero
                _rocketRight.transform.localPosition = Vector3.zero; //set the rocket position values to zero

                _rocketLeft.transform.localEulerAngles = new Vector3(0, 0, 0); //set the rotation values to be properly aligned with the rockets forward direction
                _rocketRight.transform.localEulerAngles = new Vector3(0, 0, 0); //set the rotation values to be properly aligned with the rockets forward direction

                _rocketLeft.transform.parent = null; //set the rocket parent to null
                _rocketRight.transform.parent = null; //set the rocket parent to null

                _rocketLeft.GetComponent<GameDevHQ.FileBase.Missile_Launcher.Missile.Missile>().AssignMissleRules(_missileType, _target, _launchSpeed, _power, _fuseDelay, _destroyTime, this.gameObject); //assign missle properties 
                _rocketRight.GetComponent<GameDevHQ.FileBase.Missile_Launcher.Missile.Missile>().AssignMissleRules(_missileType, _target, _launchSpeed, _power, _fuseDelay, _destroyTime, this.gameObject); //assign missle properties 

                _misslePositionsLeft[0].SetActive(false); //turn off the rocket sitting in the turret to make it look like it fired
                _misslePositionsRight[0].SetActive(false); //turn off the rocket sitting in the turret to make it look like it fired

                yield return new WaitForSeconds(_fireDelay); //wait for the firedelay

                yield return new WaitForSeconds(_reloadTime); //wait for reload time
                _misslePositionsLeft[0].SetActive(true); //enable fake rocket to show ready to fire
                _misslePositionsRight[0].SetActive(true); //enable fake rocket to show ready to fire

                _launched = false; //set launch bool to false
            }
        }
        Transform SetEnemyTarget()
        {
            foreach (var enemy in _attackQueue)
            {
                if (enemy.activeInHierarchy == true)
                {
                    return enemy.transform;
                }
            }
            return null;
        }

        void Attack()
        {
            if(_canFire <= Time.time && _launched == false)
            {
                _canFire = Time.time + FireRate;
                _launched = true; //set the launch bool to true
                StartCoroutine(FireRocketsRoutine()); //start a coroutine that fires the rockets
            }            
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
            if (_target != null && _target.gameObject.activeInHierarchy == true)
            {
                Attack();
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
                if (other.gameObject.Equals(_target.gameObject))
                {
                    _target = SetEnemyTarget();
                }
            }
        }
        public void CleanTarget()
        {
            if(_target != null)
                _attackQueue.Remove(_target.gameObject);
            _target = null;
            if(_towerType == TowerType.Missile_Turret)
            {
                _rocket.GetComponent<GameDevHQ.FileBase.Missile_Launcher.Missile.Missile>().Hide();
            }
            else if (_towerType == TowerType.Dual_Missile_Turret)
            {
                _rocketLeft.GetComponent<GameDevHQ.FileBase.Missile_Launcher.Missile.Missile>().Hide();
                _rocketRight.GetComponent<GameDevHQ.FileBase.Missile_Launcher.Missile.Missile>().Hide();
            }
            
        }
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        /* IEnumerator FireRocketsRoutine()
        {
            for (int i = 0; i < _misslePositions.Length; i++) //for loop to iterate through each missle position
            {
                GameObject rocket = Instantiate(_missilePrefab) as GameObject; //instantiate a rocket

                rocket.transform.parent = _misslePositions[i].transform; //set the rockets parent to the missle launch position 
                rocket.transform.localPosition = Vector3.zero; //set the rocket position values to zero
                rocket.transform.localEulerAngles = new Vector3(-90, 0, 0); //set the rotation values to be properly aligned with the rockets forward direction
                rocket.transform.parent = null; //set the rocket parent to null

                rocket.GetComponent<GameDevHQ.FileBase.Missile_Launcher.Missile.Missile>().AssignMissleRules(_missileType, _target, _launchSpeed, _power, _fuseDelay, _destroyTime); //assign missle properties 

                _misslePositions[i].SetActive(false); //turn off the rocket sitting in the turret to make it look like it fired

                yield return new WaitForSeconds(_fireDelay); //wait for the firedelay
            }

            for (int i = 0; i < _misslePositions.Length; i++) //itterate through missle positions
            {
                yield return new WaitForSeconds(_reloadTime); //wait for reload time
                _misslePositions[i].SetActive(true); //enable fake rocket to show ready to fire
            }

            _launched = false; //set launch bool to false
        } */
    }
}

