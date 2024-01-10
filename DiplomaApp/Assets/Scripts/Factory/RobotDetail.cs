using UnityEngine;
using System.Linq;

namespace Diploma.Factory
{
    public class RobotDetail : MonoBehaviour
    {
            private RaycastHit hit;
            private GameObject panelRobotDetail;
            private bool pushed;
            void Start()
            {
                pushed = false;
            }

            void FixedUpdate()
            {
                if (Input.GetMouseButton(0)){
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10))
                    {
                        var selection = hit.transform;
                        print("selection name: " + selection.name);
                        if(selection.name == transform.name)
                        {
                            pushed = true;
                        }
                    
                    }
                }

                if(pushed)
                {
                    pushed = false;
                    Robot thisRobot = Factory_Program.robots.Where(r => r.Guid == transform.name).FirstOrDefault();
                    Factory_Program.details = thisRobot.Details();
                    Factory_Program.state = State.SHOW_ROBOT_DETAILS;
                }
            }
    }
}
