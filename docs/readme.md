CSV Lint plug-in documentation
==============================

**CSV Lint** is a plug-in for Notepad++ to work with  csv and fixed width data files.

* [CSV Lint plug-in](https://github.com/BdR76/CSVLint/)
* [Notepad++ homepage](http://notepad-plus-plus.org/)

![preview screenshot](../csvlint_preview.png?raw=true "CSVLint plug-in preview")

Use the **CSV Lint** plug-in to quickly and easily inspect csv data files,
add column syntax highlighting, detect technical errors and fix datetime and
decimal formatting. It is a quality control tool to examine, verify or polish
up a dataset before further processing, it's not meant as a replacement for a
spreadsheet program.

First install and open Notepad++, then go to the menu item `Plugins > Plugins Admin...`,
search for "csv lint", check the checkbox and press Intall. This will add
CSV Lint under the `Plugins > CSV Lint` menu item and a CSV Lint icon in the
toolbar icon.

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
edit options in the plug-in to work correctly. You can press "Refresh from
Data" to automatically detect the metadata from the datafile, and/or manually
edit the metadata in the textbox. When manually editing the metadata, always
press the save icon (blue disk) to apply it before continuing.

### Refresh from Data ###

Press the "Refresh from Data" button the from the currently open data file.
When a file is opened this auto-detection is also run once by default.
When a file is opened the plug-in will:

1) check for a `schema.ini` file in same folder as data file
2) check if the `schema.ini` contains a section for the filename
2) if no `schema.ini` or section found, then run "Refresh from data"

Note; if "Refresh from data" cannot automatically detected any columns, then
the metadata definition will default to a "TextFile" with one column of
width 9999 characters, i.e. no columns found.

### Save icon ###

Click this blue-disk to save the metadata after editing the metadata manually
to apply the changes. This will also save the metadata to a `schema.ini` file
in the same folder as the data file. The next time you open the datafile with
the plug-in, it will automatically load the metadata from this file.

The metadata will be save under a section with the filename, it can contain
multiple metadata. A `schema.ini` file can contain the meta data for more
than one data file, using separate sections for each file.

### Split / Reformat / Validate ###

The **Split column**, **Reformat** and **Validate** buttons are explained
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

DateTimeFormat is not case sensitive and uses dd/mm/yyyy or yyyy-mm-dd hh:nn:ss etc.

DecimalSymbol can be either `.` or `,` and CSV Lint will assume the thousand
separators symbol is the opposite of the DecimalSymbol. Define the maximum
decimals digits for example `NumberDigits=2` for values like "1.23" or
"-45.67" etc.

Reformat
--------
Reformat data dialog has several options to reformat the entire data file.

    Note; always backup your data files to prevent data loss.

![CSV Lint reformat data dialog](/docs/csvlint_reformat_data.png?raw=true "CSV Lint plug-in reformat data dialog")

### Datetime reformat ###

Datetime format, reformat all datatime values in the file uniformly. Note that
both date and datetime values will get the same format. This means that it
can potentially remove the time-part of datetime values, or add `00:00:00` as
a time part to all values.

### Decimal separator ###

Set the decimal separator for all decimal/float values, select either the  `.` or `,`.

### Column separator ###

Reformat the column separator,  for example from comma separated `,` to semicolon separated `;`.
Any values that contain the new separator character will be put in quotes, for example `"error; no read"`.

When converting to fixed width format, it will use the width of each column as set in the metadata.
Integer or decimal values will be right aligned, any other datatypes are left aligned.

### Trim all values ###

Trim spaces from all values, for example trim the value " Yes " to "Yes".

### Align vertically ###

Align vertically, will add white space to vertically align all columns.

This can be useful for viewing the data, but it's not recommended to store
the data with this extra white space. The file size will become unnecessary
large and it will potentially be harder for other applications to process the
data correctly.

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
* Incorrect date format, example value "12/31/2020" when DateTimeFormat=dd/mm/yyyy

Important note: If you've edited the data file, for example changed the column
separator or added columns using the Split function, make sure to also update
the metadata before validating. Make sure the metadata reflects the current
data file by either pressing "Refresh from data" or updating it manually and
then saving it.

Split column
------------
Split values into new columns.

![CSV Lint split column dialog](/docs/csvlint_split_column.png?raw=true "CSV Lint plug-in split column dialog")

### Split valid and invalid values ###

Split valid and invalid values is useful when an integer column also
contains string values like `error` or `N/A`. This option will create a new
column with just the valid values and a new column containing invalid values.

As an example, if there is a column VISITDAT and in the metadata it is defined
as a date value formated as `dd-mm-yyyy`, see some example results below:

