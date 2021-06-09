using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorSwitchLight : MonoBehaviour
{
    MeshRenderer buttonRender;

    private void Awake()
    {
        buttonRender = GetComponent<MeshRenderer>();
    }

    LightStatus _status;
    public LightStatus ButtonLightStatus
    {
        get
        {
            if (!GameManager.Instance.GlobalElectricityStatus)
                return LightStatus.Off;
            return _status;
        }
        set => _status = value;
    }
    public Floor ButtonLocation;
    public ButtonColor ButtonColor;

    bool IsUp;
    bool Elevating;

    private void OnEnable()
    {
        ElevatorControl.ElevatorGoingUpward += GoUp;
        ElevatorControl.ElevatorGoingDownward += GoDown;
        ElevatorControl.ElevatorReachDestination += Finished;
    }

    private void Finished()
    {
        Elevating = false;
    }

    private void OnDisable()
    {
        ElevatorControl.ElevatorGoingUpward -= GoUp;
        ElevatorControl.ElevatorGoingDownward -= GoDown;
        ElevatorControl.ElevatorReachDestination -= Finished;
    }

    private void Update()
    {
        //Button coloring
        if (ButtonColor == ButtonColor.Red)
            buttonRender.material = ElevatorControl.Instance.RedLight[(int)ButtonLightStatus];
        else
            buttonRender.material = ElevatorControl.Instance.GreenLight[(int)ButtonLightStatus];
        //Status update
        CurrentTimer -= Time.smoothDeltaTime;
        if (CurrentTimer < 0)
        {
            CurrentTimer = BlinkDuration;
            if (Elevating)
            {
                if (IsUp)//Going up
                {
                    //Blinking green of 2nd floor
                    if ((ButtonLocation == Floor.Second && ButtonColor == ButtonColor.Green) ||
                        (ButtonLocation == Floor.First && ButtonColor == ButtonColor.Red))
                    {
                        if (ButtonLightStatus == LightStatus.Dim)
                            ButtonLightStatus++;
                        else if (ButtonLightStatus == LightStatus.Blink)
                            ButtonLightStatus--;
                    }
                    else
                        ButtonLightStatus = LightStatus.Dim;
                }
                else
                {
                    //Blinking green of 1nd floor
                    if ((ButtonLocation == Floor.First && ButtonColor == ButtonColor.Green) ||
                        (ButtonLocation == Floor.Second && ButtonColor == ButtonColor.Red))
                    {
                        if (ButtonLightStatus == LightStatus.Dim)
                            ButtonLightStatus++;
                        else if (ButtonLightStatus == LightStatus.Blink)
                            ButtonLightStatus--;
                    }
                    else
                        ButtonLightStatus = LightStatus.Dim;
                }
            }
            else
            {
                //Bright only on current floor
                //Set to bright if floor match
                if ((ButtonLocation == ElevatorControl.Instance.CurrentFloor && ButtonColor == ButtonColor.Green) ||
                    (ButtonLocation != ElevatorControl.Instance.CurrentFloor && ButtonColor == ButtonColor.Red))
                {
                    ButtonLightStatus = LightStatus.Blink;
                }
                else
                {
                    ButtonLightStatus = LightStatus.Dim;
                }
            }
        }
    }
    public float BlinkDuration = 0.25f;
    public float CurrentTimer;

    private void GoDown()
    {
        Elevating = true;
        IsUp = false;
    }

    private void GoUp()
    {
        Elevating = true;
        IsUp = true;
    }

}

public enum ButtonColor
{
    Red,
    Green
}

public enum LightStatus
{
    Off,
    Dim,
    Blink
}