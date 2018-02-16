using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro;
using MahApps.Metro.Controls;
using Microsoft.Win32;

namespace VOICeVIO.SinsyGUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private MainModel _mainModel;
        public MainWindow()
        {
            _mainModel = new MainModel();
            _mainModel.LoadDefault();
            this.DataContext = _mainModel;
            InitializeComponent();
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(Application.Current,
                ThemeManager.Accents.FirstOrDefault(accent => accent.Name == "Crimson"), theme.Item1);


        }

        private void Synth_Click(object sender, RoutedEventArgs e)
        {
            Synth(false);
        }

        private void Synth(bool outputLabel = false)
        {
            var savePath = _mainModel.SavePath + (outputLabel ? ".txt" : ".wav");
            var inputName = Path.GetFileName(_mainModel.XmlPath);
            var outputName = Path.GetFileName(savePath);
            TranState.Content = "合成中:" + inputName;
            MProgress.IsIndeterminate = true;
            MProgress.Visibility = Visibility.Visible;
            var exe = _mainModel.IsRemix ? "sinsy-r.exe" : "sinsy.exe";
            var cmd =
                $"-w {_mainModel.Language.ToParamString()} {(string.IsNullOrWhiteSpace(_mainModel.DicPath) ? "" : "-x \"" + _mainModel.DicPath + "\"")} -m \"{_mainModel.HtsVoicePath}\" -o \"{savePath}\" {(outputLabel ? "-l " + _mainModel.LabelType.ToParamString() : "")} \"{_mainModel.XmlPath}\"";
            Console.WriteLine(cmd);
            ProcessStartInfo processStartInfo =
                new ProcessStartInfo(exe, cmd) { WindowStyle = ProcessWindowStyle.Hidden };
            //processStartInfo.CreateNoWindow = true;
            var p = new Process
            {
                StartInfo = processStartInfo,
                EnableRaisingEvents = true
            };
            p.Exited += (o, args) =>
            {
                if (p.ExitCode == 0)
                {
                    this.Invoke(() =>
                    {
                        MProgress.Visibility = Visibility.Hidden;
                        TranState.Content = "已完成:" + outputName;
                    });
                }
                else
                {
                    this.Invoke(() =>
                    {
                        MProgress.Visibility = Visibility.Hidden;
                        TranState.Content = "合成失败:" + outputName;
                    });
                }
            };
            p.Start();
        }

        private void OutputLabel_Click(object sender, RoutedEventArgs e)
        {
            Synth(true);
        }


        private void OpenFile(object sender, ExecutedRoutedEventArgs e)
        {
            switch (e.Parameter.ToString())
            {
                case "HtsVoicePath":
                    OpenFileDialog dialog = new OpenFileDialog();
                    dialog.Multiselect = false;
                    dialog.Filter = "HTS Voice File(*.htsvoice;*.tsvoice)|*.htsvoice;*.tsvoice|All Files|*.*";
                    if (dialog.ShowDialog() == true)
                    {
                        _mainModel.HtsVoicePath = dialog.FileName;
                    }
                    break;
                case "SavePath":
                    SaveFileDialog dialog2 = new SaveFileDialog();
                    dialog2.Filter = "Wave File(*.wav)|*.wav|Text File(*.txt)|*.txt";
                    if (dialog2.ShowDialog() == true)
                    {
                        _mainModel.SavePath = dialog2.FileName;
                    }
                    break;
                case "XmlPath":
                    OpenFileDialog dialog3 = new OpenFileDialog();
                    dialog3.Multiselect = false;
                    dialog3.Filter = "MusicXML File(*.xml)|*.xml|All Files|*.*";
                    if (dialog3.ShowDialog() == true)
                    {
                        _mainModel.XmlPath = dialog3.FileName;
                    }
                    break;
                case "DicPath":
                    //摸了！
                    break;
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            _mainModel.LoadDefault();
        }
    }
}
