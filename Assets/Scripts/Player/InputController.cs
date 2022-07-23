using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


    public class InputController : Singleton<InputController>
    {
        public delegate void InputControllerFirstInputAction();
        public static event /*InputController.*/InputControllerFirstInputAction IsFirstInput;

        public delegate void InputControllerCharacterMove(float targetPosX, float rightPosX, float leftPosX);
        public static event /*InputController.*/InputControllerCharacterMove CharacterMove;

        [SerializeField][Range(0, 5)] private float inputSpeed = 2f;
        [SerializeField] private float leftPosX = -3f;
        [SerializeField] private float rightPosX = 3f;

        private float targetPosX;
        private float distanceX;
        private float screenRateWithPose;

        public bool firstInput = false;

        private Vector3 prevPos;
        private Vector3 currentPos;

        //========================================================================

        private void Awake()
        {
            //ekran oranlaması hesaplandı
            ScreenRateCalculator();
        }

        //========================================================================

        private void OnEnable()
        {

            /*InputController.Instance.*/
            IsFirstInput += OnIsFirstInput;
        }

        private void OnDisable()
        {

            /*InputController.Instance.*/
            IsFirstInput -= OnIsFirstInput;
        }

        //========================================================================

        /// <summary>
        /// her oyun döngüsünde ilk input sıfırlanır
        /// </summary>
        private void Start()
        {
            firstInput = false;
        }

        //========================================================================

        /// <summary>
        /// ilk input verildigi bilgisini yollar
        /// </summary>
        private void OnIsFirstInput()
        {

        }

        //========================================================================

        void Update()
        {
#if UNITY_EDITOR
            TEST();
#endif

            Touch();

            // ilk input tıklandıysa karakter hareketini gercekler
            if (firstInput)
            {
                CharacterMove?.Invoke(targetPosX, rightPosX, leftPosX);
            }
        }

        //========================================================================

#if UNITY_EDITOR
        /// <summary>
        /// sağ click ile zamanı durdurarak test yapmamızı sağlar
        /// </summary>
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
        //========================================================================

        /// <summary>
        /// Inputu verilmesini dinler
        /// </summary>
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
                    distanceX = Mathf.Clamp(currentPos.x - prevPos.x, -1, 1);
                    targetPosX = distanceX;
                    prevPos = currentPos;

                Debug.Log(" currentPos " + currentPos + " distanceX " + distanceX + " targetPosx" + targetPosX + " ");
                }
                if (Input.GetMouseButtonUp(0))
                {
                    targetPosX = 0;
                }
            }
        }

        //========================================================================

        /// <summary>
        /// input degisimini alıp anlık olarak ekranın genisligini yolun sag ve sol alanına göre oranlayarak horizontal degisimini hesaplar
        /// </summary>
        private float ScreenRateWithPoseCalculateEditor(float distanceX)
        {
            return screenRateWithPose * distanceX;
        }

        //========================================================================

        /// <summary>
        /// ekranın genisligini yolun sag ve sol alanına göre oranlayarak horizontal degisimini hesaplar
        /// </summary>
        private void ScreenRateCalculator()
        {
            screenRateWithPose = ((rightPosX - leftPosX) / (Screen.width /* -0 */)) * inputSpeed;
        }


        public bool GetFirstInput()
        {
            return firstInput;
        }
    }


#region "Example Using"
/*public class PlayerController : MonoBehaviour
{
    private void OnEnable()
    {
        InputController.CharacterMove += OnCharacterMove;
    }

    private void OnDisable()
    {
        InputController.CharacterMove -= OnCharacterMove;

    }

    private void OnCharacterMove(float targetPosX, float rightPosX, float leftPosX)
    {
        Vector3 currentPosition = transform.position;

        currentPosition.x += targetPosX;

        transform.position = currentPosition;
    }
}*/
#endregion