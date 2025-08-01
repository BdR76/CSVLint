CSV Lint plug-in documentation
==============================

**CSV Lint** is a plug-in for Notepad++ to work with [comma-separated values](https://en.wikipedia.org/wiki/Comma-separated_values)
(csv) and fixed width data files.


* [CSV Lint plug-in](https://github.com/BdR76/CSVLint/)
* [Notepad++ homepage](http://notepad-plus-plus.org/)

![preview screenshot](../csvlint_preview.png?raw=true "CSVLint plug-in preview")

Use the **CSV Lint** plug-in to quickly and easily inspect csv data files,
apply syntax highlighting to columns, detect technical errors and fix datetime
and decimal formatting. It's not meant as a replacement for a spreadsheet
program, but rather it's a quality control tool to examine, verify or polish up
a dataset before further processing.

First install and open Notepad++, then go to the menu item `Plugins > Plugins Admin...`,
search for "csv lint", check the checkbox and press Install. This will add
CSV Lint under the `Plugins > CSV Lint` menu item and a CSV Lint icon in the
toolbar icon.

CSV Lint doesn't require an internet connection and doesn't use any cloud service.
All data processing is done offline on the pc that runs Notepad++.

**If you find the CSV Lint plug-in useful you can buy me a coffee!** ☕  
[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/donate/?hosted_button_id=T8QZSFBNAPERL)

Syntax highlighting colors
--------------------------
Syntax highlighting will make it easier to see columns in the data files.
Note that the rendering of the syntax highlighting runs on a separate thread
in the background. This means that for larger files (~50MB or more) it can
happen that the beginning of the file displays column colors but at the end of the
file it's still uncolored.

There are four pre-defined color schemes you can select using the
`Highlighting` button on the `Plugins > CSV Lint > Setttings` dialog.
At first time startup, the plug-in will select light or darkmode color scheme,
depending on the Dark Mode setting in the Notepad++ `config.xml`.

![CSV Lint color styles for syntax highlighting](/docs/csvlint_color_styles.png?raw=true "CSV Lint plug-in color styles for syntax highlighting")

These predefined color sets have been [carefully selected](https://github.com/BdR76/CSVLint/tree/master/extra#generate_colorspy)
so that each column color is as different as possible from the next.
This makes it easier to distinguish columns and also makes it more accessible
for people who are [color blind](https://github.com/BdR76/CSVLint/tree/master/extra/colorblind_vision1.png).
The color scheme settings are stored in a file `CSVLint.xml` which is
automatically created at first time startup or when the file is missing,
also see an example [CSVLint.xml file here](https://github.com/BdR76/CSVLint/blob/master/extra/).

	%USERPROFILE%\AppData\Roaming\Notepad++\plugins\config\CSVLint.xml

You can also change the colors in the "Style Configurator dialog", see menu item
`Settings > Style configurator... > Language: CSV Lint`. If you have a CSV file
open you'll immediately see the changes as you edit them. Note that selecting a
default color scheme in `Setttings` will overwrite any changes made in
the Style Configurator dialog.

### Default extension ###

By default the plugin applies syntax highlighting to files with the extension
`.csv`. If you also want this for files with extension `.ssv` or `.tsv` then
you can change this in the Notepad++ settings.
Go to `Settings > Style Configuration` and in the Language dropdown select
`CSV Linter and validator`. Then go to the textbox "User ext." and add `ssv`
and click the "Save & Close" button. If you want to add more than one
extension then separate them by space, so for example `ssv tsv skv`.

CSV Lint window
---------------
Click the toolbar icon or select the menu item `Plugins > CSVLint > CSV Lint window`
to toggle the CSV Lint window visible.

![CSV Lint window screenshot](/docs/csvlint_main_window.png?raw=true "CSV Lint plug-in main window")

In the CSV Lint window on the left side, the metadata of the current file is
displayed. This contains information about the file, which separator is used
and if it has column headers, as well as all the definitions for each column,
such as datatype and width.

The metadata is very important for the other functionality to work. When the
content of the file and the metadata get out-of-sync, for example when editing
the data file, the plug-in will detect the columns incorrectly. This can lead
to unexpected results when reformatting or validating the data.

The metadata is based on the `schema.ini` format and it is important for the
edit options in the plug-in to work correctly. You can press "Detect columns"
to automatically detect the metadata from the datafile, and/or manually
edit the metadata in the textbox. When manually editing the metadata, always
press the save icon (blue disk) to apply it before continuing.

### Detect columns ###

Press the "Detect columns" button to auto-detect column types from the
currently active file. The auto-detection function will try to infer the
column separator character and column data types by looking at the data.
When a file is opened and no schema.ini is found then
this auto-detection feature will also run once by default.

When a file is opened the plug-in will:

1) check for a `schema.ini` file in same folder as data file
2) check if the `schema.ini` contains a section for the filename
2) if no `schema.ini` or section found, then try to auto-detect metadata

Note; if "Detect columns" cannot automatically detect any columns, then
the metadata definition will look like this:

    ; *********************************
    ; Unable to detect column separator
    ; *********************************
    Format=FixedLength
    ColNameHeader=False
    Col1=Textfile Text Width 9999

In order for automatic column detection to work, the data should have
the same amount of separator characters on each line, and the separator
should be one of the characters specified in the `Separators` setting,
see the [Settings screen](#Settings). For fixed width data files, each
line should have the same length, and columns are detected by looking where
numeric, non-numeric and space characters start or end.

### Detect columns manually ###

You can manually override the auto-detection by unchecking the auto-detect
checkbox. When the auto-detect is not checked and you press "Detect columns",
a dialog will appear where you can manually enter the properties of the datafile.

![CSV Lint detect columns manually dialog](/docs/csvlint_detect_columns.png?raw=true "CSV Lint plug-in detect columns manually dialog")

Here you can enter the column separator character or Fixed Width.
When selecting "Fixed Width" you can optionally provide a comma separated
list of the column ending positions in "Column end positions". For example
the text data `2025-10-15HbA1c 123.5` has column end positions `10, 16, 21`.
The plug-in expects the column character positions, but if you instead enter
the individual column widths, so `10, 6, 5` in this example, that will also
work in most cases. Leave "Fixed positions" empty and the plug-in will
try to detect fixed width columns same as auto-detect.

You can select the "Skip lines" option and the column detection process will skip the first X lines of the file.
For example when the first line of data (including header names) starts on
line `16` then you can set "Skip lines" to `15`.
Note that the `SkipLines` keyword is not officially part of the schema.ini format
(please upvote [the suggestion here](https://feedback.azure.com/d365community/search/?q=schema.ini)),
so applications that use the ODBC Text driver will ignore this setting.

You can also indicate a comment character, any lines starting with this character will be skipped.

### Save icon ###

Click this blue-disk to save the metadata after editing the metadata manually
to apply the changes. This will also save the metadata to a `schema.ini` file
in the same folder as the data file. The next time you open the datafile with
the plug-in, it will automatically load the metadata from this file.

The file and column metadata will be saved under a section with the filename.
A `schema.ini` file can contain the meta data for more
than one data file, using a separate section for each file.

### Toggle syntax highlighting ###

Click the colored "Aa" button to toggle syntax highlighting on and off.
This does the same as selecting the menu items `Language > CSVLint` and
`Language > None (Normal Text)`.

### Sort / Add column / Reformat / Validate ###

The **Sort data**, **Add column**, **Reformat** and **Validate** buttons are explained
elsewhere in this document. See separate paragraphs for more information.

Schema.ini
----------
The metadata uses the standard schema.ini format, see
[documentation](https://docs.microsoft.com/en-us/sql/odbc/microsoft/schema-ini-file-text-file-driver?view=sql-server-ver15),
see schema.ini example below:

    [mydata.csv]
    Format=CSVDelimited
    DateTimeFormat=dd-mm-yyyy
    DecimalSymbol=.
    NumberDigits=2
    Col1=OrderId Integer Width 8
    Col2=Quantity Integer Width 3
    Col3=PartName Text Width 50
    Col4=OrderDate DateTime Width 10

Format can be CSVDelimited for commas, TabDelimited for tabs, for any other
delimiter use for example Format=Delimited(;). Use FixedLength for fixed
width text files and set the Width for each column.

DateTimeFormat is not case sensitive and uses `dd/mm/yyyy` or `yyyy-mm-dd hh:nn:ss` etc.

DecimalSymbol can be either `.` or `,` and CSV Lint will assume the thousand
separators symbol is the opposite of the DecimalSymbol. Define the maximum
decimals digits for example `NumberDigits=2` for values like "1.23" or
"-45.67" etc.

### Comment lines

Some data files contain comments or documentation about the data.
CSV Lint supports a `SkipLines` option to skip at start of the file, and a `CommentChar` option to skip lines thst start with this character.

Note that the `SkipLines` and `CommentChar` keywords are not officially part of the schema.ini format,
(please upvote [the suggestion here](https://feedback.azure.com/d365community/search/?q=schema.ini)),
so applications that use the ODBC Text driver will ignore this setting.

### Enumeration

Enumeration, or coded values, is when a column may only contain a certain set of values.
For example boolean columns that can only contain `true`/`false` or `Yes`/`No`,
or a variable "TestStage" that can only contain `Warmup`, `Training` or `Recovery`.

This type of column is also not supported by the schema.ini format, but
CSV Lint does support this using the `Enumeration` keyword followed by the
enumeration items separated by a `|` character, see example below

	Col6=Filtered Text Width 3
	;Col6=Filtered Enumeration No|Yes
	
    Col7=TestStage Text Width 8
    ;Col7=TestStage Enumeration Recovery|Training|Warmup
	
Auto-detect will check the unique values for string or integer columns, if the unique value count is less than or equal to the `UniqueValuesMax` setting, then 
it will be treated as a enumeration column, and stored as such in the meta data.

The plug-in menu options `Convert data` and `Generate metadata` will also export these enumeration metadata, where possible.
When converting the data to an SQL insert statements, the script will also contain column constrains or enum types, depending on the SQL database type.
The Generate metadata option for "W3C CSV Schema JSON" will also export the coded value in the `format` tags.

Reformat
--------
Reformat data dialog has several options to reformat the entire data file.

    Note; always backup your data files to prevent data loss.

![CSV Lint reformat data dialog](/docs/csvlint_reformat_data.png?raw=true "CSV Lint plug-in reformat data dialog")

### Column separator ###

Reformat the column separator,  for example from comma separated `,` to semicolon separated `;`.
Any values that contain the new separator character will be put in quotes, for example `"error; no read"`.

When converting to fixed width format, it will use the width of each column as set in the metadata.
Integer or decimal values will be right aligned, any other datatypes are left aligned.

### Datetime reformat ###

Datetime format, reformat all datetime values in the file uniformly
(preferably to [ISO format](https://xkcd.com/1179/)).
The drop-down list contains the most common datetime formats, or you can
freely type any other datetime pattern. This includes two-digit year or
just the time part, for example `dd/MM/yyyy`, `dd.MM.yy`, `HH:mm:ss:fff` etc.

Note that both date and datetime values will get the same format. This means
that it can potentially remove the time-part of datetime values,
or add `00:00:00` as a time part to all values.

### Decimal separator ###

Set the decimal separator for all decimal/float values, select either the dot `.` or the comma `,`.

### Replace CrLf within values ###

Replace new-line characters (carriage return / line feed) within quoted values with a given string.
New lines usually indicate the next record in a dataset. However, quoted values may also contain a new line character.
Sometimes this can cause problems and these values aren't processed correctly.
You can use this option to replace the new-lines with for example `<br>` or `\par` or just a space ` `.
The plug-in will only replace the new-line characters within a quoted value, not the new-lines at the end of each record.

### Align vertically ###

Align vertically, will add spaces to vertically align all columns.
The amount of white space is based on the column widths.
Integer and decimal values will be right aligned and any other datatypes are left aligned,
similar to reformatting as fixed-width format.

This can be useful for viewing the data, but it's not recommended to store
the data with this extra white space. The file size will become unnecessary
large and it will be more difficult or even impossible for other
applications to process the data correctly.

### Trim all values* ###

Trim spaces from all values, for example trim the value `" No sample "` to just `"No sample"`.
This option can also be used to undo vertically aligned columns.

### Re-apply quotes* ###

Apply quotes will put all or just certain values in quotes. The default quote
character is `"` but it can be adjusted typically to `'`, see `DefaultQuoteChar`
in the settings. Select one of the following options.

* None / Minimal = Do not use quotes except when a value contains the column separator character, or a carriage return/line-feed character (this rule is always applied, also for the options below)
* Values with spaces = Apply quotes only to values that contain one or more spaces
* All string values = Apply quotes only to columns with datatype Text
* All non-numeric values = Apply quotes to all columns except datatypes Integer or Float
* All values = Apply quotes to all values in all columns

Note: Any quote character in a value will be escaped using two quote characters.
For example applying quotes to text value `CP 3/8" KAVD` will result in `"CP 3/8"" KAVD"`.

### *General settings ###

Note that the `Trim all values` and `Re-apply quotes` options are General settings,
meaning these settings are also applied when detecting, sorting, adding and validating data.
Also see `Plugins > CSV Lint > Settings`.

Validate data
-------------
Validate data based on the meta data. When you press "Validate data",
the input data will be checked for technical errors based on the given metadata.
The line and column numbers of any errors will be logged in the textbox on the
right. It will check the input data for the following errors:

* Values that are too long, example value "abcde" when column is "Width 4"
* Non-numeric values in numeric columns, example value "n/a" when column datatype is Integer
* Incorrect decimal separator, example value "12.34" when DecimalSymbol is set to comma
* Too many decimals, example value "12.345" when NumberDigits=2.
* Incorrect date format, example value "12/31/2025" when DateTimeFormat=dd/mm/yyyy

Important note: If you've edited the data file, for example changed the column
separator or added columns using the Split function, make sure to also update
the metadata before validating. Make sure the metadata reflects the current
data file by either pressing "Detect columns" or updating it manually and
then saving it.

Sort data
---------
Sort data on a single column, and take into account the data type of the column.
String text columns will be sorted alphabetically. Integer, decimal and
datetime columns will be sorted according to their respective values.
Note, that the resulting new dataset will have quotes applied according to the
current `Apply quotes` setting in the Reformat dialog.

![CSV Lint sort data dialog](/docs/csvlint_sort_data.png?raw=true "CSV Lint plug-in sort data dialog")

Sort on **value** to sort on the actual values, for example date value `31-01-2025` is lower than `01-12-2025`.  
Sort on **length of value** to sort on the character length of the values, for example `xray` has a smaller length than `abdominal`.

Sort **ascending** start with low values, end with high values `0 -> 9, A -> Z`  
Sort **descending** start with high values, end with low values `Z -> A, 9 -> 0`

When sorting on value, text columns will be sorted alphabetically, integer and
decimal columns are sorted numerically and datetime values are sorted chronologically.
Columns with enumeration are sorted according to the enumeration codelist indexes.
For example, when sorting a column with `Enumeration Never|Sometimes|Often|Always`
the records will be sorted starting with `Never`, then `Sometimes`,
then `Often` and ending with `Always`.

When sorting on a column that contains several of the same values, then the
sort order for the lines with those values will not change. Meaning that lines
with the same value will be in the sort order as they were before sorting.

It is not possible to sort on multiple columns, however it is possible to sort
multiple times to get the same result. For example if you want to sort a
dataset on visit date ascending and patient numbers descending, you can do
this by sorting each column but in reverse order. Meaning, first sort on
patient number descending and then sort on visit date ascending.

Add new column(s)
-----------------
Add column(s) based on an existing column, either edit a column or split values into new columns.
Note, that the resulting new dataset will have quotes applied according to the
current `Apply quotes` setting in the Reformat dialog.

![CSV Lint add new column dialog](/docs/csvlint_add_new_column.png?raw=true "CSV Lint plug-in add new column dialog")

### Pad character ###

Pad all values in a column with a character to a given total length.
Note that if the value is longer than the total length, then it will not be
changed. For example pad with `0` for total width of `7`, see example
results below:

| patnr     | patnr (2) |
|-----------|-----------|
| 123       | 0000123   |
| 1234      | 0001234   |
| -95       | 0000-95   |
| 123456789 | 123456789 |
| HDL       | 0000HDL   |

You can enter a negative total length to pad characters to the right instead
of to the left. For example pad with `0` for total width of `-7` will change
value `PT123` into `PT12300`, see other examples below:

| patnr     | patnr (2) |
|-----------|-----------|
| 456       | 4560000   |
| 4567      | 4567000   |
| -99       | -990000   |
| 123456789 | 123456789 |
| Glu       | Glu0000   |

### Search and replace ###

Search and replace a string with another string for all values in a column.
Unlike the default "Search and replace" function of Notepad++, this only affect
the values in a single column, not the values of any other columns. Note that
this is case-sensitive, for example search for `no` and replace with `False`:

| description | description (2)  |
|-------------|------------------|
| no          | False            |
| nonorganic  | FalseFalserganic |
| technology  | techFalselogy    |
| No          | No               |
| ACKNOWLEDGE | ACKNOWLEDGE      |

### Split valid and invalid values ###

Split valid and invalid values is useful when an integer column also
contains string values like `error` or `N/A`. This option will create two new
columns, one with just the valid values and one containing the invalid values.

As an example, if there is a column VISITDAT and in the metadata it is defined
as a date value formated as `dd-mm-yyyy`, see some example results below:

| visitdat   | visitdat (2) | visitdat (3) |
|------------|--------------|--------------|
| 15-04-2023 | 15-04-2023   |              |
| 23-05-2021 | 23-05-2021   |              |
| 07/26/2024 |              | 07/26/2024   |
| 18-09-2025 | 18-09-2025   |              |
| No show    |              | No show      |

### Split on character ###

Split on a character, split the value on the Nth occurrence of character or
string. For example split on `/` with Nth occurrence `1` will split the orginal
value `121/84` into `121` and `84`, see examples below:

| bpvalue              | bpvalue (2) | bpvalue (3)     |
|----------------------|-------------|-----------------|
| 113/79               | 113         | 79              |
| 31/12/99             | 31          | 12/99           |
| 125                  | 125         |                 |
| /85                  |             | 85              |
| IPPV/ASSIST/AutoFlow | IPPV        | ASSIST/AutoFlow |


You can also enter a negative occurrence to split the value on the last Nth
occurrence of the character. For example splitting on character `/` and
occurrence `-1` will split `pat/01/023` into `pat/01` and `023`, see other
examples below

| stringval             | stringval (2) | stringval (3) |
|-----------------------|---------------|---------------|
| usr/medic/p01/img.nii | usr/medic/p01 | img.nii       |
| 31/12/2025            | 31/12         | 2025          |
| Creatinine            | Creatinine    |               |
| /MED-0001             |               | MED-0001      |
| mo/tu/we/th/fr        | mo/tu/we/th   | fr            |

### Split on position ###

Split on character will split column values on certain position. This will
create two substring values and split them in two new columns. For example,
enter position `3` to split value `PAT-000123` to `PAT` and `-000123`, see
other examples below

| position        | position (2) | position (3) |
|-----------------|--------------|--------------|
| ZKH\21-006-2516 | ZKH          | \21-006-2516 |
| 1-6-2025        | 1-6          | -2025        |
| TGAGCATCGGAC    | TGA          | GCATCGGAC    |
| 1.2650          | 1.2          | 650          |
| 4015672111745   | 401          | 5672111745   |

You can enter a negative position to split from the end of the value, for
example split on position `-4` will split value `PT0123` into `PT` and `0123`,
see other examples below

| posneg         | posneg (2) | posneg (3) |
|----------------|------------|------------|
| medication.txt | medication | .txt       |
| 31-12-2025     | 31-12-     | 2025       |
| ACGAGTATCATG   | ACGAGTAT   | CATG       |
| 12.345         | 12         | .345       |
| M978196A002    | M978196    | A002       |


### Remove original column ###

By checking the "Remove original column" checkbox the original column will be
removed after splitting it into new columns. Keep it unchecked to add the new
columns but also keep the original values.

Note that you can also right-click the radio buttons to deselect them.
By not selecting any of the options and checking the "Remove original column" 
you can press OK to simply remove the selected original column.

Analyse data report
-------------------
Analyse data report will run an analysis on the data file and list summary
information of all values per column. This can be useful to quickly detect any
issues with the data. For example mixed datatypes, unexpected coded values,
integer values out of range etc.

The output will contain the following information for each column.

* DataTypes - the amount of values (and percentages) of each datatype value found in this column
* Width range - the minimum and maximum length of all values
* DateTime range - the minimum and maximum datetime value, if any datetime values found
* Integer range - the minimum and maximum integer value, if any integer values found
* Decimal range - the minimum and maximum decimal value, if any decimal values found
* Unique values - list of unique values, if the number of unique values in the column
is smaller than or equal to `UniqueValuesMax`,else this list will not be shown.

See report example for one single column below:

    7: TestStage
    DataTypes      : string (703 = 76.1%) empty (221 = 23.9%)
    Width range    : 6 ~ 8 characters
    -- Unique values (3) --
    n=234          : Recovery
    n=234          : Training
    n=235          : Warmup

In this example the seventh column has a header name `TestStage`,
and it only contains the values `Recovery`, `Training` and `Warmup` and empty values.
The amount of each of these 3 values is listed under "Unique values".

Count unique values
-------------------
Count unique values, this will list all unique values in a column, or
combination of columns, and count how often that unique value or combination
of values was found. This can be useful to check if the dataset contains the
expected amount of unique names, patients, product codes, barcodes etc.

![CSV Lint unique values dialog](/docs/csvlint_unique_values.png?raw=true "CSV Lint plug-in unique values dialog")

As an example, if you have a data file where each line is one blood pressure
measurement of a participant, and you want to verify that each participant in
the data file has exactly 3 measurements. In that case you can select just the
column participantId and select sort by `count`, to sort the result by the new
`count_unique` column.

If the data is correct, it should list all participantId with a `count_unique`
value of 3. And, because it's sorted by `count_unique`, you can check the
beginning and end of the list to see if there are any participants with fewer
or more than 3 measurements.

When you disable sorting, the resulting list of values will be in the order as
the values were first found in the dataset.

Convert data
------------
Convert the currently selected CSV file to SQL, XML or JSON format.

![CSV Lint Convert data dialog](/docs/csvlint_convert_data.png?raw=true "CSV Lint plug-in Convert data dialog")

Select SQL to convert the data to an SQL script to create a database
table and inserts all records from the csv datafile into that table.
The insert statement will be grouped in batches of X lines of csv data,
as set by the Batch size number in the plug-in Settings.

Depending on which database type you select, MySQL, MS-SQL or PostgreSQL,
the create table part and the autonumber field `_record_number` will be
slightly different. Enter a table name to use, or leave it empty to use the
current filename as table name.

See below for an example of an SQL insert script the plugin will generate:

    -- -------------------------------------
    -- CSV Lint plug-in: v0.4.6.8
    -- File: cardio.txt
    -- SQL type: MySQL
    -- -------------------------------------
    CREATE TABLE cardio(
        _record_number int AUTO_INCREMENT NOT NULL,
        patid integer,
        visitdat datetime,
        labpth numeric(5,1),
        primary key(_record_number)
    );
    
    -- -------------------------------------
    -- insert records 1 - 1000
    -- -------------------------------------
    INSERT INTO cardio(
        patid,
        visitdat,
        labpth
    ) VALUES
    (1001, '2025-08-21', 10.8),
    (2002, '2025-09-05', 143.5),
    (3003, '2025-09-24', 76.4),
    -- etc.

Select XML or JSON to convert the data to an XML or JSON dataset.
The plug-in will automatically apply formatting based on the metadata,
as well as applying character escaping where needed for these formats.
For XML, enter a `Table/tag name` to use as tag name for each record,
or leave it empty to use the current filename.

Generate metadata
-----------------
Generate metadata in different formats based on the column separator,
column datatypes, date format etc. Note that the generated scripts are meant
as a starting point for further script development, they don't handle all
possible data errors and you'll need to write additional code.

![CSV Lint Generate metadata dialog](/docs/csvlint_generate_metadata.png?raw=true "CSV Lint plug-in Generate metadata dialog")

### schema ini ###

File and column metadata in [schema.ini](https://docs.microsoft.com/en-us/sql/odbc/microsoft/schema-ini-file-text-file-driver?view=sql-server-ver15)
format for the Microsoft Jet OLE DB, also known as the ODBC text driver.

Note that some CSV Lint features are [not supported](https://learn.microsoft.com/en-us/answers/questions/1640430/query-odbc-text-data-using-schema-ini-and-skipline)
by the ODBC Text driver, such as [SkipLines, CommentChar](https://feedback.azure.com/d365community/search/?q=schema.ini), Enumeration columns or columns with different datetime formats.
That is why the lines with those features are commented out.

### schema JSON ###

File and column metadata in [W3C CSV schema JSON](https://www.w3.org/TR/tabular-data-primer/) format.

### Datadictionary CSV ###

Column metadata in CSV format, generates comma-separated metadata
which contains the following columns:

| Column      | Description                                      | Example               |
|-------------|--------------------------------------------------|-----------------------|
| Nr          | Column sequence number                           | 1, 2, 3 etc           |
| ColumnName  | Column name                                      | PAT_ID, VisitDate etc |
| DataType    | String, Integer, Decimal, DateTime, Date or Time |                       |
| Width       | Column maximum width total                       |                       |
| Decimals    | Nr of decimals, only for DataType=Decimal        | 1, 2, 3 etc           |
| Mask        | Mask only for DataType=Decimal or DateTime       | #0.00, dd-MM-yyyy etc |
| Enumeration | Enumeration items separated by pipe character    | No\|Yes\|Unknown      |

### Python ###

Generates a [Python](https://www.python.org/) script to read the csv data file
as a dataframe. It contains the required scripting for the appropriate
datatypes, and it is meant as a starting point for further script development.

### R-script ###

Generates an [R-script](https://www.r-project.org/) to read the csv data file
as a dataframe. It contains the required scripting for the appropriate
datatypes, and it is meant as a starting point for further script development in
[R-Studio](https://www.rstudio.com/products/rstudio/).

### PowerShell ###

Generates a [PowerShell](https://en.wikipedia.org/wiki/PowerShell) script to read the csv data file
as a dataset variable. It contains the required scripting for the appropriate
datatypes, and it is meant as a starting point for further script development.

Settings
--------
The CSV Lint plug-in settings can be changed in the menu item `Plugins > CSVLint > settings`
and they are stored in a settings file `%USERPROFILE%\AppData\Roaming\Notepad++\plugins\config\CSVLint.ini`

![CSV Lint settings window](/docs/csvlint_settings.png?raw=true "CSV Lint plug-in settings window")

| setting          | description                                                                                                     | Default |
|------------------|-----------------------------------------------------------------------------------------------------------------|---------|
| CommentChar      | Comment character, when the first lines start with this character they will be skipped as comment lines         | #       |
| DecimalDigitsMax | Maximum amount of decimals for decimal values, if a value has more then it's considered a text value. Applies to both autodetecting datatypes and validating data | 20 |
|DecimalLeadingZero| Decimal values must have leading zero, set to false to accept values like .5 or .01                             | true    |
| ErrorTolerance   | Error tolerance percentage, when analyzing allow X % errors. For example when a column with a 1000 values contains all integers except for 9 or fewer non-integer values, then it's still interpreted as an integer column. | 1 |
| IntegerDigitsMax | Maximum amount of digits for integer values, if a value has more then it's considered a text value. Applies to both autodetecting datatypes and validating data. Useful to distinguish (bar)codes and actual numeric values  | 12 |
| UniqueValuesMax  | Maximum unique values when reporting or detecting coded values, if column contains more than it's not reported. |   15    | 
| YearMaximum      | When detecting date or datetime values, years larger than this value will be considered an out-of-range date.   | 2050    |
| YearMinimum      | When detecting date or datetime values, years smaller than this value will be considered an out-of-range date.  | 1900    |
| ReformatQuotes   | Reformat dataset, apply quotes option: 0 = None minimal, 1 = Values with spaces, 2 = All string values, 3 = All non-numeric values, 4 = Always | 0       |
| TrimValues       | Trim values before analyzing or editing (recommended).                                                          | true    |
| TwoDigitYearMax  | Maximum year for two digit year date values. For example, when set to 2030 the year values 30 and 31 will be interpreted as 2030 and 1931. Set as CurrentYear for current year. | CurrentYear |
| AutoSyntaxLimit  | Convert data, automatically apply syntax highlighting to resulting file, only when it's smaller than this size. Prevent Notepad++ from freezing on large files. |1024*1024|
| DefaultQuoteChar | Default quote character, typically a double quote " or a single quote '                                         | "       |
| FontDock         | Default font for text boxes in CSV Lint docking window. Changing the font requires closing and opening the CSV docked window.  | Courier New, 11.25pt  |
| NullKeyword      | A case-sensitive keyword that will be treated as an empty value, typically `NULL`, `NaN`, `NA` or `None` depending on your data | NaN     |
| SeparatorColor   | Include separator in syntax highlighting colors. Set to false and the separator characters are always white.    | false   |
| Separators       | Preferred characters when automatically detecting the separator character. For special characters like tab, use \\t or hexadecimal escape sequence \\u0009 or \\x09. | ,;\t&#124; |
| TransparentCursor| Transparent cursor line, changing this setting will require a restart of Notepad++                              | true    |
| UserPref section | Various input settings for the CSV Lint dialogs for Convert Data, Reformat, Split Column etc.                   |         |

About
-----
An about dialog with the version number, contact information and a link to the documentation.

Disclaimer
----------
This software is free-to-use and it is provided as-is without warranty of any kind.  
Always back-up your data files to prevent data loss.  
All [test files](../testdata/), examples and screenshots provided in this github repository contain fictitious and 
[randomly generated](https://github.com/BdR76/RandomValuesNPP) data,
any resemblance to real-life cases is the result of chance.

History
-------
15-dec-2019 - v0.1 first release  
02-may-2021 - v0.2 reformat data, double-click jumps to line, various bugfixes  
25-aug-2021 - v0.3 quoted string values, syntax highlighting, SQL export  
26-sep-2021 - v0.4 performance improvement, save/load metadata, split column option, count unique values  
17-oct-2021 - v0.4.1 various bugfixes  
29-oct-2021 - v0.4.2 startup error "CSVLint.xml is missing" fixed, toggle toolbar icon, clean up settings  
12-nov-2021 - v0.4.3 dark mode icons and colors, save form settings, documentation, bugfixes  
19-dec-2021 - v0.4.4 Support large integer values, various bugfixes  
12-mar-2022 - v0.4.5 Render on background thread, transparent cursor line, convert to XML/JSON, generate metadata, quotes reformat, help icons, bugfixes  
27-apr-2022 - v0.4.5.1 bugfix for Notepad++ 8.4 Lexer v5 update  
03-jun-2022 - v0.4.5.2 Another Lexer v5 bugfix, generate Python script  
25-jul-2022 - v0.4.5.3 Manually detect columns, improved fixed width support  
14-aug-2022 - v0.4.5.4 Improved datatype and datetime mask detection, various bugfixes  
30-sep-2022 - v0.4.6 Improved unicode support, sort data option, default color sets 12 colors, various bugfixes  
09-oct-2022 - v0.4.6.1 Improved sort data and split options, various bugfixes  
29-oct-2022 - v0.4.6.2 Fixed width positions parameter, docked window font, syntax highlighting toggle button  
16-apr-2023 - v0.4.6.3 Syntax Highlighting fix, new SkipLines feature and quoted string improvements  
02-may-2023 - v0.4.6.4 Comment character and dark mode support  
04-jun-2023 - v0.4.6.5 Support enum/coded values and various updates  
16-dec-2023 - v0.4.6.6 PowerShell support and various updates  
25-jun-2024 - v0.4.6.7 Reformat bugfix, improved enumeration, sort on length  
28-feb-2025 - v0.4.6.8 Improved sorting for enumeration columns, minor updates  

BdR©2019-2025 Free to use - send questions or comments: Bas de Reuver - bdr1976@gmail.com
