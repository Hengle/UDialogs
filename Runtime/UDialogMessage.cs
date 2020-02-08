using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UDialogs
{
    public class UDialogMessage : MonoBehaviour
    {
        [Header("General Settings")]
        [SerializeField]
        private float LoadingRotationSpeed = 8f;
        [SerializeField]
        private TextMeshProUGUI DisplayText = default;
        [SerializeField]
        private GameObject ButtonContainer = default;
        [SerializeField]
        private Image DisplayIcon = default;
        [SerializeField]
        private string DefaultOkText = default;
        [SerializeField]
        private string DefaultNoText = default;
        [SerializeField]
        private string DefaultCancelText = default;
        [SerializeField, Header("Buttons")]
        private Button OkButton = default;
        [SerializeField]
        private UnityEvent OkButtonClick = default;
        [SerializeField,Space(5)]
        private Button NoButton = default;
        [SerializeField]
        private UnityEvent NoButtonClick = default;
        [SerializeField,Space(5)]
        private Button CancelButton = default;
        [SerializeField]
        private UnityEvent CancelButtonClick = default;

        public string Id { get; private set; } = default;

        public void Init(string id, Sprite Icon, bool animate ,DialogButtons buttons, string text, ButtonOptions OkOptions,
            ButtonOptions NoOptions, ButtonOptions CancelOptions , float timeout)
        {
            Id = id;

            DisplayText.text = text;

            DisplayIcon.gameObject.SetActive(Icon != null);
            
            DisplayIcon.transform.rotation = Quaternion.identity;
            
            DisplayIcon.sprite = Icon;

            if (animate)
                StartCoroutine(I_RotateIcon());

            if (timeout > 0f)
                StartCoroutine(I_AutoHide(timeout));

            if (buttons == DialogButtons.None)
            {
                ActivateButton(OkButton, false);
                ActivateButton(NoButton, false);
                ActivateButton(CancelButton, false);
                ButtonContainer.SetActive(false);
                return;
            }

            ButtonContainer.SetActive(true);

            if (buttons == DialogButtons.Ok || buttons == DialogButtons.YesNo)
            {
                OkButton.onClick.AddListener(() =>
                {
                    if (OkOptions != null && OkOptions.action != null)
                        OkOptions.action.Invoke(this);

                    OkButtonClick.Invoke();
                });

                if (OkOptions != null)
                    UpdateText(OkButton, OkOptions.displayText);
                else
                    UpdateText(NoButton, DefaultOkText);

                ActivateButton(OkButton, true);
            }else
            {
                ActivateButton(OkButton, false);
            }

            if (buttons == DialogButtons.YesNo || buttons == DialogButtons.YesNoCancel)
            {
                NoButton.onClick.AddListener(() =>
                {
                    if (NoOptions != null && NoOptions.action != null)
                        NoOptions.action.Invoke(this);

                    NoButtonClick.Invoke();
                });

                if (NoOptions != null)
                    UpdateText(NoButton, NoOptions.displayText);
                else
                    UpdateText(NoButton, DefaultNoText);

                ActivateButton(NoButton, true);
            }else
            {
                ActivateButton(NoButton, false);
            }

            if (buttons == DialogButtons.YesNoCancel)
            {
                CancelButton.onClick.AddListener(() =>
                {
                    if (CancelOptions != null && CancelOptions.action != null)
                        CancelOptions.action.Invoke(this);
                    
                    CancelButtonClick.Invoke();
                });

                if (CancelOptions != null)
                    UpdateText(CancelButton, CancelOptions.displayText);
                else
                    UpdateText(CancelButton, DefaultCancelText);

                ActivateButton(CancelButton, true);
            }else
            {
                ActivateButton(CancelButton, false);
            }
        }

        private void ActivateButton(Button btn, bool state)
        {
            if (btn.gameObject.activeInHierarchy != state)
                btn.gameObject.SetActive(state);
        }

        protected virtual void UpdateText(Button btn, string text)
        {
            TextMeshProUGUI ugui = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (ugui != null)
                ugui.text = text;
        }

        public void Hide()
        {
            UDialogManager.HideDialogWindow(Id);
        }

        internal void Kill()
        {
            Debug.Log("KILL");
            Destroy(gameObject,Time.deltaTime);
            StopAllCoroutines();
        }

        private IEnumerator I_AutoHide(float t)
        {
            yield return new WaitForSeconds(t);
            Hide();
            UDialogManager.NewTimeOut(Id);
        }

        private IEnumerator I_RotateIcon()
        {
            RectTransform rt = DisplayIcon.GetComponent<RectTransform>();
            while(gameObject.activeInHierarchy)
            {
                rt.Rotate( Vector3.forward * LoadingRotationSpeed * Time.deltaTime , Space.Self);
                yield return new WaitForEndOfFrame();
            }
            rt.rotation = Quaternion.identity;
            yield return 0;
        }

    }
}