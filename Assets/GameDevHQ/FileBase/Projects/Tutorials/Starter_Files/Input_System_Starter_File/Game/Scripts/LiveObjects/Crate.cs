using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

namespace Game.Scripts.LiveObjects
{
    public class Crate : MonoBehaviour
    {
        [SerializeField] private float _punchDelay;
        [SerializeField] private GameObject _wholeCrate, _brokenCrate;
        [SerializeField] private Rigidbody[] _pieces;
        [SerializeField] private BoxCollider _crateCollider;
        [SerializeField] private InteractableZone _interactableZone;
        private bool _isReadyToBreak = false;

        private List<Rigidbody> _brakeOff = new List<Rigidbody>();

        private PlayerInputActions _inputActions; //

        private void OnEnable() //
        {
            _inputActions = new PlayerInputActions();
            _inputActions.Crate.Enable();
            _inputActions.Crate.Break.performed += Break;
            _inputActions.Crate.DoubleBreak.performed += DoubleBreak; // EDIT
        }

        /*  private void OnEnable()
          {
              InteractableZone.onZoneInteractionComplete += InteractableZone_onZoneInteractionComplete;
          } */

        private void Break(UnityEngine.InputSystem.InputAction.CallbackContext context) //
        {
            if (_isReadyToBreak == false && _brakeOff.Count > 0)
            {
                _wholeCrate.SetActive(false);
                _brokenCrate.SetActive(true);
                _isReadyToBreak = true;
            }

            if (_isReadyToBreak && _interactableZone.GetZoneID() == 6 && _interactableZone.IsInZone()) //Crate zone            
            {
                _interactableZone.OnPressKeyHit();
                if (_brakeOff.Count > 0)
                {
                    BreakPart();
                    StartCoroutine(PunchDelay());
                }
                else if (_brakeOff.Count == 0)
                {
                    _isReadyToBreak = false;
                    _crateCollider.enabled = false;
                    _interactableZone.CompleteTask(6);
                    Debug.Log("Completely Busted");
                }
            }
        }

        private void DoubleBreak(UnityEngine.InputSystem.InputAction.CallbackContext context) // EDIT
        {
            Break(context);
            if (_brakeOff.Count > 0)
            {
                Break(context);
            }
        }

        /*  private void InteractableZone_onZoneInteractionComplete(InteractableZone zone)
          {

              if (_isReadyToBreak == false && _brakeOff.Count >0)
              {
                  _wholeCrate.SetActive(false);
                  _brokenCrate.SetActive(true);
                  _isReadyToBreak = true;
              }

              if (_isReadyToBreak && zone.GetZoneID() == 6) //Crate zone            
              {
                  if (_brakeOff.Count > 0)
                  {
                      BreakPart();
                      StartCoroutine(PunchDelay());
                  }
                  else if(_brakeOff.Count == 0)
                  {
                      _isReadyToBreak = false;
                      _crateCollider.enabled = false;
                      _interactableZone.CompleteTask(6);
                      Debug.Log("Completely Busted");
                  }
              }
          } */

        private void Start()
        {
            _brakeOff.AddRange(_pieces);
            
        }



        public void BreakPart()
        {
            int rng = Random.Range(0, _brakeOff.Count);
            _brakeOff[rng].constraints = RigidbodyConstraints.None;
            _brakeOff[rng].AddForce(new Vector3(1f, 1f, 1f), ForceMode.Force); // SWITCH BACK TO ONE
            _brakeOff.Remove(_brakeOff[rng]);       
        }

        IEnumerator PunchDelay()
        {
            float delayTimer = 0;
            while (delayTimer < _punchDelay)
            {
                yield return new WaitForEndOfFrame();
                delayTimer += Time.deltaTime;
            }

            _interactableZone.ResetAction(6);
        }

        private void OnDisable() //
        {
            _inputActions.Crate.Break.performed -= Break;
            _inputActions.Crate.Disable();

        }

     /*   private void OnDisable()
        {
            InteractableZone.onZoneInteractionComplete -= InteractableZone_onZoneInteractionComplete;
        } */
    }
}