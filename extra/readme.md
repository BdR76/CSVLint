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

### generate_colors.py ###

Python script to determine the optimal colors for syntax highlighting,
by determinging a color sequence with the most contrast between each color.

### CSVLint.xml configuration ###

Syntax highlighting configuration for different predefined color sets

| File                  | Description                                    |
|-----------------------|------------------------------------------------|
| CSVLint.xml           | Old configuration, default colors pre v0.4.5   |
| CSVLint_8_colors.xml  |  8 colors optimized                            |
| CSVLint_12_colors.xml | 12 colors optimized, default colors in v0.4.6  |

BdR 2022 Free to use - questions or comments: bdr1976@gmail.com


