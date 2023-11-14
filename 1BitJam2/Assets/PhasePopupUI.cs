using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI), typeof(Animator))]
public class PhasePopupUI : MonoBehaviour
{
   private TextMeshProUGUI phaseText;
   private Animator animator;
   public bool animActive;
    private void OnEnable()
    {
        PhaseController.OnPhaseChanged += ShowPopup;
    }
    
    private void OnDisable()
    {
        PhaseController.OnPhaseChanged -= ShowPopup;
    }

    private void Awake()
    {
        phaseText = GetComponent<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
    }

    void ShowPopup(PhaseController.GamePhase phase)
    {
        if (animActive)
            animator.Play("PhasePopup_Show", 0, 0);
        else
            animator.SetTrigger("ShowPopup");
        
        phaseText.text = phase.ToString();

        

    }

}
