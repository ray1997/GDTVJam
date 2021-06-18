using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastReceiver : MonoBehaviour
{
    public GameObject ToastMessagePrototype;
    public Transform MessageHolderParent;

    private delegate void ShowMessage(string message);
    private static event ShowMessage OnRequestShowMessage;

    public static void ShowToastMessage(string message) =>
        OnRequestShowMessage?.Invoke(message);

    private void OnEnable()
    {
        if (MessageCooldown is null)
            MessageCooldown = new Dictionary<string, float>();
        OnRequestShowMessage += ShowToastMessageInUI;
    }

    private void OnDisable()
    {
        OnRequestShowMessage -= ShowToastMessageInUI;
    }

    private void ShowToastMessageInUI(string message)
    {
        //Message cooldown system
        if (ShouldCooldownAndStopDisplay(message))
        {
            return;
        }
        else
        {
            Debug.Log($"Try showing toast message: \r\n{message}");
            var msg = Instantiate(ToastMessagePrototype, MessageHolderParent, false);
            var toast = msg.GetComponent<MessageToast>();
            toast.ShowToast(message);
        }
    }

    Dictionary<string, float> MessageCooldown;
    public float MessageCooldownDuration = 10f;
    public bool ShouldCooldownAndStopDisplay(string message)
    {
        Debug.Log($"Message cooldown has message? {MessageCooldown.ContainsKey(message)}");
        if (!MessageCooldown.ContainsKey(message))
        {
            //Add to cooldown
            MessageCooldown.Add(message, Time.realtimeSinceStartup + MessageCooldownDuration);
            return false;
        }
        else
        {
            Debug.Log($"Message can show at: {MessageCooldown[message]}\r\n" +
                $"Currently it's {Time.realtimeSinceStartup}");
            if (MessageCooldown[message] < Time.realtimeSinceStartup)
            {
                //Can show message, cooldown over
                MessageCooldown[message] = Time.realtimeSinceStartup + MessageCooldownDuration;
                return false;
            }
            else
            {
                Debug.Log($"Message still on cooldown for: {MessageCooldown[message] - Time.realtimeSinceStartup} seconds");
                //Can't show message
                return true;
            }
        }
    }
}
