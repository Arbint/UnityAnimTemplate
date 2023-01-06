using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ComboDescription
{
    public AnimationClip comboClip;
    public float comboClearWaitTime;
}

public class AnimEventComponent : MonoBehaviour
{
    [Header("Combo Setup")]
    [SerializeField] ComboDescription[] combos;
    [SerializeField] AnimationClip comboOverrideOrigionalClip;
    Coroutine StopComboCoroutine;

    int nextComboIndex = 0;
    bool shouldUpdateComboWhenClicked = false;
    bool comboOnGoing = false;

    Animator animator;
    List<AnimatorOverrideController> comboOverrideControllers = new List<AnimatorOverrideController>();
  
    bool running;
    MovmentComponent movmentComp;
    GroundChecker groundChecker;
    bool onGround = true;

    float fullBodyAmt = 1f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        GenerateComboOverrides();
        movmentComp = GetComponent<MovmentComponent>();
        groundChecker = GetComponent<GroundChecker>();
        InputManager.GetPlayerInput().Gameplay.Combo.performed += ComboBtnClicked;
        InputManager.GetPlayerInput().Gameplay.Emote.performed += DoEmote;
        InputManager.GetPlayerInput().Gameplay.Death.performed += DoDeath;
    }

    private void DoDeath(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        animator.SetTrigger("die");
    }

    private void DoEmote(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        animator.SetTrigger("emote");
    }

    private void ComboBtnClicked(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if(comboOnGoing)
        {
            if(shouldUpdateComboWhenClicked)
            {
                nextComboIndex = (nextComboIndex + 1) % combos.Length;
                shouldUpdateComboWhenClicked = false;
            }
        }
        else
        {
            StartCoroutine(CommitCombo(nextComboIndex));
        }
    }

    IEnumerator StopCombo(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        nextComboIndex = 0;
        //Debug.Log("Combo Cleared");
    }

    // Update is called once per frame
    void Update()
    {
        GenerateData();
        UpdateAnimation();
        DetermineAttackBodyMode();
    }
    private void GenerateData()
    {
        Vector3 vel = movmentComp.Velocity;
        vel.y = 0;
        running = vel.magnitude > 0;
        bool prevOnGround = onGround;
        onGround = groundChecker.IsOnGround();
        if(onGround && !prevOnGround)
        {
            animator.SetTrigger("landed");
        }
    }
    private void UpdateAnimation()
    {
        animator.SetBool("inAir", !onGround);
        animator.SetBool("running", running);
    }

    private void GenerateComboOverrides()
    {
        foreach (ComboDescription comboDescription in combos)
        {
            AnimatorOverrideController comboOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(comboOverrideOrigionalClip, comboDescription.comboClip));
            comboOverrideController.ApplyOverrides(overrides);
            comboOverrideControllers.Add(comboOverrideController);
        }
    }

    public void NextComboStart()
    {
        shouldUpdateComboWhenClicked = true;
    }

    public void ClearCombo()
    {
        //combo never issued, otherwise should be false, fallback to first combo
        if (shouldUpdateComboWhenClicked)
        {
            nextComboIndex = 0;
        }

        shouldUpdateComboWhenClicked = false;
    }

    IEnumerator CommitCombo(int comboIndex)
    {
        ComboDescription comboDescription = combos[comboIndex];
        
        if (StopComboCoroutine != null)
            StopCoroutine(StopComboCoroutine);

        StopComboCoroutine = StartCoroutine(StopCombo(comboDescription.comboClearWaitTime));

        animator.runtimeAnimatorController = comboOverrideControllers[comboIndex];
        animator.SetTrigger("nextCombo");
        comboOnGoing = true;
        yield return new WaitForSeconds(comboDescription.comboClip.length);
        comboOnGoing = false;
    }

    void DetermineAttackBodyMode()
    {
        if(running || !onGround)
        {
            fullBodyAmt -= Time.deltaTime;
        }
        else
        {
            fullBodyAmt += Time.deltaTime;
        }

        fullBodyAmt = Mathf.Clamp(fullBodyAmt, 0, 1);
        animator.SetLayerWeight(3, fullBodyAmt);
        animator.SetLayerWeight(2, 1 - fullBodyAmt);
    }
}
