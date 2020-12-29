using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationReaction : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator burgerAnimator;
    public Animator stellaAnimator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getDestroyed()
    {
        Destroy(gameObject);
    }

    public void monsterPunched()
    {
        burgerAnimator.Play("BurgerPunched");
    }

    public void stellaDamaged()
    {
        stellaAnimator.Play("StellaDamaged");
    }

    public void stellaSpatOn()
    {
        stellaAnimator.Play("StellaSpatOn");
    }

    public void BurgerRoasted()
    {
        burgerAnimator.Play("BurgerRoasted");
    }
}
