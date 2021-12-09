using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace InstanceReference
{
    public static class GUIExntensions
    {
        public static async Task ChangeOpacity(this Window window, double from, double to, double duration = 3000)
        {
            double durationStep = 100;
            double durationSteps = duration / durationStep;

            double valueStep = (to - from) / durationSteps;

            await Task.Run(() =>
            {
                var timer = new Timer((state) =>
                {

                }, null, 0, Timeout.Infinite);

                //var waitEvent = new AutoResetEvent(false);

                //var timers = new List<Timer>();
                //var waitHandles = new List<WaitHandle>();

                //double currentValue = 0;
                //int stepCounter = -1;

                //while (true)
                //{
                //    currentValue += valueStep;
                //    stepCounter += 1;

                //    bool isStop = false;

                //    if ((valueStep < 0 && currentValue < to) ||
                //        (valueStep > 0 && currentValue > to))
                //    {
                //        currentValue = to;
                //        isStop = true;
                //    }

                //    var resetEvent = new AutoResetEvent(false);
                //    (double, AutoResetEvent) timerParam = (currentValue, resetEvent);

                //    var timer = new Timer((param) =>
                //    {
                //        var value = (ValueTuple<double, AutoResetEvent>)param;

                //        window.Dispatcher.Invoke(() =>
                //        {
                //            window.Opacity = value.Item1;
                //        });

                //        value.Item2.Set();
                //    }, timerParam,
                //       (int)durationStep * stepCounter,
                //       Timeout.Infinite);

                //    timers.Add(timer);
                //    waitHandles.Add(resetEvent);

                //    if (isStop)
                //    {
                //        break;
                //    }
                //}

                //WaitHandle.WaitAll(waitHandles.ToArray());

                //foreach(var timer in timers)
                //{
                //    timer.Dispose();
                //}
            });
        }
    
        public static void Animate(this FrameworkElement targetElement,
                                    DependencyProperty property,
                                    double from,
                                    double to,
                                    double duration)
        {
            var elementName = targetElement.Name;

            if (string.IsNullOrEmpty(elementName))
            {
                elementName = Guid.NewGuid().ToString().Replace("-","");

                targetElement.Name = elementName;
            }

            targetElement.RegisterName(elementName, targetElement);

            var doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = from;
            doubleAnimation.To = to;
            doubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(duration));

            Storyboard.SetTargetName(doubleAnimation, elementName);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(property));

            var storyBoard = new Storyboard();
            storyBoard.Children.Add(doubleAnimation);

            storyBoard.Begin(targetElement);
        }
    }
}
