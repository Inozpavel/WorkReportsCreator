﻿using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WorkReportCreator.Views
{
    /// <summary>
    /// Элемент списка информации о файлах
    /// </summary>
    public partial class FileInformationItem : UserControl, INotifyPropertyChanged
    {
        private Visibility _textBoxVisibility = Visibility.Visible;

        private Visibility _fileNameVisibility = Visibility.Collapsed;

        private Visibility _hintVisibility;

        private string _fileName;

        private int _number;

        #region Properties

        /// <summary>
        /// Видимость блока с описанием для файла
        /// </summary>
        public Visibility TextBoxVisibility
        {
            get => _textBoxVisibility;
            set
            {
                _textBoxVisibility = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Видимость надписи "Имя файла" и надписи с именем файла
        /// </summary>
        public Visibility FileNameVisibility
        {
            get => _fileNameVisibility;
            set
            {
                _fileNameVisibility = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Видимость подсказки ""
        /// </summary>
        public Visibility HintVisibility
        {
            get => _hintVisibility;
            set
            {
                _hintVisibility = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Имя выбранного файла
        /// </summary>
        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                TextBoxVisibility = string.IsNullOrEmpty(_fileName) == false && IsSelected ? Visibility.Visible : Visibility.Collapsed;
                FileNameVisibility = string.IsNullOrEmpty(_fileName) == false ? Visibility.Visible : Visibility.Collapsed;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Номер элемента с списке
        /// </summary>
        public int Number
        {
            get => _number;
            set
            {
                _number = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Описание файла
        /// </summary>
        public string FileDescription
        {
            get { return (string)GetValue(FileDescriptionProperty); }
            set
            {
                SetValue(FileDescriptionProperty, value);
                OnPropertyChanged();
                OnPropertyToSaveChanged();
            }
        }

        /// <summary>
        /// Путь до файла
        /// </summary>
        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set
            {
                SetValue(FilePathProperty, value);
                FileName = File.Exists(value) ? Regex.Split(value, @"/|\\").Last() : null;
                HintVisibility = string.IsNullOrEmpty(FilePath) == false ? Visibility.Hidden : Visibility.Visible;
                OnPropertyChanged();
                OnPropertyToSaveChanged();
            }
        }

        /// <summary>
        /// Является ли текущий элемент выбранным
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set
            {
                SetValue(IsSelectedProperty, value);
                TextBoxVisibility = string.IsNullOrEmpty(_fileName) == false && IsSelected ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        #endregion

        #region DependencyProperties

        public static readonly DependencyProperty FileDescriptionProperty =
            DependencyProperty.Register("FileDescription", typeof(string), typeof(FileInformationItem), new PropertyMetadata("", FileDescriptionPropertyChanged));

        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register("FilePath", typeof(string), typeof(FileInformationItem), new PropertyMetadata("", FilePathPropertyChanged));


        public static readonly DependencyProperty IsSelectedProperty =
           DependencyProperty.Register("IsSelected", typeof(bool), typeof(FileInformationItem), new PropertyMetadata(false, IsSelectedPropertyChanged));

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangedEventHandler PropertyToSaveChanged;

        public FileInformationItem()
        {
            InitializeComponent();
            DataContext = this;
            TextBoxVisibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Показывает диалоговое окно для выбора файла
        /// </summary>
        private void ChooseFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Title = "Сохранение инфорации о студенте",
                Filter = "Все файлы (*.*)|*.*",
            };

            if (dialog.ShowDialog() == true)
            {
                FilePath = dialog.FileName;
            }
        }

        private static void FileDescriptionPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => (sender as FileInformationItem)
            .FileDescription = e.NewValue as string;

        private static void FilePathPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => (sender as FileInformationItem)
            .FilePath = e.NewValue as string;

        private static void IsSelectedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => (sender as FileInformationItem)
            .IsSelected = (bool)e.NewValue;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void OnPropertyToSaveChanged([CallerMemberName] string propertyName = "") => PropertyToSaveChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void ScrollViewerPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var parent = Parent as UIElement;
            var args = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton)
            {
                RoutedEvent = MouseDownEvent
            };
            parent.RaiseEvent(args);
        }

        private void ScrollViewerPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var parent = Parent as UIElement;

            var args = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = MouseWheelEvent
            };
            parent.RaiseEvent(args);
        }
    }
}
