using System;
using UnityEngine;
using System.Linq;

namespace Diploma.Device
{
    public class ButtonControl : MonoBehaviour
    {
        private RaycastHit hit;
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
                var plcId = DeviceRead_Program.qrPlcId;
                var inputID = Int32.Parse(transform.parent.name.Split('_')[1]);
                var value = DeviceRead_Program.plcs.Where(p => p.Id == 2).Select(i => i.IOs.Where(i => i.Id == 3).Select(i=> i.Value).First()).First();
                // var plcId = 2;
                // var inputID = Int32.Parse(transform.parent.name.Split('_')[1]);
                // // print("input id: " + inputID);
                // // var value = (from plc in TestNamespace.ComTeszt.plcs where plc.Id == plcId 
                // //                 select new{
                // //                         value = (from io in plc.IOs where io.Id == inputID select io.Value).First()
                // //                 }
                // //             ).First().value;
                // var value = TestNamespace.ComTeszt.plcs.Where(p => p.Id == 2).Select(i => i.IOs.Where(i => i.Id == 3).Select(i=> i.Value).First()).First();
                print ("current value: " + value);

                // TestNamespace.ComTeszt.Singleton.Client.Send(
                RiptideServiceDevice.Singleton.Client.Send(
                    Riptide.Message.Create(Riptide.MessageSendMode.Reliable, MessageId.POST_PLC_DATA)
                    .AddInt(plcId) //PLC id < QR kódbóp jön
                    .AddInt(inputID)  //ennek az objektumnak az ősének a nevéből jön -> pl.: button_3
                    .AddBool(!Convert.ToBoolean(value)));   //linq lekéréssel a plc id és io id-ból
            }
            // print(transform.parent.name + " gomb állapota: " + pushed);
        }

        public void Toggle(int state) 
        {
            var mat = GetComponent<Renderer>().material;
            if(state == 0)
            {
                transform.localPosition  = Vector3.down * .16f;
                mat.color = Color.black;
            }
            else{
                transform.localPosition  = Vector3.zero;
                mat.color = Color.white;
            }
        }
    }
}
