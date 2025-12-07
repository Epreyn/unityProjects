using System;
using UnityEngine;

namespace Scattered
{
    [RequireComponent(typeof(Neidan))]
    public class UserControl : MonoBehaviour
    {
        private Neidan _neidan;
        public bool _jump;
        public float _move;

        private GameObject _sC, _camera;

        private void Awake()
        {
            _neidan = GetComponent<Neidan>();
            _sC = GameObject.Find("SceneController");
            _camera = GameObject.FindGameObjectWithTag("MainCamera");
        }


        private void Update()
        {
            if(_camera.GetComponent<Camera2DFollow>().target ==  null)
                _camera.GetComponent<Camera2DFollow>().target = _neidan.transform;

            if(!_sC.GetComponent<MasterManager>()._dialogueEnded
                || _camera.GetComponent<Camera2DFollow>().target.name != "Neidan") return;

            if (!_jump) _jump = Input.GetButtonDown("Jump_Neidan");
        }


        private void FixedUpdate()
        {
            _neidan.UpdateAnimation();

            if(_camera.GetComponent<Camera2DFollow>().target ==  null)
                _camera.GetComponent<Camera2DFollow>().target = _neidan.transform;

            if(!_sC.GetComponent<MasterManager>()._dialogueEnded
                || (_camera.GetComponent<Camera2DFollow>().target.name != "Neidan"))
                    _neidan.Move(0, false);
            else{
                _move = Input.GetAxis("Move_Neidan");

                _neidan.Move(_move, _jump);
                _jump = false;
            }
        }
    }
}
