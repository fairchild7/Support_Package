﻿//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;
using System.Collections;
using InfimaGames.LowPolyShooterPack;
using Random = UnityEngine.Random;

namespace InfimaGames.LowPolyShooterPack.Legacy
{
	public class Projectile : MonoBehaviour
	{

		[Range(5, 100)]
		[Tooltip("After how long time should the bullet prefab be destroyed?")]
		public float destroyAfter;

		[Tooltip("If enabled the bullet destroys on impact")]
		public bool destroyOnImpact = false;

		[Tooltip("Minimum time after impact that the bullet is destroyed")]
		public float minDestroyTime;

		[Tooltip("Maximum time after impact that the bullet is destroyed")]
		public float maxDestroyTime;

        [Tooltip("Damage deal to damageable")]
        public float damage = 60F;

        [Header("Impact Effect Prefabs")]
		public Transform[] bloodImpactPrefabs;

		public Transform[] metalImpactPrefabs;
		public Transform[] dirtImpactPrefabs;
		public Transform[] concreteImpactPrefabs;

		[SerializeField] bool isCollideVFX = true;

        [Header("Additional Custom")]
        [SerializeField] ParticleSystem impactVFX;

		private void Start()
		{
			//Grab the game mode service, we need it to access the player character!
			var gameModeService = ServiceLocator.Current.Get<IGameModeService>();
			//Ignore the main player character's collision. A little hacky, but it should work.
			if (gameModeService.GetPlayerCharacter() != null)
				Physics.IgnoreCollision(gameModeService.GetPlayerCharacter().GetComponent<Collider>(),
					GetComponent<Collider>());

			//Start destroy timer
			StartCoroutine(DestroyAfter());
		}

		//If the bullet collides with anything
		private void OnCollisionEnter(Collision collision)
		{
			//Ignore collisions with other projectiles.
			if (collision.gameObject.GetComponent<Projectile>() != null)
				return;

			// //Ignore collision if bullet collides with "Player" tag
			// if (collision.gameObject.CompareTag("Player")) 
			// {
			// 	//Physics.IgnoreCollision (collision.collider);
			// 	Debug.LogWarning("Collides with player");
			// 	//Physics.IgnoreCollision(GetComponent<Collider>(), GetComponent<Collider>());
			//
			// 	//Ignore player character collision, otherwise this moves it, which is quite odd, and other weird stuff happens!
			// 	Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
			//
			// 	//Return, otherwise we will destroy with this hit, which we don't want!
			// 	return;
			// }
			//
			//If destroy on impact is false, start 
			//coroutine with random destroy timer
			if (!destroyOnImpact)
			{
				StartCoroutine(DestroyTimer());
			}
			//Otherwise, destroy bullet on impact
			else
			{
				Destroy(gameObject);
			}

			if (isCollideVFX)
			{
				//If bullet collides with "Blood" tag
				if (collision.transform.tag == "Blood")
				{
					//Instantiate random impact prefab from array
					Instantiate(bloodImpactPrefabs[Random.Range
							(0, bloodImpactPrefabs.Length)], transform.position,
						Quaternion.LookRotation(collision.contacts[0].normal));
					//Destroy bullet object
					Destroy(gameObject);
				}

				//If bullet collides with "Metal" tag
				if (collision.transform.tag == "Metal")
				{
					//Instantiate random impact prefab from array
					Instantiate(metalImpactPrefabs[Random.Range
							(0, bloodImpactPrefabs.Length)], transform.position,
						Quaternion.LookRotation(collision.contacts[0].normal));
					Debug.Log("Metal hit");
					//Destroy bullet object
					Destroy(gameObject);
				}

				//If bullet collides with "Dirt" tag
				if (collision.transform.tag == "Dirt")
				{
					//Instantiate random impact prefab from array
					Instantiate(dirtImpactPrefabs[Random.Range
							(0, bloodImpactPrefabs.Length)], transform.position,
						Quaternion.LookRotation(collision.contacts[0].normal));
					//Destroy bullet object
					Destroy(gameObject);
				}

				//If bullet collides with "Concrete" tag
				if (collision.transform.tag == "Concrete")
				{
					//Instantiate random impact prefab from array
					Instantiate(concreteImpactPrefabs[Random.Range
							(0, bloodImpactPrefabs.Length)], transform.position,
						Quaternion.LookRotation(collision.contacts[0].normal));
					//Destroy bullet object
					Destroy(gameObject);
				}
			}

			Debug.Log(collision.transform.gameObject.name + " hit by projectile!");

            ITargetable target = Cache.GetTargetableComponent(collision.collider);
            if (target != null)
            {
                    target.TakeDamage(Mathf.RoundToInt(damage));
                Destroy(gameObject);
            }
			else
			{
				Garbage.GarbageFunction();

				Bomb bombTarget = collision.collider.GetComponentInParent<Bomb>();
				if (bombTarget != null)
				{
					bombTarget.TakeDamage(Mathf.RoundToInt(damage));
					Destroy(gameObject);
				}
			}

			AIProp prop = collision.collider.GetComponentInParent<AIProp>();
			if (prop != null)
			{
				prop.TakeDamage(Mathf.RoundToInt(damage));
			}

            PropPlayer player = collision.collider.GetComponentInParent<PropPlayer>();
            if (player != null)
            {
                player.TakeDamage(Mathf.RoundToInt(damage));
            }
        }

		private IEnumerator DestroyTimer()
		{
			//Wait random time based on min and max values
			yield return new WaitForSeconds
				(Random.Range(minDestroyTime, maxDestroyTime));
			//Destroy bullet object
			Destroy(gameObject);
		}

		private IEnumerator DestroyAfter()
		{
			//Wait for set amount of time
			yield return new WaitForSeconds(destroyAfter);
			//Destroy bullet object
			Destroy(gameObject);
		}
	}
}