using BepInEx;
using HarmonyLib;
using JuneLib;
using JuneLib.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JuneGunJamSubmission
{
    [BepInDependency("etgmodding.etg.mtgapi")]
    [BepInDependency("alexandria.etgmod.alexandria")]
    [BepInDependency("blazeykat.etg.junelib")]
    [BepInPlugin(GUID, MOD_NAME, VERSION)]
    public class Module : BaseUnityPlugin
    {
        public const string MOD_NAME = "Court of the Crimson King";
        public const string VERSION = "1.0.0";
        public static readonly string TEXT_COLOR = "#FF0000";
        public static readonly string PREFIX = "junegunjam";
        public static readonly string ASSEMBLY_NAME = "JuneGunJamSubmission";
        public const string GUID = "blazeykat.etg.gunjamsubmission";

        public void Start()
        {
            Debug.Log("");
            ETGModMainBehaviour.WaitForGameManagerStart(GMStart);
        }

        public void GMStart(GameManager g)
        {
            try
            {
                new Harmony(GUID).PatchAll();
                PrefixHandler.AddPrefixForAssembly(PREFIX);
                ItemTemplateManager.Init();

                Fog($"{MOD_NAME} v{VERSION} has successfully loaded!", TEXT_COLOR);
                Fog("\"...as the puppets dance...\"", TEXT_COLOR);
                Fog(" - ------ - ", TEXT_COLOR);
                Fog($"If you need to cheat in any item, all items use the prefix \"{PREFIX}\"", TEXT_COLOR);
            } catch (Exception e)
            {
                ETGModConsole.Log(e);
                Fog("june gun jam broke ):", TEXT_COLOR);
            }
        }

        public static void Fog(string text, string color = "#FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }

        /*
        GCC2 Ideas 
        
        GCC1 Ideas
        Link Hook - "Connected Spirits"
        - Active item, allows you to grapple onto an enemy and permanently charm them

        Crying Minotaur - "Everyone around"
        - Passive item, Enemies that get damaged deal damage in enemies to proximity

        The National Anthem - "It's holding on"
        - like crying minotaur but its a gun

        Bonesaw Rifle - "TWO INTO ONE"
        - gun, sticks enemies together (will only work if low enough HP

        Gungeon Veins - "Interconnected rooms"
        - Passive item, clearing a room without taking damage has a chance to clear another adjacent room, +2 curse

        President's Gun - "Dojyan!"
        - Full-auto gun, final projectile will make a charmed clone of an enemy, that will spawn nearby the player and will explode if it touches the original version

        Interdimensional Meat Hook - "(Inter)Connector"
        - acts like ud expect

        ???
        - Isaac's Heart Esque Item 

        Invokier
        - Gungeon Proper enemy, links itself to enemies and makes them shoot for them. killing an enemy attached will do damage to it, if all enemies attached die invokier dies
        - Bandolier ammo belt thing

        Rogue Invokier
        - Abbey enemy, like an invokier but will make enemies (and himself) explode

        Bulletipede
        - That buttgripper wall thing idea u had for something wicked

        Wizard Clip
        - Passive item, damaging enemy has a chance to link u to said enemy and make them invincible, reload == swap spots with them + some temporary invulnerability

        Ectoplasm (think of a good gun pun lol)
        - Ghost enemy that attaches itself to another high HP enemy, gives the enemy some bullet blocking orbitals

        Necroplasm ( also think of another good gun pun)
        - enemy, Upgraded version of enemy above that also makes enemy attached to jammed, and gets a movement speed buff

        Stitches 
        - Active Item, acts like antibirth stitches

        -Warped Ammomancer
        - Spawns 3 little shelleton guys that hes attached to, damaging any of these little guys damages all the other 3

        Pearl Fang - "Are you you?"
        - Idk???

        also maybe some shrine that can only be opened if two chests on the current floor are present, and will destroy both of them for 2 heart container and give u contents equal to it
        */
    }
}
