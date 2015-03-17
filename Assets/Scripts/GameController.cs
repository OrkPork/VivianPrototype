using UnityEngine;
using System.Collections.Generic;
using Entitas;
using System.IO;
using System;
//using System.Diagnostics;

public class GameController : MonoBehaviour {
    Pool _repo;
    //JSONTemplates
    int mapIndex = 0;
    public List<string> mapTitles = new List<string> { "TestMap" };
    IExecuteSystem[] _systems;
    //Only datapath works in unity editor
    FileInfo targetData;
    string fileDir;
    string targetFile;
    //StreamReader reader;
    //retrieve peristent data stuff
    //new WWW("file://" + Application.persistentDataPath + "/myStuff.txt");

    void Start() {
        _repo = new Pool(ComponentIds.TotalComponents);

        //fileDir = Application.persistentDataPath; //comment out for testing
        fileDir = Application.dataPath;//comment out for builds


        targetFile = fileDir + "/GameData/myProfiles.json";

        Debug.Log(targetFile);
        if (!File.Exists(targetFile))
        {
            Debug.Log("No profile detected, creating one");
            createProfile();
        }
        else
        {
            Debug.Log("Found profile, loading it");
            createProfile();
        }
        createSystems();
        createTestMap();
        createPlayer();
        createOpponents();
    }

    void createSystems() {
        //also need a pool for game objects
        //System order
        /*
             * Input //base input menus and the such
             *
             * Charmovement input and Battle Input 
             * 
             * Apply Acceleration/movement modifiers
             * 
             * Handle all movements.
             * 
             * populate new objects
             * 
             * handle collisions/contacts of objects
             * 
             * 
             * render objects
             * 
             * destroy objects
             * 
             * de-render objects    
             * */

        //Sytems needed
        /*
         * world system
         * 
         * Contact
         * 
         * Populate UI
         * 
         * Create behavior components for UI (shortcuts)
         * 
         * Camera Flavor
         * 
         * Dialogue
         * 
         * Populate Battle
         * 
         * Run function, run animation
         * 
         * calculate damage (restore controls)
         * */

        _systems = new [] {
            
            _repo.CreateSystem<InputSystem>(),
            
            //_repo.CreateSystem<AccelerateSystem>(),
            _repo.CreateSystem<MoveSystem>(),
            //replace/update with:
            //--load to pool
            //--instantiate from pool and view
            _repo.CreateSystem<LoadAndViewSystem>(),
            //bumpsystem goes here
            //instead of pushing a monobehaviour script
            //
            _repo.CreateSystem<RenderPositionSystem>(),

            _repo.CreateSystem<DestroySystem>(),
            _repo.CreateSystem<RenderDespawnSystem>()
        };
    }

