using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pedestrian : Entity, IDamageable
{
    public int MoveSpeed;
    public int WaitTimeMax;
    public int WaitTimeMin;
    public int PanicModeTime;

    private NPCSpawner npcSpawner;
    private bool somethingIsInFrontfMeOhNo;
    private bool walkBackwards;
    private bool walk;
    private float panicModeTimeOffset = 1;
    private float panicModeSpeedOffset = 1;
    public AudioSource HurtNpc;
    public SpriteRenderer SpriteRenderer;
    public Sprite SchoolBoy;
    public Sprite Brawler;
    public Sprite TopHat;
    public Sprite Army;
    public Sprite Dog;
    
    public Sprite DeadSchoolBoy;
    public Sprite DeadBrawler;
    public Sprite DeadTopHat;
    public Sprite DeadArmy;
    public Sprite DeadDog;
    private void Update()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        
        // RandomMovement();
        if (walk && IsAlive)
            transform.Translate(Vector3.up * (MoveSpeed * panicModeSpeedOffset)* Time.deltaTime);
        if (walkBackwards && IsAlive)
            transform.Translate(Vector3.down * (MoveSpeed/2 * panicModeSpeedOffset) * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        walk = false;
        somethingIsInFrontfMeOhNo = true;
        walkBackwards = true;
        Invoke("WalkBackwards", 1 * panicModeTimeOffset);
        // transform.Rotate(Vector3.forward * 180);

    }

    private void Start()
    {
        npcSpawner = FindObjectOfType<NPCSpawner>();
    }


    private void OnEnable()
    {
        Health = MaxHealth;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = true;
        Turn();
    }

    void WalkBackwards()
    {
        walkBackwards = false;
        somethingIsInFrontfMeOhNo = false;
        Invoke("Turn", Random.Range(WaitTimeMin, WaitTimeMax) * panicModeTimeOffset);
    }

    void Walk()
    {
        if (!somethingIsInFrontfMeOhNo && IsAlive)
        {
            walk = false;
            Invoke("Turn", Random.Range(WaitTimeMin, WaitTimeMax) * panicModeTimeOffset);
        }
    }

    void Turn()
    {
        if (!somethingIsInFrontfMeOhNo && IsAlive)
        {
            transform.Rotate(Vector3.forward * Random.Range(0, 360));
            walk = true;
            Invoke("Walk", Random.Range(WaitTimeMin, WaitTimeMax) * panicModeTimeOffset);
        }
    }


    public override void TakeDamage(int value, DamageType damageType = null)
    {
        if (damageType.Collision)
        {
            Debug.Log("NPC got run over!");
            value *= 5;
        }

        if (damageType.Water)
        {
            value *= 10;
        }

        StartCoroutine(PanicMode());
        base.TakeDamage(value, damageType);
        StartCoroutine(HurtNoise());
    }

    IEnumerator PanicMode()
    {
        panicModeSpeedOffset = 7;
        panicModeTimeOffset = 0.5f;
        yield return new WaitForSeconds(PanicModeTime);
        panicModeSpeedOffset = 1;
        panicModeTimeOffset = 1;
    }

    IEnumerator HurtNoise()
    {
        yield return new WaitForSeconds(0.5f);
        HurtNpc.Play();
        yield return new WaitForSeconds(5f);
    }

    IEnumerator ChangeSpriteOnDeath()
    {
        if (SpriteRenderer.sprite == SchoolBoy)
            SpriteRenderer.sprite = DeadSchoolBoy;
        if (SpriteRenderer.sprite == TopHat)
            SpriteRenderer.sprite = DeadTopHat;
        if (SpriteRenderer.sprite == Brawler)
            SpriteRenderer.sprite = DeadBrawler;
        if (SpriteRenderer.sprite == Army)
            SpriteRenderer.sprite = DeadArmy;
        if (SpriteRenderer.sprite == Dog)
            SpriteRenderer.sprite = DeadDog;

        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSeconds(10f);
        npcSpawner.ReloadNPC(gameObject, this);
    }

    public override void OnDeath()
    {
        FindObjectOfType<MoneySpawner>().SpawnMoney50(gameObject.transform.position);
        FindObjectOfType<GameController>().AddScore(150);

        StartCoroutine(ChangeSpriteOnDeath());
    }
}
