using UnityEngine;
using System.Collections;

public class NebulaLaser : MonoBehaviour {

	public bool laserActive = false;
	public bool autoFireForDemo = false;
	public bool useExtendedLength = false;
	public bool useLineOfSight = true;

	private float damageOverTimeTimer = 0;
	private float damageOverTimeFreq = 0.25f;
	private bool canDamageTarget = false;

	private AudioSource laserFireSound;
	private bool laserSoundPlayed = false;

	public ParticleSystem levelUpEffect;
	public Renderer[] levelIndicatorRenderers;

	public Transform turretColorRenderer;

	public Transform laserFireEmitPointTrans;
	public ParticleSystem laserFireCenterEmitter;
	public float firingParticleSize = 1;

	public Transform targetHitTransform;
	public ParticleSystem targetHitEmitter;
	public float hitParticleSize = 1;

	public LineRenderer innerLaserLineRenderer;
	public LineRenderer outerLaserLineRenderer;
	public Color laserBeamColor;
	public Material laserYBeamMaterial;

	private Vector3 laserStartPoint = Vector3.zero;
	private Vector3 laserEndPoint = Vector3.zero;

	public bool laserCanFire = false;
	public bool laserFiring = false;
	public float laserDamage = 10;
	public float laserDamagePerSecond = 2;

	public float innerLaserStartWidth = 1.0f;
	public float innerLaserEndWidth = 1.0f;
	public float outerLaserStartWidth = 1.0f;
	public float outerLaserEndWidth = 1.0f;

	public float laserGrowSpeed = 1;
	public float innerLaserTileAmount = 1.0f;
	private float _innerLaserStartWidth = 1.0f;
	private float _innerLaserEndWidth = 1.0f;
	public float outerLaserTileAmount = 1.0f;
	private float _outerLaserStartWidth = 1.0f;
	private float _outerLaserEndWidth = 1.0f;

	public float fireSpeed = 20.0f;

	public bool offsetMaterialTexture = true;
	public float scrollSpeed = 2.5f;

	private float laserFireTimer = 0;
	public float laserRechargeTime = 2.5f;

	public Transform currentTarget;


	public void UpdateLevelIndicators(int levelIn, Color levelColorIn ) {
		if(levelUpEffect) {
			levelUpEffect.startColor = levelColorIn;
			levelUpEffect.Play();
			if(levelIndicatorRenderers.Length > 0 ) {
				for( int i = 0; i < levelIndicatorRenderers.Length; i++ ) {
					if(i <= levelIn ) {
						if(!levelIndicatorRenderers[i].gameObject.activeSelf)
							levelIndicatorRenderers[i].gameObject.SetActive(true);
					} else {
						if(levelIndicatorRenderers[i].gameObject.activeSelf)
							levelIndicatorRenderers[i].gameObject.SetActive(false);
					}
					levelIndicatorRenderers[i].material.color = levelColorIn;
				}
			}
		}
	}
}




















































































