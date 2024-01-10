using UnityEngine;

namespace Diploma.Factory
{
    public class _ARButtonManager : MonoBehaviour
{
    [SerializeField] ARPlanePlacement arPlanePlacement;

    public void Click_PlaceObjects()
    {
        var temp = Factory_Program.placeOrigo;
        Factory_Program.state = State.PRE_UPDATE;
        arPlanePlacement.TogglePlaneDetection();
        Factory_Program.placeOrigo = temp;
    }

    public void Click_CloseDeatilsPanel()
    {
        Factory_Program.state = State.PRE_UPDATE;
    }
}   
}

