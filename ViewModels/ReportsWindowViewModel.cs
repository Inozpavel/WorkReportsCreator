﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using WorkReportCreator.Models;
using WorkReportCreator.Views;

namespace WorkReportCreator.ViewModels
{
    internal class ReportsWindowViewModel : INotifyPropertyChanged
    {
        private int? _selectedIndex;

        /// <summary>
        /// Список всех вкладок
        /// </summary>
        public ObservableCollection<TabItem> TabItems { get; set; } = new ObservableCollection<TabItem>();

        /// <summary>
        /// Идекс текущей выбранной вкладки
        /// </summary>
        public int? SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Вызывается при нажатии кнопки назад
        /// </summary>
        public event Action<object> ButtonBackClicked;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <param name="laboratoryWorks">Список доступный для выбора лабораторных работ</param>
        /// <param name="practicalWorks">Список доступный для выбора приктических работ</param>
        /// <param name="reportsWindow">Окно, на котором расположен элемент</param>
        /// <exception cref="Exception"/>
        public ReportsWindowViewModel(List<string> laboratoryWorks, List<string> practicalWorks, ReportsWindow reportsWindow)
        {
            FastActionsItem fastActionsItem = new FastActionsItem
            {
                IsButtonEnabled = laboratoryWorks.Count > 0 || practicalWorks.Count > 0
            };

            fastActionsItem.ButtonGenerateAllClicked += GenerateAllReports;
            fastActionsItem.ButtonBackClicked += (sender) => ButtonBackClicked?.Invoke(sender);

            ResourceDictionary tabItemStyle = new ResourceDictionary() { Source = new Uri("./Views/Styles/CommonTabItemStyle.xaml", UriKind.Relative) };

            TabItems.Add(new TabItem() { Header = "Быстрые действия", Content = fastActionsItem, Style = tabItemStyle["CommonTabItemStyle"] as Style });

            Dictionary<string, Dictionary<string, ReportInformation>> template;
            MainParams mainParams = new MainParams();
            try
            {
                template = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, ReportInformation>>>(File.ReadAllText(mainParams.CurrentTemplateFilePath));
            }
            catch (Exception)
            {
                throw new Exception("Не получилось загрузить данные из шаблона!");
            }

            if (template.Keys.Contains("Laboratories") == false && template.Keys.Contains("Practices") == false)
                throw new Exception("В файле с шаблоном отсутствуют и практические и лабораторные работы!");

            LoadTemplateInfoFromKey(template, reportsWindow, "Practices", "пр.", practicalWorks, tabItemStyle["CommonTabItemStyle"] as Style);
            LoadTemplateInfoFromKey(template, reportsWindow, "Laboratories", "лаб.", laboratoryWorks, tabItemStyle["CommonTabItemStyle"] as Style);

            SelectedIndex = 0;
            OnPropertyChanged();
        }

        private void GenerateAllReports(object obj)
        {
            List<int> failedPracticesReports = new List<int>();
            List<int> failedLaboratoriesReports = new List<int>();

            for (int i = 1; i < TabItems.Count; i++)
            {
                string name = TabItems[i].Header as string;
                ReportItem item = TabItems[i].Content as ReportItem;
                try
                {
                    item.GenerateReport();
                }
                catch (Exception)
                {
                    List<int> list = Regex.IsMatch(name, "пр|Пр") ? failedPracticesReports : failedLaboratoriesReports;
                    list.Add(i);
                }
            }

            if (failedLaboratoriesReports.Count == 0 && failedPracticesReports.Count == 0)
            {
                if (MessageBox.Show($"Все отчеты успешно созданы!\nОткрыть папку с отчетами?", "Поздравляю!",
               MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    MainParams mainParams = new MainParams();
                    if (Directory.Exists(mainParams.AllReportsPath))
                        Process.Start(Directory.GetCurrentDirectory() + mainParams.AllReportsPath);
                    else
                        MessageBox.Show("Не получилось найти папки с отчетами!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else if (failedPracticesReports.Count == 0 && failedLaboratoriesReports.Count >= 0)
            {
                MessageBox.Show($"Все отчеты для практических работ успешно созданы!\n" +
                    $"При создании отчетов для лабораторных работ произошли ошибки:\n" +
                    string.Join(" ", failedLaboratoriesReports.Take(10)) + (failedLaboratoriesReports.Count > 10 ? "..." : ""),
                    "Поздравляю!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (failedLaboratoriesReports.Count > 0 && failedPracticesReports.Count == 0)
            {
                MessageBox.Show($"Все отчеты для лабораторных работ успешно созданы!\n" +
                    $"При создании отчетов для практических работ произошли ошибки:\n" +
                    string.Join(" ", failedPracticesReports.Take(10)) + (failedPracticesReports.Count > 10 ? "..." : ""),
                    "Поздравляю!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                MessageBox.Show($"При создании отчетов произошли ошибки!\n" +
                    $"Практические работы:\n{string.Join(" ", failedPracticesReports.Take(10)) + (failedPracticesReports.Count > 10 ? "..." : "")}" +
                    $"\nЛабораторные работы:\n{string.Join(" ", failedLaboratoriesReports.Take(10)) + (failedLaboratoriesReports.Count > 10 ? "..." : "")}",
                    "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Добавляет новые вкладки для всех выбранных работ
        /// </summary>
        /// <param name="template">Шаблон для работ</param>
        /// <param name="window">Окно со всеми выбранными работами</param>
        /// <param name="workType">Тип работы</param>
        /// <param name="shortDescription">Аббревеатура типа работы</param>
        /// <param name="selectedWorks">Выбранные пользователем работы</param>
        /// <param name="style">Стиль для TabItem</param>
        private void LoadTemplateInfoFromKey(Dictionary<string, Dictionary<string, ReportInformation>> template, ReportsWindow window,
            string workType, string shortDescription, List<string> selectedWorks, Style style)
        {
            if (template.Keys.Contains(workType) == false)
                return;


            foreach (string number in template[workType].Keys.Where(x => selectedWorks.Contains(x)))
            {
                try
                {
                    List<string> dynamicTasks = template[workType][number].DynamicTasks.Select(x => x.Description).Select(x => Regex.Replace(x, "\\n", "").Trim()).ToList();
                    TabItems.Add(new TabItem()
                    {
                        Header = $"{number} {shortDescription}",
                        Content = new ReportItem(window, dynamicTasks),
                        Style = style,
                    });
                }
                catch (Exception)
                {
                    MessageBox.Show($"Не получилось загрузить {number} {shortDescription} работу,\nвозможно, ошибка в ключах.",
                        $"Ошибка при загрузке работы!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
