# IMRY: I'll Make Reports for You

![](https://camo.githubusercontent.com/6e386aa932b31a8f5281f92f93d6c210569afc13/68747470733a2f2f6170692e6369727275732d63692e636f6d2f6769746875622f686967616e2d656d752f686967616e2e7376673f7461736b3d77696e646f77732d7838365f36342d62696e6172696573)
## Описание проекта
IMRY это программа для автоматического создания Word отчетов. Отчеты создаются из шаблонов и конфигурационных файлов.
Шаблон - набор лабораторных и практических работ, в которых указаны название, цель, теоретическая часть, общее задание и индивидуальные задания, выводы.
 
## Установка
### [`>>> СКАЧАТЬ <<<`](InstallationFiles/Installer/Installer-SetupFiles/Installer.exe?raw=true)
Для установки программы можно скачать из папки **```InstallationFiles/Installer/Installer-SetupFiles```** файл **```Installer.exe```**, или просто нажать на большую кнопку сверху. При открытии файла запустится стандартный установщик, нужно будет проследовать по всем шагам. После установки программма будет отображаться в списке установленных программ, появится иконка на рабочем столе и в меню «Пуск».

### Обновление программы
При выходе новой версии, нужно скачать ее и запустить установщик. Он автоматически обновит файлы до новой версии. 
**Программу удалять не нужно**
## Запуск программы
Чтобы запустить программу, можно воспользоваться ярлыком на рабочем столе или в меню «Пуск».

---
## Реализованный функционал
- [x] Создание отчетов
- [x] Создание, сохранение, изменение шаблонов для отчетов
- [x] Сохранение и загрузка информации о студенте
- [x] Переход к папке с отчетами после создания отчета
- [x] Кнопка для быстрого перехода к репозиторию для обновления
- [x] Окно с настройками для изменения главного конфигурационного файла
- [x] Возможность сохранять и загружать введенные данные при генерации отчета
- [x] Автозагрузка сохраненных данных
---
## В разработке
- [ ] Возможность создавать отчеты для разных предметов
- [ ] Возможность выбирать расширения файлов для добавления при использовании Drag & Drop для каждой работы
---
## Удаление
Для удаления программы можно воспользоваться любым деинсталлятором. Также можно в меню «Пуск» нажать правой кнопкой по иконке приложения, затем "Удалить".
---
---
## Конфигурирование работы приложения
### *Работу программы можно конфигурировать, редактируя конфигурационные файлы, изменяя ряд параметров.*
### Описание конфигурационных файлов
Для работы программы **нужен главный конфиругационный файл** в папке приложения, который содержит следующие параметры:
#### Параметры, которые хранят значение
| Параметр                   | Тип данных | Описание параметра                                                |
| -------------------------- | --------   | ------------------------------------------------------            |
|ShortSubjectName            | string     | Расширение, которое используется для сохранения и загрузки данных |
|WorkHasTitlePage            | bool       | Есть ли в работе титульная страница                               |
|WorkHasTitlePageParams      | bool       | Есть ли в работе в титульнике какие-то параметры                  |

#### Параметры, которые хранят путь к файлу
| Параметр                   | Тип данных | Описание параметра                 | Тип данных в файле|
| -------------------------- | -------- | ------------------------------------------------------ | --------------------------------|
|WorkTitlePageFilePath       | string   | Путь до файла с титульником (при наличии)              | ```Dictionary<string, string>``` |
|WorkTitlePageParamsFilePath | string   | Путь до файл с параметрами для титульника (при наличии)| ```List<string>```              |
|PermittedDragAndDropExtentionsFilePath | string | Разрешенные расширения файлов при использовании Drag & Drop | ```List<string>```
|CurrentTemplateFilePath     | string   | Текущий шаблон для работ | ```Dictionary<string, Dictionary<string, ReportInformation>>``` ( лучше создавать в программе в пунке меню "Создать шаблон" ) |
|UserDataFilePath            | string   | Путь до файла с информацией о пользователе | ```StudentInformation``` ( создается программой ) |

#### Параметры, которые хранят путь к папке
| Параметр         | Тип данных | Описание параметра |
| ---------------- | --------   | ------------------ |
| ReportsPath      | string     | Путь, где будут сохраняться сгенерированные отчеты |
| SavedReportsPath | string     | Путь, где все отчеты будут сохраняться все введенные данные в отчеты |
### Пример главного конфигурационного файла
```json
{
  "ShortSubjectName": "java",
  "WorkHasTitlePage": true,
  "WorkTitlePageFilePath": "./Subjects/Java/TitlePage.docx",
  "WorkHasTitlePageParams": true,
  "WorkTitlePageParamsFilePath": "./Subjects/Java/TitlePageParams.params.json",
  "PermittedDragAndDropExtentionsFilePath": "./Subjects/Java/PermittedDragAndDropExtentions.extensions.json",
  "CurrentTemplateFilePath": "./Subjects/Java/JavaTasks.template.json",
  "ReportsPath": "./Reports/Java",
  "SavedReportsPath": "./SavedReports/Java"
}
```
---
#### При использовании Drag & Drop, программа фильтрует файлы, список разрешенных расширения вынесен в отдельный конфигурационный файл.
### **Если добавить символ \*, программа добавит все файлы без фильтрации**
#### Пример файла с разрешенными расширениями файлов для Drag & Drop
```json
[
    "срр",
    "h",
    "cs",
    "java",
    "py",
    "asm",
    "png",
    "bmp",
    "jpg",
    "jpeg",
    "*"
]
```
---
#### В пустом документе ( *.doc, *.docx ) можно указать ряд параметров, которые будут заменены на значения
## **!!! Все параметры должны быть вставлены в документ формате {{НазваниеПараметра}}**
---
### Таблица параметров

| Параметр         | Описание                                       | На что будет заменен                             |
| ---------------- | ---------------------------------------------- | ------------------------------------------------ |
| UserName         | ФИО пользователя (полное или инициалы)         | Строка (значение формируется из введенных данных |
| WorkType         | Тип работы                                     | "Практическая работа" / "Лабораторная работа"    |
| WorkNumber       | Номер работы                                   | Число                                            |
| WorkName         | Полное название работы                         | Строка ( значение берется из шаблона )           |
| TheoryPart       | Теоретическая часть работы                     | Строка ( значение берется из шаблона )           |
| WorkTarget       | Цель работы                                    | Строка ( значение берется из шаблона )           |
| Conclusions      | Выводы работы                                  | Строка ( значение берется из шаблона )           |
| CommonTask       | Общее описание работы                          | Один или несколько абзацев                       |
| DynamicTasks     | Выбранные задания в работе                     | Один или несколько абзацев                       |
| UserFiles        | Файлы, добавленные пользователем               | Один или несколько абзацев                       |

---
#### Для титульной страницы существует отдельный конфигурационный файл, в котором можно указать любое количество параметров, в этом файле необходимо указать название параметра и его значение.
#### Пример конфигурационного файла для титульной страницы
```json
{
    "DesciplineName": "«Программирование на Java»",
    "TeacherName": "Фамилия И. О.",
    "Departament": "преподаватель кафеды",
    "City": "Москва",
    "Year": "2020"
}
```

## Добавление \ изменение работ
Для добавления новых работ в программе в главном меню есть отдельньные кнопки "Создатm шаблон" и "Изменить шаблон"
### При заполнении шаблонов, можно также вставить изображения, изображение вставляется в виде ```{{image source="Относительный/абсолютный путь", name="Подпись картинки (можно не указывать)"}}```. Картинки нумеруются автоматически. 


