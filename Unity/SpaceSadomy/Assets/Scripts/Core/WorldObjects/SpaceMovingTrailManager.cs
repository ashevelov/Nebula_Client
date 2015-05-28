using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpaceMovingTrailManager : MonoBehaviour {

    public GameObject trailPrefab;
    public float trailLifeTime = 3.0f;
    public float trailEmitInterval = 0.5f;
    public float trailGenerationCameraSpaceRadius = 2.0f;
    public float trailGenerationCameraSpaceZ = 10.0f;
    public float trailZSpeed = 5.0f;

    private List<TrailInfo> trails;
    private List<TrailInfo> trailsToDelete;
    private Transform selfTrainsfom;

    public class TrailInfo {
        public float Time { get; set;  }
        public Transform Trail { get; set; }
    }


    public void Setup(GameObject inTrailPrefab, float inTrailLifetime, float inTrailEmitInterval,
        float inTrailGenerationCameraSpaceRadius, float inTrailGenerationCameraSpaceZ, float inTrailSpeed)
    {
        trailPrefab = inTrailPrefab;
        trailLifeTime = inTrailLifetime;
        trailEmitInterval = inTrailEmitInterval;
        trailGenerationCameraSpaceRadius = inTrailGenerationCameraSpaceRadius;
        trailGenerationCameraSpaceZ = inTrailGenerationCameraSpaceZ;
        trailZSpeed = inTrailSpeed;
        trails = new List<TrailInfo>();
        trailsToDelete = new List<TrailInfo>();
        selfTrainsfom = transform;
        StartCoroutine(CorGenerateTrail());
    }

    IEnumerator CorGenerateTrail()
    {
        while (true)
        {
            yield return new WaitForSeconds(trailEmitInterval);

            Vector3 localPosition = GetTrailGenerationPosition();
            GameObject obj = Instantiate(trailPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            obj.transform.parent = selfTrainsfom;
            obj.transform.localPosition = localPosition;
            trails.Add(new TrailInfo { Time = 0.0f, Trail = obj.transform });
        }
    }

    private Vector3 GetTrailGenerationPosition()
    {
        Vector2 randomCircleVector = Random.insideUnitCircle * trailGenerationCameraSpaceRadius;
        Vector3 localVector =  new Vector3( randomCircleVector.x, randomCircleVector.y, trailGenerationCameraSpaceZ );
        return localVector;
    }

    void Update()
    {
        foreach (TrailInfo trail in trails)
        {
            trail.Time += Time.deltaTime;
            trail.Trail.localPosition = new Vector3(trail.Trail.localPosition.x, trail.Trail.localPosition.y, trail.Trail.localPosition.z - trailZSpeed * Time.deltaTime);
            if (trail.Time >= trailLifeTime)
            {
                trailsToDelete.Add(trail);
            }
        }

        if (trailsToDelete.Count > 0)
        {
            foreach (TrailInfo trail in trailsToDelete)
            {
                trails.Remove(trail);
                Destroy(trail.Trail.gameObject);
            }
            trailsToDelete.Clear();
        }
    }
}
