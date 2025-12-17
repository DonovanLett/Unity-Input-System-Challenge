using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Scripts.LiveObjects;
using Cinemachine;

namespace Game.Scripts.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        private CharacterController _controller;
        private Animator _anim;
        [SerializeField]
        private float _speed = 5.0f;
        private bool _playerGrounded;
        [SerializeField]
        private Detonator _detonator;
        private bool _canMove = true;
        [SerializeField]
        private CinemachineVirtualCamera _followCam;
        [SerializeField]
        private GameObject _model;

        private PlayerInputActions _playerInput; //

        [SerializeField]
        private InteractableZone _pickUpC4InteractableZone, _placeC4InteractableZone, _detonateC4InteractableZone; //   // MAKE EMPTY BEFORE MERGING WITH DEV


        private void OnEnable() //
        {
            _playerInput = new PlayerInputActions();
            _playerInput.Player.Enable();

            _playerInput.Detonator.Enable();
            _playerInput.Detonator.PickupC4.performed += PickupC4;
            _playerInput.Detonator.PlaceC4.performed += ShowDetonatorInHand;
            _playerInput.Detonator.DetonateC4.performed += TriggerExplosive;

          //  InteractableZone.onZoneInteractionComplete += InteractableZone_onZoneInteractionComplete;
            Laptop.onHackComplete += ReleasePlayerControl;
            Laptop.onHackEnded += ReturnPlayerControl;
            Forklift.onDriveModeEntered += ReleasePlayerControl;
            Forklift.onDriveModeExited += ReturnPlayerControl;
            Forklift.onDriveModeEntered += HidePlayer;
            Drone.OnEnterFlightMode += ReleasePlayerControl;
            Drone.onExitFlightmode += ReturnPlayerControl;
        }

        /*  private void OnEnable()
          {
              InteractableZone.onZoneInteractionComplete += InteractableZone_onZoneInteractionComplete;
              Laptop.onHackComplete += ReleasePlayerControl;
              Laptop.onHackEnded += ReturnPlayerControl;
              Forklift.onDriveModeEntered += ReleasePlayerControl;
              Forklift.onDriveModeExited += ReturnPlayerControl;
              Forklift.onDriveModeEntered += HidePlayer;
              Drone.OnEnterFlightMode += ReleasePlayerControl;
              Drone.onExitFlightmode += ReturnPlayerControl;
          } */

        private void Start()
        {
            _controller = GetComponent<CharacterController>();

            if (_controller == null)
                Debug.LogError("No Character Controller Present");

            _anim = GetComponentInChildren<Animator>();

            if (_anim == null)
                Debug.Log("Failed to connect the Animator");
        }

        private void Update()
        {
            if (_canMove == true)
                CalcutateMovement();

        }

        private void CalcutateMovement() //
        {
            _playerGrounded = _controller.isGrounded;

            var inputMovement = _playerInput.Player.Movement.ReadValue<Vector2>();
            float h = inputMovement.x;
            float v = inputMovement.y;

            transform.Rotate(transform.up, h);

            var direction = transform.forward * v;
            var velocity = direction * _speed;


            _anim.SetFloat("Speed", Mathf.Abs(velocity.magnitude));


            if (_playerGrounded)
                velocity.y = 0f;
            if (!_playerGrounded)
            {
                velocity.y += -20f * Time.deltaTime;
            }

            _controller.Move(velocity * Time.deltaTime);

        }

        /* private void CalcutateMovement()
        {
            _playerGrounded = _controller.isGrounded;
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            transform.Rotate(transform.up, h);

            var direction = transform.forward * v;
            var velocity = direction * _speed;


            _anim.SetFloat("Speed", Mathf.Abs(velocity.magnitude));


            if (_playerGrounded)
                velocity.y = 0f;
            if (!_playerGrounded)
            {
                velocity.y += -20f * Time.deltaTime;
            }
            
            _controller.Move(velocity * Time.deltaTime);                      

        } */

        private void PickupC4(UnityEngine.InputSystem.InputAction.CallbackContext context) //
        {
            if (_pickUpC4InteractableZone.GetZoneID() == 0 && _pickUpC4InteractableZone.IsInZone())
            {
                _pickUpC4InteractableZone.OnPressKeyHit();
            }
        }

        private void ShowDetonatorInHand(UnityEngine.InputSystem.InputAction.CallbackContext context) //
        {
            if(_placeC4InteractableZone.GetZoneID() == 1 && _placeC4InteractableZone.IsInZone())
            {
                _detonator.Show();
            }
        }

      /*  private void InteractableZone_onZoneInteractionComplete(InteractableZone zone)
        {
            switch(zone.GetZoneID())
            {
                case 1: //place c4
                    _detonator.Show();
                    break;
                case 2: //Trigger Explosion
                    TriggerExplosive();
                    break;
            }
        } */

        private void ReleasePlayerControl() //
        {
            _canMove = false;
            _followCam.Priority = 9;
        }

        /* private void ReleasePlayerControl()
         {
             _canMove = false;
             _followCam.Priority = 9;
         } */

        private void ReturnPlayerControl() //
        {
            _model.SetActive(true);
            _canMove = true;
            _followCam.Priority = 10;
        }

       /* private void ReturnPlayerControl()
        {
            _model.SetActive(true);
            _canMove = true;
            _followCam.Priority = 10;
        } */

        private void HidePlayer() //
        {
            _model.SetActive(false);
        }

       /* private void HidePlayer()
        {
            _model.SetActive(false);
        } */

        private void TriggerExplosive(UnityEngine.InputSystem.InputAction.CallbackContext context) //
        {
            if (_detonateC4InteractableZone.GetZoneID() == 2 && _detonateC4InteractableZone.IsInZone())
            {
                _detonator.TriggerExplosion();
            }
        }

       /* private void TriggerExplosive()
        {
            _detonator.TriggerExplosion();
        } */

        private void OnDisable() //
        {
            _playerInput.Player.Disable();
            _playerInput.Detonator.Disable();
          //  InteractableZone.onZoneInteractionComplete -= InteractableZone_onZoneInteractionComplete;
            Laptop.onHackComplete -= ReleasePlayerControl;
            Laptop.onHackEnded -= ReturnPlayerControl;
            Forklift.onDriveModeEntered -= ReleasePlayerControl;
            Forklift.onDriveModeExited -= ReturnPlayerControl;
            Forklift.onDriveModeEntered -= HidePlayer;
            Drone.OnEnterFlightMode -= ReleasePlayerControl;
            Drone.onExitFlightmode -= ReturnPlayerControl;
        }

      /*  private void OnDisable()
        {
            InteractableZone.onZoneInteractionComplete -= InteractableZone_onZoneInteractionComplete;
            Laptop.onHackComplete -= ReleasePlayerControl;
            Laptop.onHackEnded -= ReturnPlayerControl;
            Forklift.onDriveModeEntered -= ReleasePlayerControl;
            Forklift.onDriveModeExited -= ReturnPlayerControl;
            Forklift.onDriveModeEntered -= HidePlayer;
            Drone.OnEnterFlightMode -= ReleasePlayerControl;
            Drone.onExitFlightmode -= ReturnPlayerControl;
        } */

    }
}