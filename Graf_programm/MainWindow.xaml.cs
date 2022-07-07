using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Graf_programm
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        int CurPosX = 0;
        int CurPosY = 0;
        int Mode = 0;
        int HightCounter = 0;
        int MainChildren = 4;
        int StartHight = 0;
        int StartFlag = 0;
        List<List<int>> GrafCreation = new List<List<int>>();
        List<Point> XY = new List<Point>();
        List<int> AddPath = new List<int>();
        List<int> DelPath = new List<int>();

        private void Add_Hight_Button_Click(object sender, RoutedEventArgs e)
        {
            Mode = 1;
            ModeStatus.Content = Mode;
        }

        private void Del_Hights_Button_Click(object sender, RoutedEventArgs e)
        {
            Mode = 2;
            ModeStatus.Content = Mode;
        }

        private void Add_Path_Button_Click(object sender, RoutedEventArgs e)
        {
            Mode = 3;
            ModeStatus.Content = Mode;
        }

        private void Del_Path_Button_Click(object sender, RoutedEventArgs e)
        {
            Mode = 4;
            ModeStatus.Content = Mode;
        }

        private void DeleteGraf_Height()
        {
            if (GrafSpace.Children.Count - MainChildren > 0) GrafSpace.Children.RemoveAt(GrafSpace.Children.Count - 1);
        }

        bool pressed = false;
        private void GrafSpace_MouseMove(object sender, MouseEventArgs e)
        {
            var CurPos = e.MouseDevice.GetPosition(GrafSpace);
            CurPosX = (int)CurPos.X;
            LabelX.Content = CurPosX;
            CurPosY = (int)CurPos.Y;
            LabelY.Content = CurPosY;
        }
        private void GrafSpace_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pressed = true;
        }

        private void GrafSpace_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pressed = false;
            if (Mode == 1)
            {
                CreateGrafPoint();
            }
        }

        private void CreateGrafPoint()
        {
            GrafCreation.Add(new List<int>());
            XY.Add(new Point(CurPosX - 20, CurPosY - 20));
            HightCounter += 1;
            var button = new Button
            {
                Content = HightCounter,
                Height = 40,
                Width = 40,
                Margin = new Thickness(CurPosX - 20, CurPosY - 20, 0, 0),
                Style = FindResource("Graf_Hights") as Style,
            };
            button.Click += GrafHight_Click;
            GrafSpace.Children.Add(button);
        }

        private void GrafHight_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            if (sender is Button button)
            {
                i = (int)button.Content;
            }
            if (Mode == 2)
            {
                GrafCreation.RemoveAt(i - 1);
                XY.RemoveAt(i - 1);
                HightCounter -= 1;
                for (int k = 0; k < GrafCreation.Count; k++)
                {
                    /*for (int j = 0; j < GrafCreation[k].Count; j++)
                    {
                        if(GrafCreation[k][j] == i)
                        {
                            GrafCreation[k].RemoveAt(j);
                            j -= 1;
                        }
                    }*/
                    while(GrafCreation[k].Remove(i));
                }
                for (int k = 0; k < GrafCreation.Count; k++)
                {
                    for (int j = 0; j < GrafCreation[k].Count; j++)
                    {
                        if(GrafCreation[k][j] > i) GrafCreation[k][j] -= 1;
                    }
                }
                while (GrafSpace.Children.Count != MainChildren)
                {
                    GrafSpace.Children.RemoveAt(GrafSpace.Children.Count - 1);
                }
                Path_Creation();
                Graf_Creation();
                /*if (GrafSpace.Children.Count - 5 >= 0)
                {
                    GrafSpace.Children.RemoveAt(GrafSpace.Children.Count - ButtonCounter + i - 1);
                    ButtonCounter -= 1;
                }*/
            }
            if(Mode != 3)
            {
                AddPath.Clear();
            }
            if(Mode == 3)
            {
                AddPath.Add(i);
                if (AddPath.Count == 2)
                {
                    Path_Add(AddPath);
                    /*Line x = new Line();
                    x.X1 = XY[AddPath[0] - 1].X + 15;
                    x.X2 = XY[AddPath[1] - 1].X + 15;
                    x.Y1 = XY[AddPath[0] - 1].Y + 15;
                    x.Y2 = XY[AddPath[1] - 1].Y + 15;
                    x.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                    x.StrokeThickness = 4;
                    GrafSpace.Children.Add(x);*/
                    while (GrafSpace.Children.Count != MainChildren)
                    {
                        GrafSpace.Children.RemoveAt(GrafSpace.Children.Count - 1);
                    }
                    Path_Creation();
                    Graf_Creation();
                    AddPath.Clear();
                }
            }
            if(Mode != 4)
            {
                DelPath.Clear();
            }
            if (Mode == 4)
            {
                DelPath.Add(i);
                if (DelPath.Count == 2)
                {
                    Path_Del(DelPath);
                    
                    while (GrafSpace.Children.Count != MainChildren)
                    {
                        GrafSpace.Children.RemoveAt(GrafSpace.Children.Count - 1);
                    }
                    Path_Creation();
                    Graf_Creation();
                    DelPath.Clear();
                }
            }
            if (Mode == 5)
            {
                StartHight = i;
                Width_Search(sender, e);
            }
            if (Mode == 6)
            {
                StartHight = i;
                Depth_Search(sender, e);
            }
        }

        private void Path_Add(List<int> PathList)
        {
            GrafCreation[PathList[0] - 1].Add(PathList[1]);
            GrafCreation[PathList[1] - 1].Add(PathList[0]);
        }

        private void Path_Del(List<int> PathList)
        {
            while(GrafCreation[PathList[0] - 1].Remove(PathList[1]));
            while (GrafCreation[PathList[1] - 1].Remove(PathList[0]));
        }

        private void Path_Creation()
        {
            for (int i = 0; i < GrafCreation.Count; i++)
            {
                for (int j = 0; j < GrafCreation[i].Count; j++)
                {
                    Line x = new Line();
                    x.X1 = XY[i].X + 15;
                    x.X2 = XY[GrafCreation[i][j] - 1].X + 15;
                    x.Y1 = XY[i].Y + 15;
                    x.Y2 = XY[GrafCreation[i][j] - 1].Y + 15;
                    x.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                    x.StrokeThickness = 4;
                    GrafSpace.Children.Add(x);
                }
            }
        }

        private void Graf_Creation()
        {
            /*Style buttonStyle = new Style();
            buttonStyle = Graf_Hights;*/
            for (int i = 0; i < GrafCreation.Count; i++)
            {
                var button = new Button
                {
                    Content = i + 1,
                    Height = 40,
                    Width = 40,
                    Margin = new Thickness(XY[i].X, XY[i].Y, 0, 0),
                    Style = FindResource("Graf_Hights") as Style,
                };
                button.Click += GrafHight_Click;
                GrafSpace.Children.Add(button);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Tablet__Button_Click(object sender, RoutedEventArgs e)
        {
            int[][] message = new int[HightCounter][];
            string messageBoxText = "";
            for (int i = 0; i < HightCounter; i++)
            {
                message[i] = new int[HightCounter];
            }
            for (int i = 0; i < HightCounter; i++)
            {
                for (int j = 0; j < HightCounter; j++)
                {
                    message[i][j] = 0;
                }
            }

            for (int i = 0; i < GrafCreation.Count; i++)
            {
                for (int j = 0; j < GrafCreation[i].Count; j++)
                {
                    message[i][GrafCreation[i][j] - 1] = 1;
                    message[GrafCreation[i][j] - 1][i] = 1;
                }
            }

            messageBoxText += "   ";
            for (int i = 0; i < HightCounter; i++)
            {
                messageBoxText += (i + 1);
                if (i != HightCounter - 1)
                {
                    messageBoxText += " ";
                }
            }
            messageBoxText += "\n";

            for (int i = 0; i < HightCounter; i++)
            {
                messageBoxText += (i + 1) + " ";
                for (int j = 0; j < HightCounter; j++)
                {
                    messageBoxText += message[i][j];
                    if (j != HightCounter - 1)
                    {
                        messageBoxText += "|";
                    }
                    if (j == HightCounter - 1)
                    {
                        messageBoxText += "\n";
                    }
                }
                /*if (i != HightCounter - 1)
                {
                    messageBoxText += "   ";
                    for (int k = 0; k < HightCounter; k++)
                    {
                        messageBoxText += "-";
                        if (k != HightCounter - 1)
                        {
                            messageBoxText += "+";
                        }
                    }
                    messageBoxText += "\n";
                }*/
            }
            string caption = "GraphSpace";
            MessageBoxButton button = MessageBoxButton.OK;
            //MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, MessageBoxImage.None, MessageBoxResult.Yes);
        }

        private void List_Button_Click(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "";
            for (int i = 0; i < GrafCreation.Count; i++)
            {
                messageBoxText += i + 1;
                messageBoxText += ":";
                for (int j = 0; j < GrafCreation[i].Count; j++)
                {
                    messageBoxText += " ";
                    messageBoxText += GrafCreation[i][j];
                    messageBoxText += " ";
                }
                messageBoxText += "\n";
            }
            string caption = "GraphSpace";
            MessageBoxButton button = MessageBoxButton.OK;
            //MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, MessageBoxImage.None, MessageBoxResult.Yes);
        }

        public void Width_Button_Click(object sender, RoutedEventArgs e)
        {
            Mode = 5;
            ModeStatus.Content = Mode;
        }

        async public void Width_Search(object sender, RoutedEventArgs e)
        {
            List<int> GrafStatus = new List<int>();
            List<int> Heap = new List<int>();
            for (int i = 0; i < HightCounter; i++)
            {
                GrafStatus.Add(0);
            }
            Heap.Add(StartHight - 1);
            Create_Hightlighted_Button(Heap[0]);
            await Task.Delay(750);
            GrafStatus[StartHight - 1] = 1;
            while (Heap.Count != 0)
            {
                while (GrafStatus[Heap[0]] == 2)
                {
                    Heap.RemoveAt(0);
                    if (Heap.Count == 0)
                    {
                        break;
                    }
                }
                if (Heap.Count == 0)
                {
                    break;
                }
                int Cur = Heap[0];
                for (int j = 0; j < GrafCreation[Cur].Count; j++)
                {
                    if (GrafStatus[GrafCreation[Cur][j] - 1] == 0)
                    {
                        //Heap.Insert(0, GrafCreation[Cur][j] - 1);
                        Heap.Add(GrafCreation[Cur][j] - 1);
                        Create_Hightlighted_Button(GrafCreation[Cur][j] - 1);
                        await Task.Delay(750);
                        GrafStatus[GrafCreation[Cur][j] - 1] = 1;
                    }
                }
                GrafStatus[Cur] = 2;
            }
            await Task.Delay(1000);
            Path_Creation();
            Graf_Creation();
        }

        public void Create_Hightlighted_Button(int i)
        {
            var button = new Button
            {
                Content = i + 1,
                Height = 40,
                Width = 40,
                Margin = new Thickness(XY[i].X, XY[i].Y, 0, 0),
                Style = FindResource("Graf_Hights_Hightlighted") as Style,
            };
            button.Click += GrafHight_Click;
            button.Background = Brushes.Red;
            GrafSpace.Children.Insert(GrafSpace.Children.Count - (HightCounter - i), button);
            GrafSpace.Children.RemoveAt(GrafSpace.Children.Count - (HightCounter - i));
        }

        public void Depth_Button_Click(object sender, RoutedEventArgs e)
        {
            Mode = 6;
            ModeStatus.Content = Mode;
        }

        async public void Depth_Search(object sender, RoutedEventArgs e)
        {
            List<int> GrafStatus = new List<int>();
            List<int> Heap = new List<int>();
            for (int i = 0; i < HightCounter; i++)
            {
                GrafStatus.Add(0);
            }
            Heap.Add(StartHight - 1);
            Create_Hightlighted_Button(Heap[0]);
            await Task.Delay(750);
            GrafStatus[StartHight - 1] = 1;
            while (Heap.Count != 0)
            {
                while (GrafStatus[Heap[0]] == 2)
                {
                    Heap.RemoveAt(0);
                    if (Heap.Count == 0)
                    {
                        break;
                    }
                }
                if (Heap.Count == 0)
                {
                    break;
                }
                int Cur = Heap[0];
                int flag = 0;
                for (int j = 0; j < GrafCreation[Cur].Count; j++)
                {
                    if (GrafStatus[GrafCreation[Cur][j] - 1] == 0)
                    {
                        Heap.Insert(0, GrafCreation[Cur][j] - 1);
                        //Heap.Add(GrafCreation[Cur][j] - 1);
                        Create_Hightlighted_Button(GrafCreation[Cur][j] - 1);
                        await Task.Delay(750);
                        GrafStatus[GrafCreation[Cur][j] - 1] = 1;
                        flag = 1;
                        break;
                    }
                }
                if (flag == 0) GrafStatus[Cur] = 2;
            }
            await Task.Delay(1000);
            Path_Creation();
            Graf_Creation();
        }
    }
}
