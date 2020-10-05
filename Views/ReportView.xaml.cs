﻿using System.Windows.Controls;

namespace WorkReportCreator
{
    /// <summary>
    /// Логика взаимодействия для ReportView.xaml
    /// </summary>
    public partial class ReportView : UserControl
    {
        private readonly ReportViewModel _model = new ReportViewModel();
        public ReportView()
        {
            InitializeComponent();
            DataContext = _model;
        }

        private void ChooseFile(object sender, System.Windows.RoutedEventArgs e)
        {
            int a = 2;
        }
    }
}
