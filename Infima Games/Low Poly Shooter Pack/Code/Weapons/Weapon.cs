//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;
using UnityEngine.AdaptivePerformance.Provider;
using Shooter;
namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Weapon. This class handles most of the things that weapons need.
    /// </summary>
    public class Weapon : WeaponBehaviour
    {
        #region FIELDS SERIALIZED
        [Title(label: "Raycast settings")]
        [SerializeField] LayerMask spawnGunLayerMask;
        [SerializeField] float rayLength = 15f;

        [Title(label: "Settings")]
        [SerializeField]
        private WeaponType weaponType;

        [Tooltip("Weapon Name. Currently not used for anything, but in the future, we will use this for pickups!")]
        [SerializeField] 
        private string weaponName;

        [Tooltip("How much the character's movement speed is multiplied by when wielding this weapon.")]
        [SerializeField]
        private float multiplierMovementSpeed = 1.0f;
        
        [Title(label: "Firing")]

        [Tooltip("Is this weapon automatic? If yes, then holding down the firing button will continuously fire.")]
        [SerializeField] 
        private bool automatic;
        
        [Tooltip("Is this weapon bolt-action? If yes, then a bolt-action animation will play after every shot.")]
        [SerializeField]
        private bool boltAction;

        [Tooltip("Amount of shots fired at once. Helpful for things like shotguns, where there are multiple projectiles fired at once.")]
        [SerializeField]
        private int shotCount = 1;
        
        [Tooltip("How far the weapon can fire from the center of the screen.")]
        [SerializeField]
        private float spread = 0.25f;
        
        [Tooltip("How fast the projectiles are.")]
        [SerializeField]
        private float projectileImpulse = 400.0f;

        [Tooltip("Amount of shots this weapon can shoot in a minute. It determines how fast the weapon shoots.")]
        [SerializeField] 
        private int roundsPerMinutes = 200;

        [Title(label: "Reloading")]
        
        [Tooltip("Determines if this weapon reloads in cycles, meaning that it inserts one bullet at a time, or not.")]
        [SerializeField]
        private bool cycledReload;
        
        [Tooltip("Determines if the player can reload this weapon when it is full of ammunition.")]
        [SerializeField]
        private bool canReloadWhenFull = true;

        [Tooltip("Should this weapon be reloaded automatically after firing its last shot?")]
        [SerializeField]
        private bool automaticReloadOnEmpty;

        [Tooltip("Time after the last shot at which a reload will automatically start.")]
        [SerializeField]
        private float automaticReloadOnEmptyDelay = 0.25f;

        [Title(label: "Animation")]

        [Tooltip("Transform that represents the weapon's ejection port, meaning the part of the weapon that casings shoot from.")]
        [SerializeField]
        private Transform socketEjection;

        [Tooltip("Settings this to false will stop the weapon from being reloaded while the character is aiming it.")]
        [SerializeField]
        private bool canReloadAimed = true;

        [Title(label: "Resources")]

        [Tooltip("Casing Prefab.")]
        [SerializeField]
        private GameObject prefabCasing;
        
        [Tooltip("Projectile Prefab. This is the prefab spawned when the weapon shoots.")]
        [SerializeField]
        private GameObject prefabProjectile;
        
        [Tooltip("The AnimatorController a player character needs to use while wielding this weapon.")]
        [SerializeField] 
        public RuntimeAnimatorController controller;

        [Tooltip("Weapon Body Texture.")]
        [SerializeField]
        private Sprite spriteBody;
        
        [Title(label: "Audio Clips Holster")]

        [Tooltip("Holster Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipHolster;

        [Tooltip("Unholster Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipUnholster;
        
        [Title(label: "Audio Clips Reloads")]

        [Tooltip("Reload Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipReload;
        
        [Tooltip("Reload Empty Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipReloadEmpty;
        
        [Title(label: "Audio Clips Reloads Cycled")]
        
        [Tooltip("Reload Open Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipReloadOpen;
        
        [Tooltip("Reload Insert Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipReloadInsert;
        
        [Tooltip("Reload Close Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipReloadClose;
        
        [Title(label: "Audio Clips Other")]

        [Tooltip("AudioClip played when this weapon is fired without any ammunition.")]
        [SerializeField]
        private AudioClip audioClipFireEmpty;
        
        [Tooltip("")]
        [SerializeField]
        private AudioClip audioClipBoltAction;

        #endregion

        #region FIELDS

        /// <summary>
        /// Weapon Animator.
        /// </summary>
        private Animator animator;
        /// <summary>
        /// Attachment Manager.
        /// </summary>
        private WeaponAttachmentManagerBehaviour attachmentManager;

        /// <summary>
        /// Amount of ammunition left.
        /// </summary>
        private int ammunitionCurrent;

        #region Attachment Behaviours
        
        /// <summary>
        /// Equipped scope Reference.
        /// </summary>
        private ScopeBehaviour scopeBehaviour;
        
        /// <summary>
        /// Equipped Magazine Reference.
        /// </summary>
        private MagazineBehaviour magazineBehaviour;
        /// <summary>
        /// Equipped Muzzle Reference.
        /// </summary>
        private MuzzleBehaviour muzzleBehaviour;

        /// <summary>
        /// Equipped Laser Reference.
        /// </summary>
        private LaserBehaviour laserBehaviour;
        /// <summary>
        /// Equipped Grip Reference.
        /// </summary>
        private GripBehaviour gripBehaviour;

        #endregion

        /// <summary>
        /// The GameModeService used in this game!
        /// </summary>
        private IGameModeService gameModeService;
        /// <summary>
        /// The main player character behaviour component.
        /// </summary>
        private CharacterBehaviour characterBehaviour;

        /// <summary>
        /// The player character's camera.
        /// </summary>
        private Transform playerCamera;

        //Spawn item prefab use for special weapon SpawnGun.
        private GameObject spawnItemPrefab;

        private SyringeType currentSyringeType;

        //Nextbot type will be spawn by SpawnGun.
        private NextbotType nextbotType;
        private UploadedImageData uploadImageData;

        #endregion

        #region UNITY
        
        protected override void Awake()
        {
            //Get Animator.
            animator = GetComponent<Animator>();
            //Get Attachment Manager.
            attachmentManager = GetComponent<WeaponAttachmentManagerBehaviour>();

            //Cache the game mode service. We only need this right here, but we'll cache it in case we ever need it again.
            gameModeService = ServiceLocator.Current.Get<IGameModeService>();
            //Cache the player character.
            characterBehaviour = gameModeService.GetPlayerCharacter();
            //Cache the world camera. We use this in line traces.
            playerCamera = characterBehaviour.GetCameraWorld().transform;

            if (weaponType == WeaponType.SpawnGun)
            {
                //Register event to change spawn item on special weapon SpawnGun
                this.RegisterListener(EventID.OnChangeSpawnItem, (param) =>
                {
                    spawnItemPrefab = (GameObject)param;
                });

                this.RegisterListener(EventID.OnChangeSpawnNextbot, (param) =>
                {
                    if (param is NextbotSpawnData)
                    {
                        var message = param as NextbotSpawnData;
                        nextbotType = message.type;
                        uploadImageData = message.uploadImageData;
                    }
                });
            }
            else if (weaponType == WeaponType.ToolGun)
            {
                this.RegisterListener(EventID.OnChangeSyringeType, (param) =>
                {
                    currentSyringeType = (SyringeType)param;
                });
            }
        }
        protected override void Start()
        {
            #region Cache Attachment References

            //Get Scope.
            scopeBehaviour = attachmentManager.GetEquippedScope();
            
            //Get Magazine.
            magazineBehaviour = attachmentManager.GetEquippedMagazine();
            //Get Muzzle.
            muzzleBehaviour = attachmentManager.GetEquippedMuzzle();

            //Get Laser.
            laserBehaviour = attachmentManager.GetEquippedLaser();
            //Get Grip.
            gripBehaviour = attachmentManager.GetEquippedGrip();

            #endregion

            //Max Out Ammo.
            ammunitionCurrent = magazineBehaviour.GetAmmunitionTotal();

            this.PostEvent(EventID.UpdateMainGunBullet, ammunitionCurrent.ToString());
        }
        #endregion

        #region GETTERS

        /// <summary>
        /// GetFieldOfViewMultiplierAim.
        /// </summary>
        public override float GetFieldOfViewMultiplierAim()
        {
            //Make sure we don't have any issues even with a broken setup!
            if (scopeBehaviour != null) 
                return scopeBehaviour.GetFieldOfViewMultiplierAim();
            
            //Error.
            Debug.LogError("Weapon has no scope equipped!");
  
            //Return.
            return 1.0f;
        }
        /// <summary>
        /// GetFieldOfViewMultiplierAimWeapon.
        /// </summary>
        public override float GetFieldOfViewMultiplierAimWeapon()
        {
            //Make sure we don't have any issues even with a broken setup!
            if (scopeBehaviour != null) 
                return scopeBehaviour.GetFieldOfViewMultiplierAimWeapon();
            
            //Error.
            Debug.LogError("Weapon has no scope equipped!");
  
            //Return.
            return 1.0f;
        }
        
        /// <summary>
        /// GetAnimator.
        /// </summary>
        public override Animator GetAnimator() => animator;
        /// <summary>
        /// CanReloadAimed.
        /// </summary>
        public override bool CanReloadAimed() => canReloadAimed;

        /// <summary>
        /// GetSpriteBody.
        /// </summary>
        public override Sprite GetSpriteBody() => spriteBody;
        /// <summary>
        /// GetMultiplierMovementSpeed.
        /// </summary>
        public override float GetMultiplierMovementSpeed() => multiplierMovementSpeed;

        /// <summary>
        /// GetAudioClipHolster.
        /// </summary>
        public override AudioClip GetAudioClipHolster() => audioClipHolster;
        /// <summary>
        /// GetAudioClipUnholster.
        /// </summary>
        public override AudioClip GetAudioClipUnholster() => audioClipUnholster;

        /// <summary>
        /// GetAudioClipReload.
        /// </summary>
        public override AudioClip GetAudioClipReload() => audioClipReload;
        /// <summary>
        /// GetAudioClipReloadEmpty.
        /// </summary>
        public override AudioClip GetAudioClipReloadEmpty() => audioClipReloadEmpty;
        
        /// <summary>
        /// GetAudioClipReloadOpen.
        /// </summary>
        public override AudioClip GetAudioClipReloadOpen() => audioClipReloadOpen;
        /// <summary>
        /// GetAudioClipReloadInsert.
        /// </summary>
        public override AudioClip GetAudioClipReloadInsert() => audioClipReloadInsert;
        /// <summary>
        /// GetAudioClipReloadClose.
        /// </summary>
        public override AudioClip GetAudioClipReloadClose() => audioClipReloadClose;

        /// <summary>
        /// GetAudioClipFireEmpty.
        /// </summary>
        public override AudioClip GetAudioClipFireEmpty() => audioClipFireEmpty;
        /// <summary>
        /// GetAudioClipBoltAction.
        /// </summary>
        public override AudioClip GetAudioClipBoltAction() => audioClipBoltAction;
        
        /// <summary>
        /// GetAudioClipFire.
        /// </summary>
        public override AudioClip GetAudioClipFire() => muzzleBehaviour.GetAudioClipFire();
        /// <summary>
        /// GetAmmunitionCurrent.
        /// </summary>
        public override int GetAmmunitionCurrent() => ammunitionCurrent;

        /// <summary>
        /// GetAmmunitionTotal.
        /// </summary>
        public override int GetAmmunitionTotal() => magazineBehaviour.GetAmmunitionTotal();
        /// <summary>
        /// HasCycledReload.
        /// </summary>
        public override bool HasCycledReload() => cycledReload;

        /// <summary>
        /// IsAutomatic.
        /// </summary>
        public override bool IsAutomatic() => automatic;
        /// <summary>
        /// IsBoltAction.
        /// </summary>
        public override bool IsBoltAction() => boltAction;

        /// <summary>
        /// GetAutomaticallyReloadOnEmpty.
        /// </summary>
        public override bool GetAutomaticallyReloadOnEmpty() => automaticReloadOnEmpty;
        /// <summary>
        /// GetAutomaticallyReloadOnEmptyDelay.
        /// </summary>
        public override float GetAutomaticallyReloadOnEmptyDelay() => automaticReloadOnEmptyDelay;

        /// <summary>
        /// CanReloadWhenFull.
        /// </summary>
        public override bool CanReloadWhenFull() => canReloadWhenFull;
        /// <summary>
        /// GetRateOfFire.
        /// </summary>
        public override float GetRateOfFire() => roundsPerMinutes;
        
        /// <summary>
        /// IsFull.
        /// </summary>
        public override bool IsFull() => ammunitionCurrent == magazineBehaviour.GetAmmunitionTotal();
        /// <summary>
        /// HasAmmunition.
        /// </summary>
        public override bool HasAmmunition() => ammunitionCurrent > 0;

        /// <summary>
        /// GetAnimatorController.
        /// </summary>
        public override RuntimeAnimatorController GetAnimatorController() => controller;
        /// <summary>
        /// GetAttachmentManager.
        /// </summary>
        public override WeaponAttachmentManagerBehaviour GetAttachmentManager() => attachmentManager;


        public override WeaponType GetWeaponType() => weaponType;
        #endregion

        #region METHODS

        /// <summary>
        /// Reload.
        /// </summary>
        public override void Reload()
        {
            //Set Reloading Bool. This helps cycled reloads know when they need to stop cycling.
            const string boolName = "Reloading";
            animator.SetBool(boolName, true);
            
            //Try Play Reload Sound.
            ServiceLocator.Current.Get<IAudioManagerService>().PlayOneShot(HasAmmunition() ? audioClipReload : audioClipReloadEmpty, new AudioSettings(1.0f, 0.0f, false));
            
            //Play Reload Animation.
            animator.Play(cycledReload ? "Reload Open" : (HasAmmunition() ? "Reload" : "Reload Empty"), 0, 0.0f);
        }
        /// <summary>
        /// Fire.
        /// </summary>
        public override void Fire(float spreadMultiplier = 1.0f)
        {
            //We need a muzzle in order to fire this weapon!
            if (muzzleBehaviour == null)
                return;
            
            //Make sure that we have a camera cached, otherwise we don't really have the ability to perform traces.
            if (playerCamera == null)
                return;

            //Play the firing animation.
            const string stateName = "Fire";
            animator.Play(stateName, 0, 0.0f);

            //Special weapon firing handle! (Spawn gun only spawn object, not for shooting)
            if (weaponType == WeaponType.SpawnGun)
            {
                    Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);
                    Ray ray = Camera.main.ScreenPointToRay(screenCenter);

                    RaycastHit hit;

                if (Physics.Raycast(ray, out hit, rayLength, spawnGunLayerMask))
                {
                    if (spawnItemPrefab != null)
                    {
                        if (hit.collider.gameObject.layer == LayerMask.NameToLayer(Constants.LAYER_GROUND))
                        {
                            Vector3 hitPosition = hit.point;
                            GameObject spawnItem = Instantiate(spawnItemPrefab, hitPosition, Quaternion.identity);

                            ShooterCharacter character = spawnItem.GetComponent<ShooterCharacter>();
                            if (character != null)
                            {
                                if (character.TryGetComponent<NextbotCharacter>(out NextbotCharacter nextbot))
                                {
                                    if (nextbotType == NextbotType.CustomNextbot)
                                    {
                                        nextbot.CustomNextbot(uploadImageData);
                                    }
                                    else
                                    {
                                        nextbot.InitNextbot(nextbotType);
                                    }
                                }

                                if (GameManager.IsState(GameState.Gameplay))
                                {
                                    character.InitCharacter();
                                }
                                BattleManager.Ins.AddCharacter(character);
                            }

                            Animal animal = spawnItem.GetComponent<Animal>();
                            if (animal != null)
                            {
                                if (GameManager.IsState(GameState.Gameplay))
                                {
                                    animal.InitAnimal();
                                }
                                BattleManager.Ins.AddCharacter(animal);
                            }

                            if (spawnItem.GetComponent<ISpawnable>() != null)
                            {
                                LevelManager.Ins.AddSpawnedObject(spawnItem.GetComponent<ISpawnable>());
                            }
                        }
                    }

                    GetComponent<LaserGun>().Fire();
                } 
            }
            //Other special weapon
            else if (weaponType == WeaponType.ToolGun)
            {
                for (var i = 0; i < shotCount; i++)
                {
                    //Determine a random spread value using all of our multipliers.
                    Vector3 spreadValue = Random.insideUnitSphere * (spread * spreadMultiplier);
                    //Remove the forward spread component, since locally this would go inside the object we're shooting!
                    spreadValue.z = 0;
                    //Convert to world space.
                    spreadValue = playerCamera.TransformDirection(spreadValue);

                    //Spawn projectile from the projectile spawn point.
                    GameObject projectile = Instantiate(prefabProjectile, playerCamera.position, Quaternion.Euler(playerCamera.eulerAngles + spreadValue));

                    projectile.GetComponent<InjectBullet>().SyringeType = currentSyringeType;

                    //Add velocity to the projectile.
                    projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileImpulse;
                }
            }
            else if (weaponType == WeaponType.Stinger)
            {
                //Start fight
                if (!GameManager.IsState(GameState.Gameplay))
                {
                    if (BattleManager.Ins.EnemyCount > 0 || BattleManager.Ins.NeutralCount > 0)
                    {
                        GameManager.ChangeState(GameState.Gameplay);
                        BattleManager.Ins.OnBattleStart();
                    }
                }

                //Determine a random spread value using all of our multipliers.
                Vector3 spreadValue = Random.insideUnitSphere * (spread * spreadMultiplier);
                //Remove the forward spread component, since locally this would go inside the object we're shooting!
                spreadValue.z = 0;
                //Convert to world space.
                spreadValue = playerCamera.TransformDirection(spreadValue);

                //Spawn projectile from the projectile spawn point.
                GameObject projectile = Instantiate(prefabProjectile, playerCamera.position, Quaternion.Euler(playerCamera.eulerAngles + spreadValue));

                this.PostEvent(EventID.OnFiringRocket, projectile);
            }
            else if (weaponType == WeaponType.FlameThrower)
            {
                GetComponent<WeaponFlameThrower>().Fire();
            }
            else if (weaponType == WeaponType.WaterGun)
            {
                GetComponent<WeaponWaterGun>().Fire();
            }
            else if (weaponType == WeaponType.PortalGun)
            {
                GetComponent<WeaponPortalGun>().Fire();                   
            }
            else if (weaponType == WeaponType.Crowbar)
            {
                if (!GameManager.IsState(GameState.Gameplay))
                {
                    if (BattleManager.Ins.EnemyCount > 0 || BattleManager.Ins.NeutralCount > 0)
                    {
                        GameManager.ChangeState(GameState.Gameplay);
                        BattleManager.Ins.OnBattleStart();
                    }
                }

                GetComponent<Crowbar>().Attack();
            }
            else if (weaponType == WeaponType.BalloonGun)
            {
                for (var i = 0; i < shotCount; i++)
                {
                    //Determine a random spread value using all of our multipliers.
                    Vector3 spreadValue = Random.insideUnitSphere * (spread * spreadMultiplier);
                    //Remove the forward spread component, since locally this would go inside the object we're shooting!
                    spreadValue.z = 0;
                    //Convert to world space.
                    spreadValue = playerCamera.TransformDirection(spreadValue);

                    //Spawn projectile from the projectile spawn point.
                    GameObject projectile = Instantiate(prefabProjectile, playerCamera.position, Quaternion.Euler(playerCamera.eulerAngles + spreadValue));

                    //Add velocity to the projectile.
                    projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileImpulse;
                }
            }
            //Basic weapon firing handle
            else
            {
                //Start fight
                if (!GameManager.IsState(GameState.Gameplay))
                {
                    if (BattleManager.Ins.EnemyCount > 0 || BattleManager.Ins.NeutralCount > 0)
                    {
                        GameManager.ChangeState(GameState.Gameplay);
                        BattleManager.Ins.OnBattleStart();
                    }
                }

                //Reduce ammunition! We just shot, so we need to get rid of one!
                ammunitionCurrent = Mathf.Clamp(ammunitionCurrent - 1, 0, magazineBehaviour.GetAmmunitionTotal());

                this.PostEvent(EventID.UpdateMainGunBullet, ammunitionCurrent.ToString());

                //Set the slide back if we just ran out of ammunition.
                if (ammunitionCurrent == 0)
                    SetSlideBack(1);

                //Play all muzzle effects.
                muzzleBehaviour.Effect();

                //Spawn as many projectiles as we need.
                for (var i = 0; i < shotCount; i++)
                {
                    //Determine a random spread value using all of our multipliers.
                    Vector3 spreadValue = Random.insideUnitSphere * (spread * spreadMultiplier);
                    //Remove the forward spread component, since locally this would go inside the object we're shooting!
                    spreadValue.z = 0;
                    //Convert to world space.
                    spreadValue = playerCamera.TransformDirection(spreadValue);

                    //Spawn projectile from the projectile spawn point.
                    GameObject projectile = Instantiate(prefabProjectile, playerCamera.position, Quaternion.Euler(playerCamera.eulerAngles + spreadValue));
                    //Add velocity to the projectile.
                    projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileImpulse;
                }

                if (ammunitionCurrent == 0)
                {
                    if (!automaticReloadOnEmpty)
                    {
                        UIManager.Ins.GetUI<UIGameplay>().ToggleMainGunAndMelee();
                    }
                }    
            }
        }

        /// <summary>
        /// FillAmmunition.
        /// </summary>
        public override void FillAmmunition(int amount)
        {
            //Update the value by a certain amount.
            if (magazineBehaviour == null)
            {
                return;
            }  
            
            ammunitionCurrent = amount != 0 ? Mathf.Clamp(ammunitionCurrent + amount, 
                0, GetAmmunitionTotal()) : magazineBehaviour.GetAmmunitionTotal();

            this.PostEvent(EventID.UpdateMainGunBullet, ammunitionCurrent.ToString());
        }
        /// <summary>
        /// SetSlideBack.
        /// </summary>
        public override void SetSlideBack(int back)
        {
            //Set the slide back bool.
            const string boolName = "Slide Back";
            animator.SetBool(boolName, back != 0);
        }

        /// <summary>
        /// EjectCasing.
        /// </summary>
        public override void EjectCasing()
        {
            //Spawn casing prefab at spawn point.
            if(prefabCasing != null && socketEjection != null)
                Instantiate(prefabCasing, socketEjection.position, socketEjection.rotation);
        }

        #endregion
    }
}