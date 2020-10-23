using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace FunnyAlgorithm
{
    public class StartControl : MonoBehaviour
    {
        public StartView view;
        private List<List<Node>> SortNodes = new List<List<Node>>();
        private List<SortDemoModel> demo = new List<SortDemoModel>();
        private float duration = 0.5f;
        // Start is called before the first frame update
        void Start()
        {
            Node.width = 50;
            Node.verticalStandard = 60;
            MoveTool.duration = 0.3f;
            for ( int i = 0; i < view.Nodes.Length; i++ )
            {
                List<Node> temp = new List<Node>();
                int len = view.Nodes[i].transform.childCount;
                for ( int j = 0; j < len; j++ )
                {
                    temp.Add(new Node(view.Nodes[i].transform.GetChild(j).gameObject, len - j, j, false));
                }
                SortNodes.Add(temp);
            }

            demo.Add(new InsertSortModel(SortNodes[0]));
            demo.Add(new SelectSortModel(SortNodes[1]));
            demo.Add(new BubbleSortModel(SortNodes[2]));
            demo.Add(new ShellSortModel(SortNodes[3]));
            demo.Add(new MergeSortModel(SortNodes[4]));
            demo.Add(new QuickSortModel(SortNodes[5]));

            for ( int i = 0; i < demo.Count; i++ )
            {
                demo[i].RecordProce();
            }
        }

        public void ModeMouseIn(int index)
        {
            StopCoroutine("ForwardCoroutine");
            for ( int i = 0; i < 6; i++ )
            {
                if ( i != index ) 
                {
                    view.Nodes[i].SetActive(false);
                }
            }
            StartCoroutine("ForwardCoroutine", index);
            view.rects[index].DOSizeDelta(new Vector2(300f, 250f), duration);
            view.images[index].DOColor(MyTools.Color_HexToRgb("#FFBC8E"), duration);
            view.texts[index].DOScale(new Vector3(1.15f, 1.15f, 1f), duration);
        }
        public void ModeMouseOut(int index)
        {
            view.Nodes[index].SetActive(false);
            view.rects[index].DOSizeDelta(new Vector2(300f, 80f), duration);
            view.images[index].DOColor(MyTools.Color_HexToRgb("#A1DBFF"), duration);
            view.texts[index].DOScale(new Vector3(1f, 1f, 1f), duration);
        }

        public IEnumerator ForwardCoroutine(int index)
        {
            yield return new WaitForSeconds(MoveTool.duration);
            view.Nodes[index].SetActive(true);
            while ( demo[index].DemoQueue.Count > 0 )
            {
                StartCoroutine(demo[index].PlayForward());
                yield return new WaitForSeconds(MoveTool.duration);
                if ( demo[index].DemoQueue.Count == 0 )
                {
                    if ( index == 0 || index == 3 || index == 4 )
                        StartCoroutine(demo[index].FinishSort());
                }
            }
        }
        private void OnDestroy()
        {
            SortDemoModel.IsRun = false;
        }
    }
}
