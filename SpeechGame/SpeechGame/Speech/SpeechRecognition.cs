//#define UseProgram

#region Using Statements

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;

#endregion

namespace SpeechGame
{
    public class SpeechRecognition
    {
        static bool jump, left, right, stop, shoot, gust;
        SpeechRecognitionEngine recognizer;

        readonly string[] speechChoices = new string[] {
            "left",
            "one",
            "stop",
            "up",
            "e",
            "eeee"
           };

        readonly string[] speechCommands  = new string[] {
            "left (A)",
            "right (D)",
            "stop (S)",
            "jump (W)",
            "shoot (R)",
            "gust (E)"
           };

        #if UseProgram
        public SpeechRecognition()
        {
            jump = left = right = stop = shoot = gust = false;
            SpeechRecognizer recognizer = new SpeechRecognizer();
            Choices choices = new Choices();
            choices.Add(new string[] { "left","one", "stop", "up", " shoot ","e"});

            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(choices);

            Grammar g = new Grammar(gb);
            recognizer.LoadGrammar(g);
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);

        }
        #else
        public SpeechRecognition()
        {
            jump = left = right = stop = shoot = gust = false;
            recognizer = new SpeechRecognitionEngine();
            recognizer.RequestRecognizerUpdate();
            Choices choices = new Choices();
            choices.Add(speechChoices);

            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(choices);

            Grammar g = new Grammar(gb);
            recognizer.LoadGrammar(g);
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.RecognizeAsync(RecognizeMode.Multiple);

            UpdateRecognizerSettings();
            AppendSpeechChoices();
        }
        #endif

        void AppendSpeechChoices()
        {
            Console.WriteLine();
            Console.WriteLine(" Voice Commands /  Actions (Keyboard keys)");
            Console.WriteLine(" ------------------------- ");
            for(int i=0;i<speechChoices.Count();i++)
                Console.WriteLine("\t" + speechChoices[i] + "\t-  "+ speechCommands[i]);
        }

        #region Recognizer settings

        readonly string[] settings = new string[] {
            "ResourceUsage",
            "ResponseSpeed",
            "ComplexResponseSpeed",
            "AdaptationOn",
            "PersistedBackgroundAdaptation",
           };

        void UpdateRecognizerSettings()
        {
            /*Console.WriteLine("Settings for recognizer {0}:",
              recognizer.RecognizerInfo.Name);
            Console.WriteLine();
            ListSettings();  */

            Console.WriteLine("Configuring Voice Recognition...");

            recognizer.UpdateRecognizerSetting("ResponseSpeed", 0);
            recognizer.UpdateRecognizerSetting("ComplexResponseSpeed", 0);
            recognizer.UpdateRecognizerSetting("AdaptationOn", 1);
            recognizer.UpdateRecognizerSetting("PersistedBackgroundAdaptation", 0);
            
            Console.WriteLine();

            ListSettings();
        }

        void ListSettings()
        {
            foreach (string setting in settings)
            {
                try
                {
                    object value = recognizer.QueryRecognizerSetting(setting);
                    Console.WriteLine("  {0,-30} = {1}", setting, value);
                }
                catch
                {
                    Console.WriteLine("  {0,-30} is not supported by this recognizer.",
                      setting);
                }
            }
            Console.WriteLine();
        }

        #endregion

        #region Events

        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence >= 0.7)
                switch (e.Result.Text)
                {
                    case "up":
                        jump = true;
                        break;
                    case "left":
                        left = true;
                        break;
                    case "one":
                        right = true;
                        break;
                    case "stop":
                        stop = true;
                        break;
                    case "e":
                        shoot = true;
                        break;
                    case "eeee":
                        gust = true;
                        break;
                }
        }

        #endregion

        #region Static Calls

        public static bool Left()
        {
            if (left)
            {
                left = false;
                return true;
            }
            return false;
        }

        public static bool Shoot()
        {
            if (shoot)
            {
                shoot = false;
                return true;
            }
            return false;
        }

        public static bool Gust()
        {
            if (gust)
            {
                gust = false;
                return true;
            }
            return false;
        }

        public static bool Right()
        {
            if (right)
            {
                right = false;
                return true;
            }
            return false;
        }

        public static bool Stop()
        {
            if (stop)
            {
                stop = false;
                return true;
            }
            return false;
        }

        public static bool Jump()
        {
            if (jump)
            {
                jump = false;
                return true;
            }
            return false;
        }

        #endregion
    }
}
