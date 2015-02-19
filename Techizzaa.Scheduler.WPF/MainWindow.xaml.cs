using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Techizzaa.Scheduler.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Private Declartion

        //Web Request
        private WebClient techizzaaClient = new WebClient();

        //Custom Schedulers, will Snych with current context
        private TaskScheduler techizzaaScheduler = TaskScheduler.FromCurrentSynchronizationContext();

        //TPL Schedulers
        TechizzaaTaskSchedulers techizzaaCustomScheduler = new TechizzaaTaskSchedulers();

        #endregion

        #region Button Event

        private void ClickMeButton_Click(object sender, RoutedEventArgs e)
        {
            FirstDemoTask();
            //SecondTaskDemo();
            //TaskFaultedDemo();
        }

        #endregion

        #region First WPF Demo

        void FirstDemoTask()
        {
            ClickMeButton.IsEnabled = false;
            techizzaaClient.DownloadStringTaskAsync(@"https://www.facebook.com/").ContinueWith(task =>
            {
                textBlock.Text = task.Result;
                ClickMeButton.IsEnabled = true;
            }, techizzaaScheduler);  // remove conetxt
        }

        #endregion

        #region Second WPF Task Demo

        void SecondTaskDemo()
        {
            Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Executing First Method");
                Thread.Sleep(2000);
                Console.WriteLine("First Execution Completed");

                return TechConstants.TechizzaaTaskResult;

            }, CancellationToken.None, TaskCreationOptions.None, techizzaaCustomScheduler).ContinueWith(task =>
            {
                Console.WriteLine("Executing Second Method");
                return task.Result + TechConstants.TechizzaaTaskTwoResult;

            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, techizzaaCustomScheduler).ContinueWith((t) =>
            {
                //Run to Handle UI Thread
                textBlock.Text = t.Result;

            }, techizzaaScheduler);
        }

        #endregion

        #region Task Faulted Demo

        void TaskFaultedDemo()
        {
            ClickMeButton.IsEnabled = false;
            techizzaaClient.DownloadStringTaskAsync(@"https://www.facek.com/").ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("Yes! Exceptions");
                }
                else
                {
                    textBlock.Text = task.Result;
                    ClickMeButton.IsEnabled = true;
                }
            }, techizzaaScheduler);
        }

        #endregion
    }
}
