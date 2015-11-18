using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Windows.Globalization;
using Windows.Media.SpeechRecognition;

namespace CameraFaceDetection.Common
{
    public static class ResourceHelper
    {
        private static ResourceContext speechContext = ResourceContext.GetForCurrentView();
        private static ResourceMap speechResourceMap = ResourceManager.Current.MainResourceMap.GetSubtree("Resources");
        private static string langTag = SpeechRecognizer.SystemSpeechLanguage.LanguageTag;
        private static SpeechRecognizer recognizer = null;
        public static string GetString(string key)
        {        
            Language speechLanguage = SpeechRecognizer.SystemSpeechLanguage;
            speechContext.Languages = new string[] { langTag };
            string ret = speechResourceMap.GetValue(key, speechContext).ValueAsString;
            return ret;
        }

      
        public async static Task<SpeechRecognizer>  InitRecognizer()
        {
            try
            {
                if (null != recognizer)
                {                   
                    recognizer.Dispose();
                    recognizer = null;
                }
                recognizer = new SpeechRecognizer(SpeechRecognizer.SystemSpeechLanguage);
                recognizer.Constraints.Add(
                    new SpeechRecognitionListConstraint(
                        new List<string>()
                        {
                        speechResourceMap.GetValue("account page", speechContext).ValueAsString,
                        speechResourceMap.GetValue("audit page", speechContext).ValueAsString,
                        speechResourceMap.GetValue("finace page", speechContext).ValueAsString,
                        speechResourceMap.GetValue("transfer page", speechContext).ValueAsString
                        }, "goto"));

                SpeechRecognitionCompilationResult compilationResult = await recognizer.CompileConstraintsAsync();
                if (compilationResult.Status != SpeechRecognitionResultStatus.Success)
                {
                    recognizer.Dispose();
                    recognizer = null;
                }

                //string uiOptionsText = string.Format("Try saying '{0}', '{1}' or '{2}'",
                //        speechResourceMap.GetValue("account page", speechContext).ValueAsString,
                //        speechResourceMap.GetValue("audit page", speechContext).ValueAsString,
                //        speechResourceMap.GetValue("audit page", speechContext).ValueAsString);
                //recognizer.UIOptions.ExampleText = uiOptionsText;
                return recognizer;
            }
            catch(Exception e)
            {             
                return null;
            }
           
        }

    }
}
