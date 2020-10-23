using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace FunnyAlgorithm
{
    public class MainControl : MonoBehaviour
    {
        public static Dictionary<string, string> ColorSetting = new Dictionary<string, string>();
        public static int SORTTYPE = 0;
        private void Awake()
        {
            #region 配色方案
            ColorSetting["normal"] = "FFFFFF";
            ColorSetting["selected"] = "FF8566";
            ColorSetting["successed"] = "66DD66";
            ColorSetting["signed"] = "0099FF";
            #endregion
        }

        public void LoadStartInterface()
        {
            SceneManager.LoadScene(0);
            SortDemoModel.IsFinish = false;
            SortDemoModel.IsRun = false;
        }

        public void LoadDemoInterface(int index)
        {
            SORTTYPE = index;
            SceneManager.LoadScene(1);
        }
        public void Quit()
        {
            Application.Quit();
        }
      
    }
}
