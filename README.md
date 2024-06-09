# Приложение "DBViewer" (RU)

## Описание
> Сканер позволяет получить всю информацию о базе данных по представленному стабильному подключению или вручную через создание запроса по представленным таблицам.

## Функционал 
### Сканер базы данных
#### Принцип работы
> - по представленному подключения и предварительным настройкам сканирует объекты базы данных.

#### Инструкция по использованию
> - на представленной форме необходимо указать параметры подключения, которые позволят установить стабильное соединение для отправки запросов к базе данных. В конечном итоге будет создан json файл с информацией по объектам.

### Генератор скрипта
#### Принцип работы
> - по переданным наименованиям таблиц и предварительным настройкам сканера формирует запрос к базе данных.

#### Инструкция по использованию
> - необходимо предоставить список таблиц в специальный для этого блок по которым необходимо узнать информацию. Далее нажать сформировать скрипт.

### Сканер xlsx
#### Принцип работы
> - сканирует информацию из файла и преобразует ее в json.

#### Инструкция по использованию 
> - необходимо скопировать информацию, которая была получена в следствии использования запроса, который был сгенерирован генератором запросов и вставить её в файл формата xlsx. Далее выбрать данный файл с данными. В итоге получится json файл.

### Конвертер из json в docx
#### Принцип работы
> - конвертирует json с данными в файл docx.

#### Инструкция по использованию 
> - необходимо выбрать один из json файлов, которые были созданы до этого и имеют наименование формата - < имя >_< типбазы >_< уникальный_номер >
При верном выборе будет создан файл с отчёт по объектам базы данных с расширением docx.
> [!NOTE] Версия
> Версия 1.0
<br />
<br />

# Desktop application "DBViewer" (EN)

## Description
> The scanner allows you to get all the information about the database by the presented stable connection or manually by creating a query on the presented tables.

## Functionality
### Database Scanner
#### The principle of operation
> - scans database objects according to the provided connection and presets.

#### Instructions for use
> - on the submitted form, you must specify the connection parameters that will allow you to establish a stable connection to send requests to the database. Eventually, a json file with information about the objects will be created.

### Script Generator
#### The principle of operation
> - generates a query to the database based on the transmitted table names and pre-settings of the scanner.

#### Instructions for use
> - it is necessary to provide a list of tables in a special block for this purpose for which you need to find out information. Next, click generate script.

### xlsx scanner
#### The principle of operation
> - scans information from a file and converts it to json.

#### Instructions for use 
> - it is necessary to copy the information that was obtained as a result of using the query that was generated by the query generator and paste it into an xlsx file. Next, select this data file. As a result, you will get a json file.

### Converter from json to docx
#### The principle of operation
> - converts json with data to a docx file.

#### Instructions for use 
> - it is necessary to select one of the json files that were created before and have the name of the format - < name >_< database type >_< unique number >
If selected correctly, a file with a report on database objects with the docx extension will be created.
> [!NOTE] Version
> Version 1.0
