using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Drawing;
using Unity.VisualScripting;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.SceneManagement;
using System;

public class BossHealth : MonoBehaviour
{
    private readonly Dictionary<string, List<string>> mixedColors = new()
        {{"Purple", new(){"Red","Blue"}},
         {"Orange", new(){"Yellow","Red"}},
         {"Green", new(){"Yellow","Blue"}}};
    private readonly List<string> basicColors = new() { "Red", "Blue", "Yellow" };

    private Dictionary<string, float> currentHealth;
    public float maxHealth = 100f;
    private Dictionary<string, Image> healthFill;
    private GameObject healthBarBackground;
    EnemyAudioManager audioManagerScript;
    AudioManager musicAudioScript;
    public enum BossType {Boss1, Boss2}
    public BossType currentBossType; 
    static public bool bossDefeated = false;
    //private bool endBoss = false;


    void Start()
    {
        GameObject audioManager = GameObject.FindGameObjectWithTag("Enemy Audio");
        audioManagerScript = audioManager.GetComponent<EnemyAudioManager>();
        GameObject musicManager = GameObject.FindGameObjectWithTag("Audio");
        musicAudioScript = musicManager.GetComponent<AudioManager>();
        currentHealth = new Dictionary<string, float>()
        {{"Red", -1}, {"Blue", -1}, {"Orange", -1}, {"Green",-1 }, {"Purple", -1}};

        healthFill = new Dictionary<string, Image>();
        Transform child = transform.Find("Canvas").Find("Health Bar Background");
        foreach (var i in currentHealth.Keys)
            healthFill[i] = child.transform.Find(i + " Health") ? child.Find(i + " Health").GetComponent<Image>() : null;

        foreach (var h in healthFill)
            if (h.Value)
            {
                h.Value.fillAmount = 1f;
                currentHealth[h.Key] = maxHealth;
            }
        healthBarBackground = child.gameObject;
        //endBoss = gameObject.name.Contains("Tricolor");
    }
    public void TakeDamage(List<float> damage)
    {
        //damage = { red_damage, blue_damage, yellow_damage };
        List<string> damaged = new();
        var zip = basicColors.Zip(damage, (c, d) => new { Color = c, Damage = d }).ToList();
        foreach (var z in zip)
        {
            if (z.Damage != 0)
            {
                if (currentHealth.TryGetValue(z.Color, out float v) && v != -1) damaged.Add(z.Color);
                damaged.AddRange(from mc in mixedColors where mc.Value.Contains(z.Color) select mc.Key);
                foreach (var d in damaged)
                    if (currentHealth[d] > 0)
                    {
                        currentHealth[d] -= z.Damage * .5f;
                        //This is only .5 because I split the damage across its two mixed colors.
                        currentHealth[d] = Mathf.Clamp(currentHealth[d], 0, maxHealth);
                        if (currentHealth[d] > 0) healthFill[d].fillAmount = currentHealth[d] / maxHealth;
                        else healthFill[d].gameObject.SetActive(false);
                    }
                damaged.Clear();
            }
        }
        if (GetTotalHealth() <= 0) Die();
    }

    void Die()
    {
        DataCollector.tempPoints += 1000;
        healthBarBackground.SetActive(false);
        GetComponent<Animator>().SetBool("isDead", true);
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.enabled = false;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        audioManagerScript.playEnemySFX(audioManagerScript.bossDeath);
        StartCoroutine(DeathFreeze());
    }

    private IEnumerator DeathFreeze()
    {
        yield return new WaitForSeconds(1.0f);
        //GetComponent<Animator>().speed = 0;
        musicAudioScript.playStageClear(musicAudioScript.StageClear);
        yield return new WaitForSeconds(3.8f);
        DataCollector.UpdateScores();
        if (currentBossType == BossType.Boss1){    //!endBoss
            bossDefeated = true;
            UnlockNewLevel();
            UnlockLevel2();
            SceneControllerScript.instance.LoadEndCard("Grom's Shop");
        }
        else if(currentBossType == BossType.Boss2){   //endBoss
            if(bossDefeated)
            {
                UnlockNewLevel();
                SceneControllerScript.instance.LoadEndCard("Good Ending");
                //SceneManager.LoadScene("Good Ending");
            }
            else
            {
                UnlockNewLevel();
                SceneControllerScript.instance.LoadEndCard("Bad Ending");
                //SceneManager.LoadScene("Bad Ending");
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D coll)
    {
        GameObject collidedWith = coll.gameObject;
        string[] tagA = collidedWith.tag.Split(" ");
        string col = tagA[0], proj = "";
        if (tagA.Length > 2) proj = tagA[2];
        if (basicColors.Contains(col) && proj != "")
        {
            Dictionary<string, float> dmg = new();
            foreach (var c in basicColors) dmg[c] = 0;
            float dam = 10f;
            if (proj == "Cone"){
                 dam *= 2f;
                 Destroy(collidedWith);
            }
            if (proj == "Bullet") Destroy(collidedWith);
            dmg[col] = dam;
            TakeDamage(dmg.Values.ToList());
        }
    }


    public float GetTotalHealth()
    {
        return currentHealth.Values.Sum();
    }

    void UnlockNewLevel(){
        if(LevelMenu.unlockedLevel == 2){
            LevelMenu.unlockedLevel++;

        }

        else if(LevelMenu.unlockedLevel == 4){
            LevelMenu.unlockedLevel++;
        }
    }
    void UnlockLevel2(){
        if(LevelMenu.unlockedLevel == 3){
            LevelMenu.unlockedLevel++;
        }
    }
}