    void createProfile()
    {
        
        List<string> cNames;
        List<List<KeyCode>> cBttns;
        List<List<string>> cAxes;
        List<List<bool>> cUP ;
        List<List<bool>> cHeld;
        List<List<bool>> cDown;
        List<List<float>> cAxesV;
        List<bool> cBttnAxis;
        targetFile = fileDir + "/GameData/myDefaults.json";
        if (File.Exists(targetFile))
        {
            var str = File.ReadAllText(targetFile);
            JSONObject jData = new JSONObject(str);
            var myLists = jData.list;
            int lnCLM;
            int lnROW;
            var e = _repo.CreateEntity();

            jData.GetField("DefaultInputs", delegate(JSONObject obj_1)
            {
                //...I know where max alt commands always is so I cheated, direct reference
                //Set the rows (possible alternate buttons for both axes and bttns)
                lnROW = (int)obj_1.list[0].n;
                obj_1.GetField("myCommands", delegate(JSONObject arr_1)
                {
                    //set capacity on outer lists
                    lnCLM = arr_1.list.Count;
                    cNames = new List<string>(lnCLM);// setting here
                    cBttns = new List<List<KeyCode>>(lnROW);// setting here
                    cAxes = new List<List<string>>(lnROW);//setting here
                    cUP = new List<List<bool>>(lnROW);
                    cHeld = new List<List<bool>>(lnROW);
                    cDown = new List<List<bool>>(lnROW);
                    cAxesV = new List<List<float>>(lnROW);
                    cBttnAxis = new List<bool>(lnCLM);//setting here
                    //set capactity on inner lists
                    //set defaults
                    for (int k = 0; k < lnROW; k++)
                    {
                        cBttns.Add(new List<KeyCode>(lnCLM));
                        cAxes.Add(new List<string>(lnCLM)); 
                        cUP.Add(new List<bool>(lnCLM)); 
                        cHeld.Add(new List<bool>(lnCLM)); 
                        cDown.Add(new List<bool>(lnCLM));
                        cAxesV.Add(new List<float>(lnCLM));
                        //populate lists
                        for (int w = 0; w < lnCLM; w++)
                        {
                            //cNames
                            ////////////////
                            cBttns[k].Add(KeyCode.None);
                            cAxes[k].Add("");
                            //may not need to populate the input state trackers
                            cUP[k].Add(false);
                            cHeld[k].Add(false);
                            cDown[k].Add(false);
                            cAxesV[k].Add(0f);
                        }

                    }
                        
                        //Debug.Log("Number of actions" + lnCLM);

                    for (int p = 0; p < arr_1.list.Count; p++ )
                    {
                        JSONObject obj = arr_1.list[p];

                        //Again I know the structure of the default inputs so I cheated
                        //probably should add some error catching!

                        //Debug.Log("Name Field: " + obj.list[0].str);
                        cNames.Add(obj.list[0].str);
                        //Debug.Log("Button as axis:" + obj.list[1].b);
                        cBttnAxis.Add(obj.list[1].b);

                        //maybe I don't need to use getfield, buttons should always be found
                        //well buttons and axes are the easiest to mess up so error catching is nice
                        obj.GetField("buttons", delegate(JSONObject bttnList)
                        {
                            for (int i = 0; i < bttnList.list.Count; i++)
                            {
                                //ToString() doesn't evaluate properly.
                                var bttnName = bttnList.list[i].str;
                                try
                                {
                                    KeyCode keyBttn = (KeyCode)System.Enum.Parse(typeof(KeyCode), bttnName);
                                    //Debug.Log("KeyCode: " + keyBttn);
                                    cBttns[i][p] = keyBttn;
                                }
                                catch
                                {
                                    Debug.Log("Bad string failed to make KeyCode: " + bttnName);
                                }



                            }
                        }, delegate(string dumbname)
                        {
                            Debug.Log("buttons not found");
                        });
                        /////////////////////////////////////////////////

                        obj.GetField("axes", delegate(JSONObject axesList)
                        {
                            for (int z = 0; z < axesList.list.Count; z++)
                            {
                                cAxes[z][p] = axesList.list[z].str;
                                //Debug.Log(axesList.list[z].str);
                            }
                        }, delegate(string dumbname2)
                        {
                            Debug.Log("axes not found");
                        });
                    }


                    e.AddMyInputs(cNames, cBttns, cAxes, cUP, cHeld, cDown, cAxesV, cBttnAxis);
                });
            }, delegate(string name)
            {	
                Debug.Log("DefaultInputs not found");
            });

        }
        else
        {
            Debug.Log("myDefaults.json not found in path specified: " + targetFile);
        }

        Dictionary<string, object> testdict = new Dictionary<string, object>();


        
    }

    void createTestMap()
    {
        var e = _repo.CreateEntity();
        e.AddResource(mapTitles[mapIndex]);
        e.AddPosition(new Vector3(0, 0, 0));
    }

    void createPlayer() {
        var e = _repo.CreateEntity();
        //push scripts have to go before resource
        e.AddPushScripts(new List<string> { "ContactFights" });
        e.AddResource("Player");
        e.AddPosition(new Vector3 (0, 0.5f, 0));
        e.AddCharMove(new Vector3 (8f, 0, 8f));
        //Debug.Log()
        //e.isAcceleratable = true;
    }

    void createOpponents() {
        const string resourceName = "Enemy";
        for (int i = 0; i < 6; i++) {
            var e = _repo.CreateEntity();
            e.AddResource(resourceName);
            if (i < 1)
            {
                e.AddPosition(new Vector3(2.5f, 0.5f, 4.7f));
            } else if (i < 2)
            {
                e.AddPosition(new Vector3(-13f, 0.5f, 7.64f));
            } else if (i < 3)
            {
               e.AddPosition(new Vector3(-5.6f, 0.5f, 3.9f));
            } else if (i < 4)
            {
               e.AddPosition(new Vector3 (6.6f, 0.5f, 4.7f));
            } else if (i < 5)
            {
                e.AddPosition(new Vector3 (2.5f, 0.5f, 4.7f));
            } else
            {
               e.AddPosition(new Vector3 (2.5f, 0.5f, 4.7f));
            }

        }
    }

    void Update() {
        foreach (var system in _systems) {
            system.Execute();
        }
    }
}
