# UDialogs

Make Dialog Modals with ease!

![Preview](preview.png)

Display via script in just one command

```c#

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDialogs;
public class Example : MonoBehaviour
{
    void Start()
    {
        UDialogManager.DisplayDialogWindow("Id" , "My amazing text" , DialogType.Info , DialogButtons.Ok , new ButtonOptions("Ok" , null));
    }
}

```

Full example with callbacks and options:
```c#

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDialogs;
public class TestDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Register callbacks
        UDialogManager.OnHide += () =>
        {
            Debug.Log("HIDE!!!");
        };

        UDialogManager.OnTimeOut += (string did) =>
        {
            Debug.Log("TIMEOUT ! " + did);
        };

        // Set id
        string id = "TEST";
        // Display a main dialog
        UDialogManager.DisplayDialogWindow(id, "Do you have time?", DialogType.Warning, DialogButtons.YesNo, new ButtonOptions("Yes", (UDialogMessage mm) => // Yes click callback
   {
        // Display secondary dialog       
       string xid = "TEST2";
       UDialogManager.DisplayDialogWindow(xid, "Hola !!!", DialogType.Info, DialogButtons.None, 5f, new ButtonOptions("Ok", null));

       // Hide main dialog after secondary dialog is diaplayed
       mm.Hide();
   }), new ButtonOptions("No", (UDialogMessage mm) => mm.Hide()));

    }
}


```