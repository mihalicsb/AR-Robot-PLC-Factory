using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Linq;
using TMPro;

namespace Diploma.Factory
{    
    public enum State{WAIT, CREATE_FACTORY,  AR_PLANE_DETECTION, OBJECT_PLACEMENT, PRE_UPDATE, INIT, ERROR, UPDATE_FACTORY, SHOW_ROBOT_DETAILS}
    public class Factory_Program : MonoBehaviour
    {
        [SerializeField] private GameObject panelARPlaneDetection, panelObjectPlacement, panelGUI, panelCreateFactory, panelRobotDetails;
        [SerializeField] private List<GameObject> robotPrefabs;
        [SerializeField] private GameObject factoryFloorPrefab, factoryNode;
        [SerializeField] private GameObject robotStatusInfoPrefab;
        [SerializeField] private Material holoMaterial;

        private GameObject placed_FactroyFloor;
        private List<GameObject> placed_RobotObjects;
        private List<GameObject> placed_RobotStatusInfos;
        public static State state;
        public static Vector3 placeOrigo;
        public static string logging;
        public static string details;
        public static List<Robot> robots;
        private float initialDistance;
        private Vector3 initialScale;
        private RaycastHit hit;
        void Start()
        {   
            
            state = State.INIT;
            placeOrigo = Vector3.zero;
            logging = "";
        }
        void Update()
        {
            switch(state)
            {
                case State.UPDATE_FACTORY: UpdateFactory();break;
                case State.WAIT: break;
                case State.INIT: Init(); break;
                case State.CREATE_FACTORY: CreateFactoryObject(); break;
                case State.AR_PLANE_DETECTION: ARPlaneDetection(); break;
                case State.OBJECT_PLACEMENT:  ObjectPlacement(); break;
                case State.PRE_UPDATE:  PreUpdate(); break;
                case State.SHOW_ROBOT_DETAILS: ShowRobotDetails(); break;
                case State.ERROR: Error(); break;
                default: break;
            }
            print(logging);
        }

        #region State methodes
        private void Init()
        {
            AllPanelDisable();
            logging = "Init";
            robots = new List<Robot>();
            placed_RobotObjects = new List<GameObject>();
            placed_RobotStatusInfos = new List<GameObject>();
            state = State.CREATE_FACTORY;
        }

        private void CreateFactoryObject()
        {
            panelCreateFactory.SetActive(true);
            if(RiptideServiceFactory.state == RiptideServiceFactory.State.STOP)
            {
                // logging = "Create Factory";
                placed_FactroyFloor = Instantiate(factoryFloorPrefab, Vector3.zero, Quaternion.identity, factoryNode.transform);
                placed_FactroyFloor.transform.localScale = new Vector3(robots[0].FactoryWidth, .01f, robots[0].FactoryLength);

                foreach(var robot in robots)
                {
                    foreach (var robotGO in robotPrefabs)
                    {
                        if(robot.Type == robotGO.name)
                        {
                            
                            var robObj = Instantiate(robotGO, new Vector3(robot.Xpos, 0, robot.Ypos), robotGO.transform.localRotation, factoryNode.transform);
                            robObj.name = robot.Guid;
                            robObj.GetComponent<RobotDetail>().enabled = false;
                            //TODO material csere -> virtuálisnak hologrammos
                            if(robot.Virtual == 1){
                                print( $"{robot.Alias} virtuális?: {robot.Virtual}");
                                var meshRenderers = robObj.GetComponentsInChildren<MeshRenderer>();
                                foreach(var meshRenderer in meshRenderers)
                                {
                                    //Minden material-t lecserélünk a modellen. CSAK így működik.
                                    var mats = new Material[meshRenderer.materials.Length];
                                    for(int i = 0; i< mats.Length; i++)
                                    {
                                        mats[i] = holoMaterial;
                                    }
                                    meshRenderer.materials = mats;
                                }
                            }
                            var robStatus = Instantiate(robotStatusInfoPrefab, robObj.transform.position, robotStatusInfoPrefab.transform.localRotation, robObj.transform);
                            robStatus.transform.position += 0.8f * Vector3.up;
                            robStatus.GetComponent<TextMeshPro>().text = $"{robot.Name}\n{robot.Address}";
                            placed_RobotStatusInfos.Add(robStatus);
                            robObj.transform.Rotate(robObj.transform.localRotation.x, -robot.ZOrinetation, robObj.transform.localRotation.z); //azért negatív a forgatás mert a robotstudio fordtva értelmezi a forgatás irányát, valamitn itt az y a függőleges tengely
                            placed_RobotObjects.Add(robObj);
                        }
                    }
                }

                initialScale = Vector3.one * PlayerPrefs.GetFloat(Settings.PREF_SCALE, .3f);
                factoryNode.transform.localScale = initialScale;
                
                if (Application.isEditor){
                    state = State.OBJECT_PLACEMENT;
                    Camera.main.transform.position = new Vector3(0.497000009f,0.0149999997f,-0.889999986f);
                }
                else{
                    state = State.AR_PLANE_DETECTION;
                }
                RiptideServiceFactory.state = RiptideServiceFactory.State.UPDATE;
            }
            
        }

