using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


    public class InputController : Singleton<InputController>
    {
        public delegate void InputControllerFirstInputAction();
        public static event InputControllerFirstInputAction IsFirstInput;

        public delegate void InputControllerCharacterMove(float targetPosX, float rightPosX, float leftPosX);
        public static event InputControllerCharacterMove CharacterMove;

        [SerializeField][Range(0, 5)] private float inputSpeed = 2f;
        [SerializeField] private float leftPosX = -3f;
        [SerializeField] private float rightPosX = 3f;

        private float targetPosX;
        private float distanceX;
        private float screenRateWithPose;

        public bool firstInput = false;

        private Vector3 prevPos;
        private Vector3 currentPos;


        private void Awake()
        {
            ScreenRateCalculator();
        }

        private void OnEnable()
        {
            IsFirstInput += OnIsFirstInput;
        }

        private void OnDisable()
        {
            IsFirstInput -= OnIsFirstInput;
        }
        private void Start()
        {
            firstInput = false;
        }

        private void OnIsFirstInput()
        {
            UIManager.Instance.TapToStart();
        }

        void Update()
        {
#if UNITY_EDITOR
            TEST();
#endif

            Touch();
            if (firstInput)
            {
                CharacterMove?.Invoke(targetPosX, rightPosX, leftPosX);
            }
        }

#if UNITY_EDITOR
        bool isPause = true;
        private void TEST()
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (isPause)
                {
                    Time.timeScale = 0;
                }
                else
                {
                    Time.timeScale = 1;
                }

                isPause = !isPause;
            }
        }

#endif
      
        private void Touch()
        {
            if (Input.touchCount > 1)
            {
                return;
            }

            if (!firstInput)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    IsFirstInput?.Invoke();
                    firstInput = true;
                }
            }

            if (firstInput)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    prevPos = Input.mousePosition;
                }
                if (Input.GetMouseButton(0))
                {
                    currentPos = Input.mousePosition;
                distanceX = currentPos.x - prevPos.x;
                targetPosX = ScreenRateWithPoseCalculateEditor(distanceX);
                prevPos = currentPos;

                }
                if (Input.GetMouseButtonUp(0))
                {
                    targetPosX = 0;
                }
            }
        }

        private float ScreenRateWithPoseCalculateEditor(float distanceX)
        {
            return screenRateWithPose * distanceX;
        }

        private void ScreenRateCalculator()
        {
            screenRateWithPose = ((rightPosX - leftPosX) / (Screen.width /* -0 */)) * inputSpeed;
        }


        public bool GetFirstInput()
        {
            return firstInput;
        }
    }

