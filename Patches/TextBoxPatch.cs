using HarmonyLib;
using JetBrains.Annotations;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using static ITP_TextSkip.TextSkip;

namespace ITP_TextSkip.Patches
{

    [HarmonyPatch(typeof(TextBox))]
    internal class TextBoxPatch
    {

        public static bool Registered = false;
        public static bool AudioInitalized = false;

        public static TextBox Txtbox = null;

        public static bool TxtboxActive()
        {
            if (Txtbox == null)
                return false;

            if (!Txtbox.IsDialogueActive())
                return false;

            return true;
        }

        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void PostAwake(ref TextBox __instance)
        {
            if (!Registered)
            {
                GameInput.Register(GameInput.KeyMap.Run, 8000, new Func<InputAction.CallbackContext, bool>(OnSkipInput));

                Registered = true;
                logger.Log("Registed run input for txt skip");
            }

            TextBox.AutoSkipDialogues = false;
            Txtbox = __instance;
        }

        public static bool OnSkipInput(InputAction.CallbackContext ctx)
        {
            if (!TxtboxActive())
            {
                TextBox.AutoSkipDialogues = false;
                return false;
            }

            if (ctx.started)
            {
                logger.Log("Skipping!");
                TextBox.AutoSkipDialogues = true;
            }
            else if (ctx.canceled)
            {
                logger.Log("Stopped skipping!");
                TextBox.AutoSkipDialogues = false;
            }

            // Skips the dialogue
            if (TextBox.AutoSkipDialogues)
            {
                TextBox.SkipTypewriter();
                if (!Txtbox.DisplayNext())
                {
                    Txtbox.Hide();
                }
            }

            return true;
        }
        
    }
}
