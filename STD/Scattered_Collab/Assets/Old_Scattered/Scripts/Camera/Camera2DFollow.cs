using System;
using UnityEngine;

namespace Scattered
{
    [ExecuteInEditMode]
    public class Camera2DFollow : MonoBehaviour
    {
        private GameObject _neidan;

        public Transform target;
        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;

        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;

        [Range(0.1f, 60.0f)]
        public float Zoom;
        private GameObject[] _cameras;
        private GameObject _rayCast;

        public int cameraY = 0;

        // Use this for initialization
        private void Start()
        {
            _neidan = GameObject.FindGameObjectWithTag("Neidan");

            m_LastTargetPosition = new Vector3(target.position.x, 0, 0);
            m_OffsetZ = (transform.position - target.position).z;
            transform.parent = null;

            _cameras = GameObject.FindGameObjectsWithTag("Camera");
            _rayCast = GameObject.FindGameObjectWithTag("RayCast");
        }


        // Update is called once per frame
        private void Update()
        {
            if (target == null) target = _neidan.transform;
            if (target.name == "Neidan") damping = 0;

            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (new Vector3(target.position.x, target.position.y, 0) - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = new Vector3(target.position.x, target.position.y, 0) + m_LookAheadPos + Vector3.forward*m_OffsetZ;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

            transform.position = newPos;

            m_LastTargetPosition = new Vector3(target.position.x, target.position.y, 0);

            if (target.position.y > cameraY + 20) cameraY = cameraY + 20;
            else if (target.position.y < cameraY - 20 ) cameraY = cameraY - 20;

            //ZOOM
            _rayCast.GetComponent<CircleCollider2D>().radius = Zoom * 2;
            GetComponent<Camera>().orthographicSize = Zoom;
            foreach(GameObject c in _cameras){
                c.GetComponent<Camera>().orthographicSize = Zoom;
            }
        }
    }
}
