using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using VOICeVIO.SinsyGUI.Annotations;

namespace VOICeVIO.SinsyGUI
{
    public enum Language
    {
        Japanese,
        Chinese,
        English
    }

    public enum LabelType
    {
        /// <summary>
        /// Disabled
        /// </summary>
        None,
        Normal,
        WithTime,
        Mono,
    }

    public class MainModel : INotifyPropertyChanged
    {
        private string _xmlPath;

        private bool _isRemix = false;
        private string _dicPath;
        private string _htsVoicePath;
        private string _savePath;

        public string Title => IsRemix ? "Sinsy-R" : "Sinsy";

        public bool IsRemix
        {
            get => _isRemix;
            set
            {
                if (value == _isRemix) return;
                _isRemix = value;
                OnPropertyChanged(nameof(IsRemix));
                OnPropertyChanged(nameof(Title));
            }
        }

        public LabelType LabelType { get; set; } = LabelType.None;

        public Language Language { get; set; } = Language.Japanese;

        public string DicPath
        {
            get => _dicPath;
            set
            {
                if (value == _dicPath) return;
                _dicPath = value;
                OnPropertyChanged(nameof(DicPath));
            }
        }

        public string HtsVoicePath
        {
            get => _htsVoicePath;
            set
            {
                if (value == _htsVoicePath) return;
                _htsVoicePath = value;
                OnPropertyChanged(nameof(HtsVoicePath));
            }
        }

        public string SavePath
        {
            get => _savePath;
            set
            {
                if (value == _savePath) return;
                _savePath = value;
                OnPropertyChanged(nameof(SavePath));
            }
        }

        public string XmlPath
        {
            get => _xmlPath;
            set
            {
                _xmlPath = value;
                OnPropertyChanged(nameof(XmlPath));
                SavePath = Path.ChangeExtension(_xmlPath, null);
            }
        }

        public ObservableCollection<string> SavedVoicePaths { get; set; }

        public ObservableCollection<string> SavedXmlPaths { get; set; }

        public void LoadDefault()
        {
            SavedVoicePaths = new ObservableCollection<string>();
            SavedXmlPaths = new ObservableCollection<string>();
            var basePath = System.AppDomain.CurrentDomain.BaseDirectory;
            if (File.Exists(Path.Combine(basePath, "sinsy-r.exe")))
            {
                IsRemix = true;
            }
            else if (File.Exists(Path.Combine(basePath, "sinsy.exe")))
            {
                IsRemix = false;
            }
            else
            {
                MessageBox.Show("找不到Sinsy!", "错误");
            }

            var dic = Path.Combine(basePath, "dic");
            if (Directory.Exists(dic))
            {
                DicPath = dic;
            }

            foreach (var htsFile in Directory.EnumerateFiles(basePath, "*.htsvoice"))
            {
                SavedVoicePaths.Add(htsFile);
            }

            if (SavedVoicePaths.Count > 0)
            {
                HtsVoicePath = SavedVoicePaths[0];
            }

            foreach (var xml in Directory.EnumerateFiles(basePath, "*.xml"))
            {
                SavedXmlPaths.Add(xml);
            }

            if (SavedXmlPaths.Count > 0)
            {
                XmlPath = SavedXmlPaths[0];
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
