using MelonLoader;
using UnityEngine;
using Il2Cpp;
using UnityEngine.UI;
using System.Linq.Expressions;

namespace NoBloodMod
{
    /*
     * there will be no blood, or well, itll be green
     */
    public class NoBloodMod : MelonMod
    {
        private string listOfMods = "Active Mods \n";
        private string version = "";
        private static bool inGame = false;
        private static float spriteInterval = 0f;
        private static bool done = false;

        public override void OnInitializeMelon()
        {
            foreach (MelonMod mod in RegisteredMelons)
            {
                listOfMods = listOfMods + mod.Info.Name + " by " + mod.Info.Author + "\n";
            }
        }

        private void DrawRegisteredMods()
        {
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.UpperRight;
            style.normal.textColor = Color.white;

            GUI.Label(new Rect(Screen.width - 500 - 10, 100, 500, 100), listOfMods, style);
        }

        private void DrawVersion()
        {
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.UpperRight;
            style.normal.textColor = Color.white;

            if (version == "")
            {
                version = GameObject.Find("VersionText").GetComponent<Text>().m_Text;
            }

            GUI.Label(new Rect(Screen.width - 500 - 10, 85, 500, 15), version, style);
        }

        public override void OnUpdate()
        {
            try
            {
                spriteInterval += Time.deltaTime;
                if (inGame && spriteInterval > 4 && !done)
                {
                    Image image = GameObject.Find("PlayerUI").transform.GetChild(2).GetComponent<Image>();
                    image.color = new Color32(0, 200, 0, 0);
                    Transform damageLayers = GameObject.Find("DamageScreen").transform;
                    for (int i = 0; i < damageLayers.childCount; i++)
                    {
                        Image layerDamage = damageLayers.GetChild(i).GetComponent<Image>();
                        layerDamage.color = new Color32(0, 240, 0, 0);
                    }
                    GameObject[] allGameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

                    foreach (GameObject obj in allGameObjects)
                    {
                        if (obj.name.ToLower().Contains("blood") && obj.name.ToLower() != "bloodscreen")
                        {
                            obj.SetActive(false);
                        }
                    }
                    done = true;
                }
            }
            catch (Exception e)
            {
                
            }
        }


        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (sceneName == "MainLevel" || sceneName == "GRASS_ROOMS_SCENE" || sceneName == "HOTEL_SCENE")
            {
                MelonEvents.OnGUI.Unsubscribe(DrawRegisteredMods);
                MelonEvents.OnGUI.Unsubscribe(DrawVersion);
                inGame = true; 
            }

            if (sceneName == "MainMenu")
            {
                MelonEvents.OnGUI.Subscribe(DrawRegisteredMods, 100);
                MelonEvents.OnGUI.Subscribe(DrawVersion, 100);
                inGame = false;
                done = false;
            }

        }
    }
} 