using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading;

namespace FunnyAlgorithm
{
    public class StartControl: MonoBehaviour
    {
        public StartView view;


        private List<RectTransform> rectNodes = new List<RectTransform>();
        private List<ArrayNode> SortNodes = new List<ArrayNode>();
        private List<Vector2> OriginPos = new List<Vector2>();
        private SortBasicModel demo;
        private float duration = 0.5f;

        // Start is called before the first frame update
        void Start()
        {
            ArrayNode.width = 50;
            ArrayNode.verticalStandard = 60;
            MoveTool.duration = 0.3f;

            for ( int i = 0; i < view.Nodes.Length; i++ )
            {
                SortNodes.Add(new ArrayNode(view.Nodes[i], view.Nodes.Length - i, i, false));
                rectNodes.Add(view.Nodes[i].GetComponent<RectTransform>());
                OriginPos.Add(new Vector2(rectNodes[i].anchoredPosition.x, rectNodes[i].anchoredPosition.y));
            }
            demo = new InsertSortDemoModel(SortNodes);
            demo.RecordProce();
        }


        public void ModeMouseIn(int index)
        {
            view.rects[index].DOSizeDelta(new Vector2(360f, 330f), duration);
            view.images[index].DOColor(MyTools.Color_HexToRgb("#FFBC8E"), duration);
            view.texts[index].DOScale(new Vector3(1.3f, 1.3f, 1f), duration);

        }
        public void ModeMouseOut(int index)
        {
            view.rects[index].DOSizeDelta(new Vector2(300f, 300f), duration);
            view.images[index].DOColor(MyTools.Color_HexToRgb("#A1DBFF"), duration);
            view.texts[index].DOScale(new Vector3(1f, 1f, 1f), duration);
        }

        #region 学习&演示模式
        bool DirFlag = true;
        public void StartStudy_Demo()
        {
            DirFlag = true;
            StopCoroutine("ShowStudy_DemoCoroutine");
            StartCoroutine("ShowStudy_DemoCoroutine");
        }

        public void BackStudy_Demo()
        {
            DirFlag = false;
            StopCoroutine("ShowStudy_DemoCoroutine");
            StartCoroutine("ShowStudy_DemoCoroutine");
        }
        #endregion

        #region 练习模式
        public void MagnifyActive(Animator animator)
        {
            animator.enabled = true;
            view.Book.DOSizeDelta(new Vector2(250f, 250f), duration);
        }
        public void MagnifyStop(Animator animator)
        {
            animator.enabled = false;
            view.Book.DOSizeDelta(new Vector2(200f, 200f), duration);
        }
        #endregion

        #region 编程模式

        public void StartCoding()
        {

            view.CodeText.DOText(view.text.text, duration * 10).SetEase(Ease.Linear);
            view.CodeText.DOPlayForward();
            view.Computer_Keyboard.DOSizeDelta(new Vector2(300f, 240f), duration);
        }
        public void PauseCoding()
        {
            view.CodeText.DOKill();
            view.CodeText.text = "";
            view.Computer_Keyboard.DOSizeDelta(new Vector2(250f, 200f), duration);
        }

        #endregion

        private IEnumerator ShowStudy_DemoCoroutine()
        {
            yield return new WaitForSeconds(MoveTool.duration);
            if ( DirFlag )
            {
                while ( demo.demoQueue.Count > 0 )
                {
                    bool flag;
                    do
                    {
                        flag = demo.PlayForward();
                        if (!flag)
                            yield return new WaitForSeconds(MoveTool.duration);
                    } while (flag);
                }
            }
            else
            {
                while ( demo.executedStack.Count > 0 )
                {
                    bool flag;
                    do
                    {
                        flag = demo.PlayBackward();
                        if (!flag)
                            yield return new WaitForSeconds(MoveTool.duration);
                    } while (flag);
                }
            }
        }

        private void OnDestroy()
        {
            demo.IsRun = false;
        }

    }

}
