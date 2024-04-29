using System;
using wcs.core.model;

namespace wcs.core.common
{
    /// <summary>
    /// 模拟数据
    /// </summary>
    public class RunningPoints
    {
        private Queue<Point> runningPath;
        private Point defaultPoint = new Point()
        {
            x = "2000",
            y = "2000",
            z = "0"
        };

        private Point _curPoint;
        public Point curPoint
        {
            get { return _curPoint; }
        }

        private static RunningPoints _instance;
        private RunningPoints()
        {
            runningPath = new Queue<Point>();
            _curPoint = new Point()
            {
                x = "2000",
                y = "2000",
                z = "0"
            };
        }
        public static RunningPoints instance
        {
            get
            {
                if (RunningPoints._instance == null)
                {
                    RunningPoints._instance = new RunningPoints();
                }

                return RunningPoints._instance;
            }
        }

        public void Rk(Point target)
        {
            runningPath.Clear();

            if (_curPoint.x != defaultPoint.x)
            {
                // 先计算回输送线路径
                getPath(curPoint, defaultPoint);
            }

            getPath(defaultPoint, target);

            RunAsync();
        }

        private void RunAsync()
        {
            do
            {
                _curPoint = runningPath.Dequeue();
                Thread.Sleep(500);
            } while (runningPath.Count > 0);
        }

        private void getPath(Point startPoint, Point endPoint)
        {
            int starX = int.Parse(startPoint.x);

            int endX = int.Parse(endPoint.x);

            int step = 400;

            if (starX == endX) { return; }
            else if (starX > endX)
            {
                do
                {
                    runningPath.Enqueue(new Point()
                    {
                        x = $"{starX}",
                        y = "2000",
                        z = "0"
                    });

                    starX -= step;
                } while (starX > endX);
            }
            else if (starX < endX)
            {
                do
                {
                    runningPath.Enqueue(new Point()
                    {
                        x = $"{starX}",
                        y = "2000",
                        z = "0"
                    });

                    starX += step;
                } while (starX < endX);
            }
        }

        public bool IsBusy()
        {
            return runningPath.Count > 0;
        }
    }
}

