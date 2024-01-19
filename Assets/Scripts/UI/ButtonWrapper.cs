using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

// So we can pass custom args from a button click.
public class ButtonWrapper: MonoBehaviour
{
    public Button buttonToWrap;
    public Dictionary<string, object> args = new Dictionary<string, object>();

    public delegate void onClickEvent(Dictionary<string, object> arguments);
    public onClickEvent onWrappedClickEvent;

    public Text buttonText;

    protected void Start()
    {
        buttonToWrap.onClick.AddListener(onButtonClick);
    }

    private void onButtonClick()
    {
         if (onWrappedClickEvent != null)
        {
            onWrappedClickEvent(args);
        }
    }
}