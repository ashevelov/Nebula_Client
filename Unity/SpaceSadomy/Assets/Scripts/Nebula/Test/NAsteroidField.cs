// NAsteroidField.cs
// Nebula
// 
// Crate by Oleg Zhelestcov on Monday, October 27, 2014 3:41:18 PM
// Copyright (c) 2014 KomarGames. All rights reserved.
//
//Simplified version of AsteroidField, which not used random materials for random meshes, use only prefabs and probabilities
namespace Nebula.Test {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    public class NAsteroidField : MonoBehaviour {

        public NAsteroidWeightData[] asteroidWeights;

        public float range = 20000.0f;
        public int maxAsteroids;
        public bool respawnDestroyedAsteroids = true;
        public bool resppawnIfOutOfRange = true;
        public float distanceSpawn = 0.95f;
        public float minAsteroidScale = 0.1f;
        public float maxAsteroidScale = 0.1f;
        public float scaleMultiplier = 1.0f;

        public bool isRigidBody = false;

        public float minAsteroidRotationSpeed = 0.0f;
        public float maxAsteroidRotationSpeed = 1.0f;
        public float rotationSpeedMultiplier = 1.0f;
        public float minAsteroidDriftSpeed = 0.0f;
        public float maxAsteroidDriftSpeed = 1.0f;
        public float driftSpeedMultiplier = 1.0f;


        private float _distanceToSpawn;
        private Transform _cacheTransform;
        private List<Transform> _asteroids = new List<Transform>();


        private bool materialsSetted = false;


        void OnEnable() {
            _cacheTransform = transform;
            _distanceToSpawn = range * distanceSpawn;


            for (int i = 0; i < _asteroids.Count; i++) {
#if UNITY_3_5
				_asteroids[i].gameObject.active = true;		
#endif
#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8
            _asteroids[i].gameObject.SetActive(true);
#endif
            }

            SpawnAsteroids(false);
        }


        void OnDisable() {
            for (int i = 0; i < _asteroids.Count; i++) {
                if (_asteroids[i] != null) {
#if UNITY_3_5
				_asteroids[i].gameObject.active = false;		
#endif
#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8
                _asteroids[i].gameObject.SetActive(false);
#endif

                    Destroy(_asteroids[i].gameObject);
                    _asteroids[i] = null;
                }
            }
        }

        void OnDrawGizmosSelected() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, range);
        }

        void Update() {
            for (int i = 0; i < _asteroids.Count; i++) {
                Transform _asteroid = _asteroids[i];

                if (_asteroid != null) {
                    float _distance = Vector3.Distance(_asteroid.position, _cacheTransform.position);

                    if (_distance > range && resppawnIfOutOfRange) {
                        _asteroid.position = Random.onUnitSphere * _distanceToSpawn + _cacheTransform.position;
                        float _newScale = Random.Range(minAsteroidScale, maxAsteroidScale) * scaleMultiplier;

                        _asteroid.localScale = new Vector3(_newScale, _newScale, _newScale);

                        Vector3 _newRotation = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
                        _asteroid.eulerAngles = _newRotation;
                    }

                } else {
                    _asteroids.RemoveAt(i);
                }

                if (respawnDestroyedAsteroids && _asteroids.Count < maxAsteroids) {
                    SpawnAsteroids(true);
                }
            }
        }

        void SpawnAsteroids(bool atSpawnDistance) {
            while (_asteroids.Count < maxAsteroids) {
                NAsteroidData data = this.GetWeightedData();
                Transform _newAsteroidPrefab = data.prefab;//prefabsAsteroids[Random.Range(0, prefabsAsteroids.Length)];

                Vector3 _newPosition = Vector3.zero;
                if (atSpawnDistance) {
                    _newPosition = _cacheTransform.position + Random.onUnitSphere * _distanceToSpawn;
                } else {
                    _newPosition = _cacheTransform.position + Random.insideUnitSphere * _distanceToSpawn;
                }

                Transform _newAsteroid = (Transform)Instantiate(_newAsteroidPrefab, _newPosition, _cacheTransform.rotation);


                _asteroids.Add(_newAsteroid);


                float _newScale = Random.Range(minAsteroidScale, maxAsteroidScale) * scaleMultiplier;

                if (data.useRelativeScale == false) {
                    _newAsteroid.localScale = new Vector3(_newScale, _newScale, _newScale);
                } else {
                    _newAsteroid.localScale = data.prefab.localScale * _newScale;
                }

                if (_newAsteroid.GetComponent<Rigidbody>()) {
                    Destroy(_newAsteroid.GetComponent<Rigidbody>());
                }

            }
        }

        void CreateTransparentMaterials(Hashtable ht) {
            foreach (var w in this.asteroidWeights) {
                foreach (var d in w.asteroidDatas) {
                    ht.Add(d.prefab.GetComponent<Renderer>().material, new Material("SpaceUnity/Asteroid Transparent"));
                    ((Material)ht[d.prefab.GetComponent<Renderer>().material]).SetTexture("_MainTex", d.prefab.GetComponent<Renderer>().material.GetTexture("_MainTex"));
                    ((Material)ht[d.prefab.GetComponent<Renderer>().material]).color = d.prefab.GetComponent<Renderer>().material.color;
                }
            }
        }

        private NAsteroidData GetWeightedData() {
            float totalWeight = 0;
            foreach (var d in this.asteroidWeights) {
                totalWeight += d.weight;
            }

            float[] weights = new float[this.asteroidWeights.Length];
            float currentWeight = 0;
            for (int i = 0; i < weights.Length; i++) {
                currentWeight += this.asteroidWeights[i].weight;
                weights[i] = currentWeight / totalWeight;
            }
            weights[weights.Length - 1] = 1.0f;

            float rnd = Random.value;
            for (int i = 0; i < weights.Length; i++) {
                if (i < weights.Length - 1) {
                    if (rnd >= weights[i] && rnd < weights[i + 1])
                        return this.asteroidWeights[i].asteroidDatas[Random.Range(0, this.asteroidWeights[i].asteroidDatas.Length - 1)];
                } else {
                    return this.asteroidWeights[weights.Length - 1].asteroidDatas[Random.Range(0, this.asteroidWeights[weights.Length - 1].asteroidDatas.Length - 1)];
                }
            }

            return this.asteroidWeights[weights.Length - 1].asteroidDatas[Random.Range(0, this.asteroidWeights[weights.Length - 1].asteroidDatas.Length - 1)];
        }

        private NAsteroidData GetDataForPrefab(string prefabName) {
            foreach (var wd in this.asteroidWeights) {
                foreach (var a in wd.asteroidDatas) {
                    if (a.prefab.name == prefabName)
                        return a;
                }
            }

            return asteroidWeights[0].asteroidDatas[0];
        }
    }

    [System.Serializable]
    public struct NAsteroidWeightData {
        public float weight;
        public NAsteroidData[] asteroidDatas;
    }

    [System.Serializable]
    public struct NAsteroidData {
        public bool useRelativeScale;
        public Transform prefab;
    }
}