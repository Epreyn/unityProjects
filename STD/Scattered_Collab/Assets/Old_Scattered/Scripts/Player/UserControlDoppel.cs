using UnityEngine;

namespace Scattered
{
    [RequireComponent(typeof(Doppel))]
    public class UserControlDoppel : MonoBehaviour
    {
        private Doppel _doppel;
        public float _move;

        private GameObject _neidan, _sC, _camera;

        private void Awake()
        {
            _doppel = GetComponent<Doppel>();

            _neidan = GameObject.FindGameObjectWithTag("Neidan");
            _sC = GameObject.Find("SceneController");
            _camera = GameObject.FindGameObjectWithTag("MainCamera");
        }


        private void Update()
        {
        }


        private void FixedUpdate()
        {
            if(_camera.GetComponent<Camera2DFollow>().target ==  null)
                _camera.GetComponent<Camera2DFollow>().target = _neidan.transform;

            if(!_sC.GetComponent<MasterManager>()._dialogueEnded
                || (_camera.GetComponent<Camera2DFollow>().target.name != "Neidan")) {
                _move = 0;
                _doppel.Move(_move);
            } else {
                _move = Input.GetAxis("Move_Doppel");
                _doppel.Move(_move);
            }
        }
    }
}
