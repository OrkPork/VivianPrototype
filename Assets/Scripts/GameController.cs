﻿using UnityEngine;
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
    string fromLocalDisk;
    string fromResources;
    //StreamReader reader;
    //retrieve peristent data stuff
    //new WWW("file://" + Application.persistentDataPath + "/myStuff.txt");

    //To-do:
    // enemy data entity
    // -enemies will need a dictionary containing references to their stats...so they can be retrieved by string
    // --because I want to modify battlebehaviors and stats
    // AI scripts
    // Initiative scripting
    // -players can be surprised by Initiative switches (for tutorial)
    // event scripting
    // getting entity triggers (<-I'm using monorefs for this, didn't work like I wanted)
    // load defaults from resources
    // parse level data
    // parse encounters
    // input manager UI
    // -users can test shortcuts and inputs on a test map
    // handle menus (how to load menu targets [new game] -> first level)
    // -scrolling through menus by controller
    // WE MAY NEED TO WRITE OUR OWN COLLISION SYSTEM
    // Relying on resource tags may prove annoying, well more annoying than it already is
    // Perform X action over-time component
    // pausing
    // disabling/enabling systems...

    //TRY
    //LOAD PROFILEDATA
    //LOAD MAP
    //----------
    //LOAD PLAYER
    //LOAD ENEMIES
    //LOAD NPCS
    //LOAD MAPDATA

    //Loading Notes:
    //-load object
    //-load object data in a second function (so that it exists for adding and keeping track of monorefs)
    //-if I write a simple collision system I could be rid of monorefs...but that may prove more annoying than my
    //hacky fix.



    void Start() {
        _repo = new Pool(ComponentIds.TotalComponents);

        //fileDir = Application.persistentDataPath; //comment out for testing
        fromLocalDisk = Application.persistentDataPath;
        //fromResources = Application.dataPath; //wrong, that's not how you load textasset resources fix when relevant
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
            _repo.CreateSystem<CharMoveSystem>(),
            //_repo.CreateSystem<MoveSystem>(),
            //replace/update with:
            //--load to pool
            //--instantiate from pool and view
            _repo.CreateSystem<LoadObjectsIntoSceneSystem>(),
            //bumpsystem goes here
            //instead of pushing a monobehaviour script
            //
            _repo.CreateSystem<RenderPositionSystem>(),

            _repo.CreateSystem<DestroySystem>(),
            _repo.CreateSystem<RenderDespawnSystem>(),
            //_repo.CreateSystem<AddScriptsToObjectsSystem>()
        };
    }

    void createProfile()
    {
        //setDefaultInputs();
        loadProfile("/GameData/Profiles/DefaultProfile");
        
    }

    void setDefaultInputs()
    {
        /*
        namesDict;
        stateDict;
        axisvalueDict;
        buttonsDict;
        axesDict;
        */
        ///////////////////////////////////////////////////
        /////TO-DO: 
        ////RETRIEVE DEFAULTS FROM RESOURCES
        ////SAVE DEFAULTS TO MY GAMEDATA PATH
        ////INTERNALLY SAVE DEFAULTS TO SAVEDINPUTS UNDER DEFAULT
        ////PUT PATH TO DEFAULTS UNDER PROFILE? OR PUT IT AS A SECTION UNDER PROFILE
        //defaults path
        targetFile = fileDir + "/GameData/myDefaults.json";
        //if defaults exist in path specified
        if (File.Exists(targetFile))
        {
            var str = File.ReadAllText(targetFile);
            JSONObject jData = new JSONObject(str);
            int lnCLM;
            int lnROW;
            string command;
            Dictionary<int, string> namesDict;
            Dictionary<string, Dictionary<string, bool>> stateDict; // ex: inputState["Left"]["isUp"];
            Dictionary<string, float> axisvalueDict;
            Dictionary<string, Dictionary<int, KeyCode>> buttonsDict;
            Dictionary<string, Dictionary<int, string>> axesDict;
            var e = _repo.CreateEntity();

            jData.GetField("DefaultInputs", delegate(JSONObject obj_1)
            {
                //...I know where max alt commands always is so I cheated, direct reference
                //Set the rows (possible alternate buttons for both axes and bttns)
                lnROW = (int)obj_1.list[0].n; //slightly unused feature, (for now sets capacity, should be used to limit alt command options
                obj_1.GetField("myCommands", delegate(JSONObject arr_1)
                {

                    lnCLM = arr_1.list.Count;//myCommands are listed here, get number of commands
                    //set capacity on outer dictionary
                    //they are all accessed by command name on outer so they have the same capacity
                    namesDict = new Dictionary<int, string>(lnCLM);
                    stateDict = new Dictionary<string, Dictionary<string, bool>>(lnCLM);
                    axisvalueDict = new Dictionary<string, float>(lnCLM);
                    buttonsDict = new Dictionary<string, Dictionary<int, KeyCode>>(lnCLM);
                    axesDict = new Dictionary<string, Dictionary<int, string>>(lnCLM);


                    Debug.Log("Number of actions" + lnCLM);
                    //For every command 
                    for (int p = 0; p < arr_1.list.Count; p++)
                    {
                        JSONObject obj = arr_1.list[p];

                        //Again I know the structure of the default inputs so I cheated
                        //probably should add some error catching!
                        command = obj.list[0].str;// store the name of our command
                        //Debug.Log("Name Field: " + obj.list[0].str);
                        // add keys set default values
                        namesDict.Add(p, command);
                        axisvalueDict.Add(command, 0f);
                        buttonsDict.Add(command, new Dictionary<int, KeyCode>(lnROW));
                        axesDict.Add(command, new Dictionary<int, string>(lnROW));

                        //hacky because I know the states I want to use and I'm not interested in making a state class
                        //maybe i should make a state class/struct...
                        //then I wouldn't need these rows or hacky references
                        //just stateDict[command].isUp ...etc
                        stateDict.Add(command, new Dictionary<string, bool>(4));
                        stateDict[command].Add("isUp", false);
                        stateDict[command].Add("isDown", false);
                        stateDict[command].Add("isHeld", false);
                        stateDict[command].Add("isAxis", obj.list[1].b);// only state retrieved from json

                        //get buttons array
                        //maybe I don't need to use getfield, buttons should always be found
                        //well buttons and axes are the easiest to mess up so error catching is nice
                        obj.GetField("buttons", delegate(JSONObject bttnList)
                        {
                            //For every alternate button
                            for (int i = 0; i < bttnList.list.Count; i++)
                            {
                                //ToString() doesn't evaluate properly.
                                var bttnName = bttnList.list[i].str;
                                try
                                {
                                    //convert string to KeyCode
                                    KeyCode keyBttn = (KeyCode)System.Enum.Parse(typeof(KeyCode), bttnName);
                                    //Debug.Log("KeyCode: " + keyBttn);
                                    //Add our button to its alt position under our command key
                                    buttonsDict[command].Add(i, keyBttn);
                                    //Debug.Log("My buttons Dict: " + buttonsDict[command][i]);
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

                        //get axes array
                        obj.GetField("axes", delegate(JSONObject axesList)
                        {
                            //For every alternate axes
                            for (int z = 0; z < axesList.list.Count; z++)
                            {
                                //Add our button to its alt position under our command key
                                axesDict[command].Add(z, axesList.list[z].str);
                                //Debug.Log("My axes Dict: " + axesDict[command][z]);
                            }
                        }, delegate(string dumbname2)
                        {
                            Debug.Log("axes not found");
                        });
                    }
                    //////////////////YO PAY ATTENTION IMPORTANT STUFF BELOW////////////////
                    e.AddMyInputs(namesDict, stateDict, axisvalueDict, buttonsDict, axesDict);

                    //e.AddMyInputs(cNames, cBttns, cAxes, cUP, cHeld, cDown, cAxesV, cBttnAxis);
                }, delegate(string bigerror)
                {
                    Debug.Log("myCommands not found, improper default inputs format");
                    //failsafe addmyinputs or something
                });
            }, delegate(string name)
            {
                Debug.Log("DefaultInputs not found");
                //failsafe addmyinputs or something
            });

        }
        else
        {
            Debug.Log("myDefaults.json not found in path specified: " + targetFile);
            //failsafe addmyinputs or something
        }
    }

    void createTestMap()
    {
        var e = _repo.CreateEntity();
        e.AddResource(mapTitles[mapIndex]);
        e.AddPosition(new Vector3(0, 0, 0));
    }
    void loadMovie()
    {
        //load a cutscene
    }

    void loadStartMenu()
    {
        //load the start menu
    }
    void loadProfile(string filePath)
    {
        //load a pre-made profile

    }

    

    void loadPlayerData()
    {
        //load data about the player, discerned from profile
        //parse from profile:
        //-current map/location
        //-current chapter/level
        //-key items/quest items
        //-medal progress/acheivements
        //-events/quests completed and events/quests progress
        //-game progress
    }

    void loadPartyData()
    {
        //load data about the party, discerned from profile
        //parse from profile:
        //-current party
        //--party member order
        //--party members current HP/MP
        //--party members current stats
        //--party members current skills
        //--party members current items
        //--party members current equipment
        //--party members current status
    }

    void loadMap()
    {
        //load the current map, discerned from profile
    }

    void loadPlayerAvatar()
    {
        //load on-screen player avatar, discerned from profile
        //parse from profile:
        //-current appearance
        //-current position
        //-current moveSPD
        //-receives input
    }
    void loadMapEncounters()
    {
        //load battle encounters (attaches to the player avatar entity), discerned from profile
        //parse from profile:
        //-battleIDs
        //-resource name for on-screen enemies (pre-battle)
        //-behavior for on-screen enemies (pre-battle)
    }
    void loadNPCs()
    {
        //...hmm, well I guess we'll code our own collision system for encountering NPCs
        //He wanted it so that NPCs are alerted to your presence so they'll need a zone and to track if the
        //player enters that zone
        //since I'll need to write a simple collision system for that I may as well...hmm or I could just attach
        //my monoref to a flat invisible circle/object under the npcs and see if the player hits that
        //I'd need a second (ontriggerstay) for one you're hugged up against an NPC and want a talk option to be
        //available
        //load on-screen NPC avatars, discerned from profile
        //parse from profile:
        //-appearance
        //-current position
        //-movement behavior
        //-dialogue IDs
        //-event IDs
    }

    void loadNPCData()
    {

    }

    void loadEnemyAvatars()
    {

    }

    void createPlayer() {
        var e = _repo.CreateEntity();
        //push scripts have to go before resource
        //e.isContactFights = true;
        
        //e.AddPushScripts(new List<string> { "ContactFightsComponent" });
        //e.AddContactFights(false, "Enemy");
        
        e.AddResource("Player");
        //Object reference hasn't loaded while this code block is running so we can't get access to it
        //Debug.Log(e.objectRef);
        //Component mycomp = e.objectRef.gameObject.AddComponent("TriggerForEncounters");
        //e.AddPushScripts(new List<string> { "TriggerForEncounters" });
        //e.AddEncounterMonoRef(e.objectRef.gameObject.AddComponent("TriggerForEncounters"));
        //e.AddEncounterTrigger(new string[] { "Enemy", "Enemy", "Enemy", "Enemy", "Enemy", "Enemy" }, new string[] { "", "", "", "", "", "" });
        //e.AddTriggerReference(e.view.gameObj.addComponent("TriggerForBattles"));
        //e.AddEncounters(e.objectRef.gameObj.addComponent("TriggerForEncounters"), areaTriggerTags, areaBattleIDs);
        e.isPlayerCharacter = true;
        e.AddPosition(new Vector3 (0, 0.5f, 0));
        e.AddCharMove(new Vector3 (.1f, 0, .1f));
        e.isReceivesInput = true;
        //e.re
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
