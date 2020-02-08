using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace UDialogs
{
    [DefaultExecutionOrder(+50)]
    public class UDialogManager : MonoBehaviour
    {
        public delegate void _onDialogWindowTimeout(string dialogid);

        public static event _onDialogWindowTimeout OnTimeOut;

        public delegate void _onDialogWindowHide();

        public static event _onDialogWindowHide OnHide;

        private static UDialogManager _instance = null;

        private static UDialogManager Instance
        {
            get
            {
                if (!_instance) _instance = FindObjectOfType<UDialogManager>();

                return _instance;
            }
        }

        [Header("General Settings")]
        [SerializeField]
        private Image Background = default;
        [SerializeField]
        private GameObject WindowsContainer = default;
        [SerializeField]
        private GameObject DialogWindowBase = default;
        [Header("Sprite Settings")]
        [SerializeField]
        private Sprite InfoIcon = default;
        [SerializeField]
        private Sprite LoadingIcon = default;
        [SerializeField]
        private Sprite WarningIcon = default;
        [SerializeField]
        private Sprite ErrorIcon = default;

        [SerializeField,Space(5)]
        private UnityEvent OnHideDialog = default;

        private Dictionary<string, UDialogMessage> db = new Dictionary<string, UDialogMessage>();

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this);
                return;
            }

            _instance = this;

            DontDestroyOnLoad(gameObject);
        }

        public static void HideDialogWindow(string id)
        {
            UDialogManager manager = Instance;

            if (manager.db.TryGetValue(id , out UDialogMessage msg))
            {
                msg.Kill();

                manager.db.Remove(id);

                if (OnHide != null)
                    OnHide.Invoke();
                manager.OnHideDialog.Invoke();
            }else
            {
                Debug.Log("KEY NOT FOUND" + id);
            }

            if (manager.db.Keys.Count == 0)
                manager.Background.gameObject.SetActive(false);
        }

        internal static void NewTimeOut(string id)
        {
            if (OnTimeOut != null)
                OnTimeOut.Invoke(id);
        }

        internal static void DisplayDialogInternal(string id, string text, ButtonOptions OkOptions = null, ButtonOptions NoOptions = null,
            ButtonOptions CancelOptions = null, DialogType type = DialogType.Info
            ,DialogButtons buttons = DialogButtons.None, float TimeOut = 0f)
        {
            UDialogManager manager = Instance;

            if (manager.db.ContainsKey(id)) return;

            UDialogMessage msg = Instantiate(manager.DialogWindowBase, manager.WindowsContainer.transform).
                GetComponent<UDialogMessage>();

            Sprite _icon = null;

            switch(type)
            {
                case DialogType.Error:
                    _icon = manager.ErrorIcon;
                    break;
                case DialogType.Info:
                    _icon = manager.InfoIcon;
                    break;
                case DialogType.Loading:
                    _icon = manager.LoadingIcon;
                    break;
                case DialogType.Warning:
                    _icon = manager.WarningIcon;
                    break;
            }

            msg.Init(id, _icon, type == DialogType.Loading , buttons, text, OkOptions, NoOptions, CancelOptions, TimeOut);

            manager.db.Add(id, msg);

            manager.Background.gameObject.SetActive(true);
        }

        public static void DisplayDialogWindow(string id , string text , DialogType type , DialogButtons buttons , ButtonOptions OkOptions = null 
            , ButtonOptions NoOptions = null , ButtonOptions CancelOptions = null  )
        {
            DisplayDialogInternal(id, text, OkOptions, NoOptions, CancelOptions, type, buttons);
        }

        public static void DisplayDialogWindow(string id, string text, DialogType type, DialogButtons buttons , float timeout, ButtonOptions OkOptions = null
            , ButtonOptions NoOptions = null, ButtonOptions CancelOptions = null)
        {
            DisplayDialogInternal(id, text, OkOptions, NoOptions, CancelOptions, type, buttons, timeout);
        }

    }
}