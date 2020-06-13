using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace CWHACK_DEOBF
{
   public  class Main : MonoBehaviour
    {
        public static List<GameObject> enemies = new List<GameObject>();
        public static Texture2D redTexture;
        public static Texture DefaultTexture;
        public static Shader inivisibleShader = Shader.Find("Hidden/Internal-GUITexture");
        public int curr = 0;
        public int max = 50;
        private bool gayMode = false;
        private bool Chams = false;
        private bool BoxEsp = false;
        private bool LineEsp = false;
        private bool DistEsp = false;
        private bool HpEsp = false;
        public static bool once = false;
        public bool fog = false;
        public bool redChams = false;
        public static Shader DefaultShader;
        private static bool menu = false;
        private bool brightness = false;
        private Color ambientColorOriginal;
        public static void RECT(float x, float y, float width, float height, Texture2D text)
        {
            GUI.DrawTexture(new Rect(x, y, width, height), text);
        }
        private bool isVisble(Vector3 toCheck)
        {
            RaycastHit hit;
            if (Physics.Linecast(Camera.main.transform.position, toCheck, out hit))
            {
                if (hit.transform.name.Contains("NPC") || hit.transform.name.Contains("client")
                    || hit.transform.name == Camera.main.name || hit.transform.name == Camera.main.gameObject.name
                    || hit.transform.name == Camera.main.transform.name || hit.transform.tag.Contains("Player"))
                {
                    return true;
                }
            }
            return false;
        }
        public static GameObject getPlayerBYIDfdfs(int id1333337777)
        {
            GameObject hnbvbncvhgbfd;
            int xcxzc = id1333337777;
            hnbvbncvhgbfd = Peer.ClientGame.AllPlayers[xcxzc].PlayerObject;
            return hnbvbncvhgbfd;
        }
        private static Rect lineRect = new Rect(0f, 0f, 1f, 1f);
        public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width, bool antiAlias)
        {
            float num = pointB.x - pointA.x;
            float num2 = pointB.y - pointA.y;
            float num3 = Mathf.Sqrt(num * num + num2 * num2);
            if (num3 < 0.001f)
            {
                return;
            }
            float num4 = width * num2 / num3;
            float num5 = width * num / num3;
            Matrix4x4 identity = Matrix4x4.identity;
            identity.m00 = num;
            identity.m01 = -num4;
            identity.m03 = pointA.x + 0.5f * num4;
            identity.m10 = num2;
            identity.m11 = num5;
            identity.m13 = pointA.y - 0.5f * num5;
            GL.PushMatrix();
            GL.MultMatrix(identity);
            GUI.DrawTexture(lineRect, redTexture);
            GL.PopMatrix();
        }
        public static void FilledRect(float x, float y, float width, float height, Texture2D text, float thickness = 1f)
        {
            RECT(x, y, thickness, height, text);
            RECT(x + width - thickness, y, thickness, height, text);
            RECT(x + thickness, y, width - thickness * 2f, thickness, text);
            RECT(x + thickness, y + height - thickness, width - thickness * 2f, thickness, text);
        }

        public static void Box(float x, float y, float width, float height, Texture2D text, float thickness = 1f)
        {
            FilledRect(x - width / 2f, y - height, width, height, text, thickness);
        }

        void Start()
        {
            redTexture = new Texture2D(2, 2, TextureFormat.ARGB32, false);          
            redTexture.SetPixel(0, 0, Color.red);
            redTexture.SetPixel(1, 0, Color.red);
            redTexture.SetPixel(0, 1, Color.red);
            redTexture.SetPixel(1, 1, Color.red);
            redTexture.Apply();

        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Home))
            {
                menu = !menu;
            }
            try
            {
                foreach (GameObject gameObj in enemies)
                {
                    if (gameObj != null)
                    {
                        var renderChilds = gameObj.GetComponentsInChildren<Renderer>();
                        foreach (Renderer rend in renderChilds)
                        {
                            if (!once)
                            {
                                DefaultTexture = rend.material.mainTexture;
                                DefaultShader = rend.material.shader;
                                once = true;
                            }
                            if (rend.material.shader.name.Contains("U") || rend.material.shader.name.Contains("B"))
                            {
                                rend.material.shader = DefaultShader;
                                rend.material.mainTexture = DefaultTexture;

                            }
                        }
                    }
                }
            }
            catch { };
            enemies.Clear();
            curr = max;
            if (curr <= max)
            {
                curr--;
                List<EntityNetPlayer> list = Peer.ClientGame.AlivePlayers;
                foreach (EntityNetPlayer pUNKNOWN in list)
                {
                    var f = pUNKNOWN.PlayerObject;
                    if (gayMode)/*PEER.SERVERGAME.ISTEAMGAME*/
                    {
                        enemies.Add(f);
                    }
                    else/*PEER.SERVERGAME.ISTEAMGAME*/
                    {
                        if (pUNKNOWN.IsBear && !Peer.ClientGame.LocalPlayer.IsBear)
                            enemies.Add(f);
                        if (!pUNKNOWN.IsBear && Peer.ClientGame.LocalPlayer.IsBear)
                            enemies.Add(f);
                    }
                }
                foreach (GameObject f in enemies)
                {
                    if (Peer.IsConnected && Chams == true)
                    {
                        var rend = f.GetComponentsInChildren<Renderer>();
                        foreach (Renderer renderer in rend)
                        {
                            if (renderer.material.name.Contains("U") || renderer.material.name.Contains("B"))//u = usec; b = bear;
                            {
                                if ((isVisble(f.transform.position) || isVisble(GetEntityNetPlayer(f).NeckPosition)) && (redChams == false))
                                {
                                    DefaultTexture = renderer.material.mainTexture;
                                    renderer.material.mainTexture = redTexture;
                                    renderer.material.shader = DefaultShader;
                                }
                                else if ((!isVisble(f.transform.position) || isVisble(GetEntityNetPlayer(f).NeckPosition)) && (redChams == false))
                                {
                                    DefaultShader = renderer.material.shader;
                                    renderer.material.mainTexture = DefaultTexture;
                                    renderer.material.shader = inivisibleShader;
                                }
                                else
                                {
                                    DefaultTexture = renderer.material.mainTexture;
                                    DefaultShader = renderer.material.shader;
                                    GUI.Label(new Rect(500, 500, 200, 200), "WORKS");
                                    renderer.material.mainTexture = redTexture;
                                    renderer.material.shader = inivisibleShader;
                                }
                            }
                        }
                    }
                }

            }

        }
        public EntityNetPlayer GetEntityNetPlayer(GameObject GetEntityNetPlayer)
        {
            for (int i = 0; i < Peer.ClientGame.AlivePlayers.Count; i++)
            {
                if (Peer.ClientGame.AlivePlayers[i].PlayerObject == GetEntityNetPlayer)
                {
                    return Peer.ClientGame.AlivePlayers[i];
                }
            }
            GUI.Box(new Rect(100, 100, 200, 200), "CANNT FIND");
            return Peer.ClientGame.AlivePlayers[0];
        }

        void OnGUI()
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                GameObject.Destroy(this);
            }
            foreach (GameObject opop32312312 in enemies)
            {
                Transform[] allChildren = opop32312312.GetComponentsInChildren<Transform>();
                foreach (Transform child in allChildren)
                {
                    if (child.name == "NPC_Head")
                    {
                        Vector3 w2s = Camera.main.WorldToScreenPoint(opop32312312.transform.position);
                        Vector3 w2s2 = Camera.main.WorldToScreenPoint(child.position);
                        float dist = Vector3.Distance(Camera.main.transform.position, opop32312312.transform.position);
                        if (w2s.z > -1 && w2s.z > -1 && dist > 2)
                        {
                            float height = Math.Abs(w2s2.y - w2s.y);
                            if (BoxEsp) Box(w2s2.x, Screen.height - w2s2.y, height / 1.6f, height, redTexture, 2);
                            if (LineEsp) DrawLine(new Vector2(Screen.width / 2, Screen.height - 50), new Vector2(w2s2.x, Screen.height - w2s2.y), new Color(255, 0, 0), 1, false);
                            if (DistEsp) GUI.Label(new Rect(w2s2.x + height / 2, Screen.height - w2s2.y, height * 1.4f, height), dist.ToString());
                            EntityNetPlayer netPlayer = GetEntityNetPlayer(opop32312312);
                            netPlayer.playerInfo.Update(netPlayer.playerInfo);
                            float health = netPlayer.playerInfo.sHealth;
                            if (HpEsp) GUI.Label(new Rect(w2s2.x + height / 2, Screen.height - w2s.y, height * 2, height * 2), health.ToString());
                        }
                    }
                }
            }
            // RenderSettings.ambientLight = new Color(255, 255, 255,100);


            GUI.Label(new Rect(Screen.width / 2, 0, 500, 100), "ROMANHOOK Copyright 2020 CREDIT YT CHANELL Roman Parhomenko");
            if (menu)
            {
                GUI.Box(new Rect(50, 0, 400, 350), "ROMANHOOK");
                if (GUI.Button(new Rect(50, 50, 350, 50), Chams == true ? "CHAMS ON" : "CHAMS OFF"))
                {
                    Chams = !Chams; enemies.Clear();
                    if (!Chams)
                    {
                        once = false;
                        foreach (EntityNetPlayer model in Peer.ClientGame.AllPlayers)
                        {
                            var renderChilds = model.PlayerObject.GetComponentsInChildren<Renderer>();
                            foreach (Renderer rend in renderChilds)
                            {
                                rend.material.shader = DefaultShader;
                                rend.material.mainTexture = DefaultTexture;
                            }
                        }
                    }

                };
                if (GUI.Button(new Rect(50, 100, 350, 50), BoxEsp == true ? "BOX ESP ON" : "BOX ESP OFF")) { BoxEsp = !BoxEsp; };
                if (GUI.Button(new Rect(450, 100, 300, 50), gayMode == true ? "IS  DM" : "TEAM")) { gayMode = !gayMode; enemies.Clear(); };
                if (GUI.Button(new Rect(50, 150, 350, 50), LineEsp == true? "LINE ESP OFF" : "LINE ESP ON")) { LineEsp = !LineEsp; };
                if (GUI.Button(new Rect(50, 200, 350, 50), "DIST ESP")) { DistEsp = !DistEsp; };
                if (GUI.Button(new Rect(50, 250, 350, 50), HpEsp ==true ? "HP ESP OFF" : "HP ESP ON")) { HpEsp = !HpEsp; };
                if (GUI.Button(new Rect(50, 300, 350, 50), fog == true ? "FOG OFF" : "FOG ON"))
                {
                    fog = !fog;
                    if (!fog)
                    {
                        RenderSettings.fog = false;
                    }
                    else
                    {
                        RenderSettings.fog = true;
                    }
                };
                if (GUI.Button(new Rect(50, 350, 350, 50), redChams == true ? "RED CHAMS OFF" : "RED CHAMS ON"))
                {
                    redChams = !redChams;
                }
                if (GUI.Button(new Rect(50, 400, 350, 50), brightness == true ? "BRIGHTNESS OFF" : "BRIGHTNESS ON"))
                {
                    brightness = !brightness;
                    if (brightness == true)
                    {
                        ambientColorOriginal = RenderSettings.ambientLight;
                        RenderSettings.ambientLight = new Color(255, 0, 0);
                    }
                    else RenderSettings.ambientLight = ambientColorOriginal;
                }
            }
        }
    }
}
