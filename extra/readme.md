CSV Lint - extra files
======================

Extra files and scripts for generating various test data.

### generate_data.py ###

Python script to generate a typical csv data file,
based on the structure of a real research dataset,
but with randomized dummy data.

### generate_data99.py ###

Python script to generate a csv data file with 99 columns,
filled with random data and using as many different column formats as possible.
Used for testing the automatic column detection algorithm.

### generate_data_unicode.py ###

Python script to generate a csv data file with random unicode characters,
used for testing the syntax highlighting and different Windows region/code page settings.

### generate_colors.py ###

Python script to determine the syntax highlighting colors. The script
determines color sequences where there won't be columns with similar colors
next to each other (red/orange or blue/cyan). The script selects a color
sequence where each color is as different as possible from the next,
see the [results here](https://github.com/BdR76/CSVLint/tree/master/extra/generate_colors.png).
This makes it easier to distinguish columns while also making it more accessible
for people who are [color blind](https://github.com/BdR76/CSVLint/tree/master/extra/colorblind_vision1.png)
(as opposed to using a color scheme [like this](https://github.com/BdR76/CSVLint/tree/master/extra/colorblind_vision2.png)).

Note: Technically the CSV Lint plugin can use max 31 different column colors.
However, for 16 or more colors the script starts generating very similar
looking colors, which kind of defeats the purpose, 12 colors seems to be the optimum.
The 16 colors xml is provided here just for the sake of completion.

### CSVLint.xml configuration ###

Syntax highlighting configuration for different predefined color sets

| File                  | Description                                    |
|-----------------------|------------------------------------------------|
| CSVLint.xml           | Old configuration, default colors pre v0.4.6   |
| CSVLint_8_colors.xml  |  8 colors optimized                            |
| CSVLint_12_colors.xml | 12 colors optimized, default colors in v0.4.6  |
| CSVLint_16_colors.xml | 16 colors optimized                            |

BdR 2022 Free to use - questions or comments: bdr1976@gmail.com