| visitdat   | visitdat (2) | visitdat (3) |
|------------|--------------|--------------|
| 15-04-2020 | 15-04-2020   |              |
| 23-05-2019 | 23-05-2019   |              |
| 07-26-2021 |              | 07-26-2021   |
| 18-09-2020 | 18-09-2020   |              |
| noshow     |              | noshow       |

### Split on character ###

Split on a character will take a substring on values in the column.
give a number of character as parameter,  use negative for right.

For example split on `/` will split the orginal value `121/84` into `121` and
`84`, see examples below:

| bpvalue | bpvalue (2) | bpvalue (3) |
|---------|-------------|-------------|
| 113/79  | 113         | 79          |
| 129/67  | 129         | 67          |
| 169/102 | 169         | 102         |
| 125     | 125         |             |
| /85     |             | 85          |

### Split on position ###

Split on character will split column values on certain position. This will
create two substring values and split them in two new columns. For example,
enter position `3` to split value `PAT-000123` to `PAT` and `-000123`, see
other examples below

| position        | position (2) | position (3) |
|-----------------|--------------|--------------|
| ZKH\21-006-4929 | ZKH          | \21-006-4929 |
| OZG\19-006-4929 | OZG          | \19-006-4929 |
| abcdefghijk     | abc          | defghijk     |
| 123.45          | 123          | .45          |
| 12345.67        | 123          | 45.67        |

You can enter a negative position to spit from the end of the value, for
example split on position `-4` will split value `PT0123` into `PT` and `0123`,
see other examples below

| posneg         | posneg (2) | posneg (3) |
|----------------|------------|------------|
| medication.txt | medication | .txt       |
| 31-12-2020     | 31-12-     | 2020       |
| abcdefghijk    | abcdefg    | hijk       |
| 12.345         | 12         | .345       |
| 4015672110397  | 401567211  | 0397       |

### Move value if it contains ###

Move all values that contains a certain text part. For example move if it
contains `.00` it will create two columns, one with values that do contain it
and one column for the values without.

| move     | move (2)  | move (3) |
|----------|-----------|----------|
| 12.0     | 12.0      |          |
| 100.00   |           | 100.00   |
| 123.45   | 123.45    |          |
| Hgb      | Hgb       |          |
| 12.00.34 |           | 12.00.34 |

### Decode multiple values ###

Decode multiple values is useful when a single variable contains a set of
multiple values separated by a character, these are typically checkbox values
that allow multiple answers.

For example `Hb;K;Nat;Lac` and character `;` will create 5 new columns,
the 4 possible values plus one for any remaining values.

| labchk          | labchk (2) | labchk (3) | labchk (4) | labchk (5) | labchk (6) |
|-----------------|------------|------------|------------|------------|------------|
| Hb;K;Nat;Lac    | Hb         | K          | Nat        | Lac        |            |
| K;Lac           |            | K          |            | Lac        |            |
| Hb;Nat;Cl       | Hb         |            | Nat        |            | Cl         |
| LDL;K;Hb;HDL    | Hb         | K          |            |            | LDL;HDL    |
| IgM;Glu         |            |            |            |            | IgM;Glu    |

Note that the order of the values within the original column does not matter,
the new columns are always created in the order as specified by the decode-string.
Also, the last new column `labchk (6)` contains any left-over unspecified values

### Remove original column ###

By checking the "Remove original column" checkbox the original column will be
removied after splitting it into new columns. Keep it unchecked to add the new
columns but also keep the original values.

Analyse data report
-------------------
Analyse data report will run an analysis on the data file and list summary
information of all values per column. This can be useful to quickly detect any
issues with the data. For example mixed datatypes, unexpected coded values,
integer values out of range etc.

The output will contain the following information for each column.

* DataTypes - the amount of values (and percentages) of each datatype value found in this column
* Width range - the minimum and maximum length of all values
* DateTime range - the minimum and maximum datatime value, if any datetime values found
* Integer range - the minimum and maximum integer value, if any integer values found
* Decimal range - the minimum and maximum decimal value, if any decimal values found
* Unique values - list of unique values, not shown when there are more than `UniqueValuesMax` unique values found (see settings)

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
expected amount of unique records.

![CSV Lint unique values dialog](/docs/csvlint_unique_values.png?raw=true "CSV Lint plug-in unique values dialog")

As an example, in a blood pressure file each line is one measurement and each
participant is expected to have exactly 3 measurements. In this case you can
select just the column participantId and select sort by count, to sort the
result by the new `count_unique` column.