        private void ARPlaneDetection()
        {
            AllPanelDisable();
            panelARPlaneDetection.SetActive(true);
            //Ha talált síkot autómatikusan a detectedbe lépteti az ARPlanePlacement
        }

        private void ObjectPlacement()
        {
            AllPanelDisable();
            panelObjectPlacement.SetActive(true);
            factoryNode.transform.position = placeOrigo; //folyamatosan frissül, ahogy az ujjunkkal mozgatjuk a síkon
            //ARPlaneAccepted-be az accepted gombbal lépünk a _ARButtonManager-ben
            if (Application.isEditor){
                    state = State.UPDATE_FACTORY;
                }
        }

        private void PreUpdate()
        {
            AllPanelDisable();
            panelGUI.SetActive(true);
            foreach(var pro in placed_RobotObjects)
            {
                pro.GetComponent<RobotDetail>().enabled = true;
            }
            state = State.UPDATE_FACTORY;
        }

        private void ShowRobotDetails()
        {
            state = State.WAIT;
            AllPanelDisable();
            panelRobotDetails.SetActive(true);
            panelRobotDetails.GetComponentInChildren<TextMeshProUGUI>().text = details;
        }

        private void UpdateFactory()
        {
            //kétujja nagyítás (pinch scale) - működik, de most nem használom
            if(Input.touchCount == 2)
            {
                var t0 = Input.GetTouch(0);
                var t1 = Input.GetTouch(1);

                if(t0.phase == TouchPhase.Ended || t0.phase == TouchPhase.Canceled ||t1.phase == TouchPhase.Ended || t1.phase == TouchPhase.Canceled)
                {
                    return;
                }

                if(t0.phase == TouchPhase.Began || t1.phase == TouchPhase.Began )
                {
                    initialDistance = Vector2.Distance(t0.position, t1.position);
                    initialScale = factoryNode.transform.localScale;
                }
                else{
                    var currentDistance = Vector2.Distance(t0.position, t1.position);
                    //véletlen, nagyon kicsi mozgatások elkerüléséhez
                    if(Mathf.Approximately(initialDistance, 0))
                    {
                        return;
                    }

                    var factor = currentDistance /initialDistance;
                    factoryNode.transform.localScale = initialScale * factor;
                    logging = $"Méretezés: {factoryNode.transform.localScale.x}";
                    if(factoryNode.transform.localScale.x > 1)  //elég csak egy irányt nézni, mert  mindent rányba egyenletes a méretezés
                    {
                        logging = $"Méretezés: {factoryNode.transform.localScale.x}";
                        factoryNode.transform.localScale = Vector3.one;
                    }
                }
            }
            //TODO ide jönne az IK folyamatokhoz a TASK-ok programpointerén lévő Robtargetek-kel frissíteni a robotkarok mozgásátfor
            foreach(var statusSign in placed_RobotStatusInfos)
            {
                //objPos - (targetPos - obPos) => 2*objPos - targetPos -> ezzel technikailag az ellenkező irányba néz az objektum
                statusSign.transform.LookAt(2 * statusSign.transform.position -  Camera.main.transform.position); //simán a kamerára nézés, nem volt jó, mert a szövegek háttal voltak
                
            }
        }

        private void Error()
        {
            logging = "Hiba: "; //hibák szövegének hozzáfűzése és hiba állapotba léptetés.
        }

        #endregion

        private void OnGUI()
        {
            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            Rect rectLog = new Rect(20, 40, w, h *2 /100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
            GUI.Label(rectLog, logging + "\n", style);
        }

        private void AllPanelDisable()
        {
            panelARPlaneDetection.SetActive(false);
            panelCreateFactory.SetActive(false);
            panelGUI.SetActive(false);
            panelObjectPlacement.SetActive(false);
            panelRobotDetails.SetActive(false);
        }
    }
}
