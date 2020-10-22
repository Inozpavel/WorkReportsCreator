﻿using System.Collections.Generic;
using System.Windows;
using WorkReportCreator.Models;
using WorkReportCreator.ViewModels;

namespace WorkReportCreator
{
    /// <summary>
    /// Окно с редактированием информации в шаблоне
    /// </summary>
    public partial class ReportsTemplate : Window
    {
        private readonly ReportsTemplateWindowViewModel _model = new ReportsTemplateWindowViewModel();

        private readonly MainWindow _mainWindow;

        public ReportsTemplate(MainWindow mainWindow)
        {
            InitializeComponent();
            DataContext = _model = new ReportsTemplateWindowViewModel();
            _mainWindow = mainWindow;
        }

        public ReportsTemplate(MainWindow mainWindow, Dictionary<string, Dictionary<string, ReportInformation>> template, string filePath)
        {
            InitializeComponent();
            DataContext = _model = new ReportsTemplateWindowViewModel(template, filePath);
            _mainWindow = mainWindow;
        }

        private void CloseApplicationClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool shouldClose = true;
            if (string.IsNullOrEmpty(_model.FilePath) && (_model.LaboratoriesWorksButtons.Count > 0 || _model.PractisesWorksButtons.Count > 0))
            {
                shouldClose = MessageBox.Show("Данные не сохранены!\nПри выходе они будут ПОТЕРЯНЫ!\nВы уверены?", "Внимание!",
                           MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes;
            }

            if (shouldClose)
                _mainWindow.Show();
            else
            {
                e.Cancel = true;
                return;
            }
        }
    }
}