If the data is correct, it should list all participantId with a `count_unique`
value of 3. Because it's sorted by `count_unique` you can check the beginning
and end of the list to see if there are any participants with fewer or more
than 3 measurements.

Convert to SQL
--------------
Convert the currently selected CSV file to an SQL script that creates a database
table and inserts all records from the csv datafile into that table.
The insert statement will be grouped in batches of X lines of csv data,
see the `SQLBatchRows` setting. For compatibility with both mySQL or MS-SQL,
the script will generate the column names as \`columnname\` or [columnname]
depending on the `SQLansi` setting.

See below for an example of an SQL insert script the plugin will generate:

    -- -------------------------------------
    -- CSV Lint plug-in v0.4.2
    -- File: cardio.txt
    -- -------------------------------------
    CREATE TABLE cardio(
        `patid` integer,
        `visitdat` datetime,
        `labpth` numeric(5,1)
    );

    -- -------------------------------------
    -- insert records 1 - 1000
    -- -------------------------------------
    insert into cardio(
        `patid`,
        `visitdat`,
        `labpth`
    ) values
    (1, '2021-08-21', 10.8),
    (2, '2021-09-05', 143.5),
    (3, '2021-09-24', 76.4),
    -- etc.

Settings
--------
Some plug-in settings can be changed in the menu item `Plugins > CSVLint > settings`
and they are stored in a settings file `%USERPROFILE%\AppData\Roaming\Notepad++\plugins\config\CSVLint.ini`

![CSV Lint settings window](/docs/csvlint_settings.png?raw=true "CSV Lint plug-in settings window")

| setting          | description                                                                                                     | Default |
|------------------|-----------------------------------------------------------------------------------------------------------------|---------|
| UniqueValuesMax  | Maximum unique values when reporting or detecting coded values, if column contains more than it's not reported. |   15    | 
| ScanRows         | Maximum rows to analyze to automatically detect data types. Set to 0 to analyze all rows, set to 1000 for better performance with large files. | 0 |
| YearMinimum      | When detecting date or datetime values, years smaller than this value will be considered as invalid dates.      | 1900    |
| YearMaximum      | When detecting date or datetime values, years larger than this value will be considered as invalid dates.       | 2050    |
| SQLBatchRows     | Maximum records per SQL insert batch, minimum batch size is 10.                                                 | 1000    |
| SQLansi          | Convert to ANSI standard SQL script, set to true for mySQL or false for MS-SQL.                                 | true    |
| TwoDigitYearMax  | Maximum year for two digit year date values. For example, when set to 2024 the year values 24 and 25 will be interpreted as 2024 and 1925. Set as SysYear for current year. | SysYear |
| DefaultQuoteChar | Default quote escape character when quotes exists inside text                                                   | "       |
| NullValue        | Keyword for empty values or null values in the csv data, case-sensitive.                                        | NaN     |
| SeparatorColor   | Include separator in syntax highlighting colors. Set to false and the separator characters are always white.    | false   |
| Separators       | Preferred characters when automatically detecting the separator character. For special characters like tab, use \\t or \\u0009. | ,;\t| |
| TrimValues       | Trim values before analyzing or editing (recommended).                                                          | true    |
| UserPref section | Various settings for CSV Lint form defaults                                                                     |         |

Syntax highlighting colors
--------------------------
Syntax highlighting will make it easier to see columns in the data files.
There are four pre-defined color schemes you can select in the `Setttings` dialog.
At first time startup, the plug-in will select normal or darkmode color scheme,
depending on the Dark Mode setting in the Notepad++ `config.xml`.

![CSV Lint color styles for syntax highlighting](/docs/csvlint_color_styles.png?raw=true "CSV Lint plug-in color styles for syntax highlighting")

The color scheme settings are stored in a file `CSVLint.xml` which is
automatically created at first time startup or when the file is missing,
also see an example [CSVLint.xml file here](https://github.com/BdR76/CSVLint/blob/master/config/CSVLint.xml).

	%USERPROFILE%\AppData\Roaming\Notepad++\plugins\config\CSVLint.xml

You can also change the colors in the "Style Configurator dialog", see menu item
`Settings > Style configurator... > Language: CSV Lint`. The "CSV Lint" should
be near bottom of list. If you have a CSV file open you'll immediately see the
changes as you edit them. Note that selecting a default color scheme in `Setttings`
will overwrite any changes made in the Style Configurator dialog.

About
-----
An about window

Disclaimer
----------
This software is free-to-use and it is provided as-is without warranty of any kind,
always back-up your data files to prevent data loss.

BdR©2021 Free to use - send questions or comments: Bas de Reuver - bdr1976@gmail.com