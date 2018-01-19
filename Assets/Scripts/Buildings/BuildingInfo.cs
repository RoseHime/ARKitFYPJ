using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfo : MonoBehaviour {

    public float f_health = 50;
    protected float f_maxHealth;
    public bool b_IsUnderAttack = false;

    protected float f_underAttackCooldown = 0;
    protected float f_previousHealth = 50;

    public int i_woodCost = 0;
    public int i_stoneCost = 0;
    public int i_magicStoneCost = 0;

    public Mesh DestroyedMesh;

    private TowerBehaviour towerBehaviour;

    // Use this for initialization
    void Start () {
        if (transform.GetComponent<TowerBehaviour>() != null)
        {
            towerBehaviour = transform.GetComponent<TowerBehaviour>();
        }
        f_maxHealth = f_health;
        f_previousHealth = f_health;
	}

    protected virtual void Update ()
    {
        if (f_health <= 0)
        {
            Destroy(gameObject);
        }
        if (f_health < f_previousHealth)
        {
            f_underAttackCooldown = 0;
            b_IsUnderAttack = true;
        }
        else
        {
            if (f_underAttackCooldown < 5)
            {
                f_underAttackCooldown += Time.deltaTime;
                if (f_underAttackCooldown >= 5)
                {
                    b_IsUnderAttack = false;
                }
            }
        }
        f_previousHealth = f_health;

        if (f_health < f_maxHealth/2)
        {
            if (DestroyedMesh != null)
            {
                gameObject.GetComponent<MeshFilter>().mesh = DestroyedMesh;
            }
        }
    }

    public virtual string GetUnitsInfo()
    {
        string unitInfo = gameObject.name;
        if (towerBehaviour != null)
        {         
            //unitInfo += "HP:" + f_health + "\nDMG" + towerBehaviour.f_damage + "\nRANGE" + towerBehaviour.f_range + "\nSPD" + towerBehaviour.f_fireRate;         
        }
        else
        {
            //unitInfo += "HP:" + f_health;
        }
        return unitInfo;
    }

    void OnClick()
    {
        Transform go_commandPanel = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0);
        go_commandPanel.GetComponent<CreateActionButton>().go_selectedUnit = gameObject;
        go_commandPanel.GetComponent<CreateActionButton>().CreateButtons();
        go_commandPanel.gameObject.SetActive(true);
    }
}
