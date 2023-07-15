using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterWeaponManager : MonoBehaviour
{
    public CharacterProjectile characterProjectile;
    public CharacterLookAim characterLookAim;

    [SerializeField] Sprite portalSprite;
    [SerializeField] Sprite bulletSprite;

    [SerializeField] Image image;

    public bool hasCoolDown = true;
    public float hasCoolDownTime = 3;
    public float count = 0;
    void Start()
    {
        image.sprite = bulletSprite;
        characterLookAim.enabled = false;
        characterProjectile.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasCoolDown)
        {
            count += Time.deltaTime;
            if(count > hasCoolDownTime)
            {
                hasCoolDown = true;
                count = 0;
            }    
        }
            
            


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            image.sprite = bulletSprite;
            characterLookAim.enabled = false;
            characterProjectile.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            image.sprite = portalSprite;
            characterLookAim.enabled = true;
            characterProjectile.enabled = false;
        }

    }

    public void DisableCoolDown()
    {
        hasCoolDown = false;
    }
}
