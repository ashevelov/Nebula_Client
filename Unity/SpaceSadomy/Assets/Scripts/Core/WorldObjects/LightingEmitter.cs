using UnityEngine;
using System.Collections;

public class LightingEmitter : MonoBehaviour {

    public Transform target;
    public int zigs = 100;
    public float speed = 1.0f;
    public float scale = 1.0f;

    Perlin noise;
    float oneOverZigs;

    private Particle[] particles;

    void Start() {

        oneOverZigs = 1.0f / (float)zigs;
        GetComponent<ParticleEmitter>().emit = false;
        GetComponent<ParticleEmitter>().Emit(zigs);
        particles = GetComponent<ParticleEmitter>().particles;
    }

    void Update() {
        if (noise == null)
            noise = new Perlin();
        float timex = Time.time * speed * 0.1365143f;
        float timey = Time.time * speed * 1.21688f;
        float timez = Time.time * speed * 2.5564f;

        for (int i = 0; i < particles.Length; i++) {
            Vector3 position = Vector3.Lerp(transform.position, target.position, oneOverZigs * (float)i);
            Vector3 offset = new Vector3(noise.Noise(timex + position.x, timex + position.y, timex + position.z),
                                noise.Noise(timey + position.x, timey + position.y, timey + position.z),
                                noise.Noise(timez + position.x, timez + position.y, timez + position.z));

            position += (offset * scale * ((float)i * oneOverZigs));
            particles[i].position = position;
            particles[i].color = Color.white;
            particles[i].energy = 1f;
        }

        GetComponent<ParticleEmitter>().particles = particles;

    }
}
