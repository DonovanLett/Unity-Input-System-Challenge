using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.LiveObjects
{
    public class EndZone : MonoBehaviour
    {
        private PlayerInputActions _endZoneInput; //
        private InteractableZone _interactableZone; //
        private void OnEnable() //
        {
            _interactableZone = GetComponent<InteractableZone>(); //
            _endZoneInput = new PlayerInputActions(); //
            _endZoneInput.EndZone.Enable(); //
            _endZoneInput.EndZone.Restart.performed += Restart; //
        }

      /*  private void OnEnable()
        {
            InteractableZone.onZoneInteractionComplete += InteractableZone_onZoneInteractionComplete;

        } */

        private void Restart(UnityEngine.InputSystem.InputAction.CallbackContext context) //
        {
            if (_interactableZone.GetZoneID() == 7 && _interactableZone.IsInZone())
            {
                _interactableZone.OnPressKeyHit();
                InteractableZone.CurrentZoneID = 0;
                SceneManager.LoadScene(0);
            }
        }

       /* private void InteractableZone_onZoneInteractionComplete(InteractableZone zone)
        {
            if (zone.GetZoneID() == 7)
            {
                InteractableZone.CurrentZoneID = 0;
                SceneManager.LoadScene(0);
            }
        } */

        private void OnDisable()
        {
            _endZoneInput.EndZone.Restart.performed -= Restart; //
        }

     /*   private void OnDisable()
        {
            InteractableZone.onZoneInteractionComplete -= InteractableZone_onZoneInteractionComplete;
        } */
    }
}