using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDialogs;
public class TestDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UDialogManager.OnHide += () =>
        {
            Debug.Log("HIDE!!!");
        };

        UDialogManager.OnTimeOut += (string did) =>
        {
            Debug.Log("TIMEOUT ! " + did);
        };

        string id = "TEST";
        
        UDialogManager.DisplayDialogWindow(id, "Tienes tiempo?", DialogType.Warning, DialogButtons.YesNo, new ButtonOptions("Si", (UDialogMessage mm) =>
   {

       string xid = "TEST2";
       UDialogManager.DisplayDialogWindow(xid, "Hola !!!", DialogType.Info, DialogButtons.None, 5f, new ButtonOptions("Ok", null));

       mm.Hide();
   }), new ButtonOptions("No", (UDialogMessage mm) => mm.Hide()));

    }
}
