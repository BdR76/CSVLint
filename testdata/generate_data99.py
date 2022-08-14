# generate csv with 99 columns
# ----------------------------
# Generate random data with all different datatypes
# for testing automatic column detection
# BdR 2022 - bdr1976@gmail.com

import pandas as pd
import math
import random
import array as arr
import datetime
from datetime import timedelta

# constants
FILE_NAME = "columns_99.txt"

testdate = datetime.datetime.now()
curryear = testdate.year

# random digits as string
def random_digits_string(c_min, c_max=None):
    # how many digits
    howmany = c_min
    if c_max != None:
        howmany = random.randint(c_min, c_max)

    # built random digits string
    retval = ""
    for i in range(1, howmany+1):
        retval += str(random.randint(0, 9))
    return retval

# random date as string
def random_datetime_string(mask, yr_min=None, yr_max=None):
    year1 = curryear
    year2 = curryear
    if yr_min != None:
        if yr_min < 0:
            year1 = curryear + yr_min
        else:
            year1 = yr_min
            
    year2 = (curryear if yr_max == None else yr_max)

    # random date
    r_year = random.randint(year1, year2)
    r_month = random.randint(1, 12)
    r_daymax = (30 if r_month in [4,6,9,11] else (28 if r_month == 2 else 31))
    r_day = random.randint(1, r_daymax)

    r_hour = random.randint(0, 23)
    r_min = random.randint(0, 59)
    r_sec = random.randint(0, 59)
    
    rnddate = datetime.datetime(r_year, r_month, r_day, r_hour, r_min, r_sec)
    #print ("TESTING %d : %d : %d" % (r_hour, r_min, r_sec))

    return rnddate.strftime(mask)

# create an Empty DataFrame object
df = pd.DataFrame()

for i in range(1, 10+1):
    print(random_datetime_string("%#d-%#m-%Y %H:%M:%S"))

#quit() # quit at this point

for col in range(1, 100):
    print(col)
    tmpval = []
    for row in range(1, 100):
        # differnt columns values
        val = None
        # integers
        if col == 1:
           val = str(row)
        elif col == 2:
            val = str(math.floor(10.1 * row))
        elif col == 3:
            val = str(random.randint(-500, 500))
        elif col == 4:
            val = str(random.randint(1, 99999))
        elif col == 5:
            val = str(random.randint(1, 8)) + random_digits_string(6)
        elif col == 6:
            val = str(random.randint(7, 8)) + random_digits_string(9)
        elif col == 7:
            val = str(random.randint(10, 12)) + random_digits_string(10)
        elif col == 8:
            val = str(random.randint(11, 13)) + random_digits_string(11)
        elif col == 9:
            val = str(random.randint(10, 15)) + random_digits_string(13)
        elif col == 10:
            val = str(random.randint(16, 20)) + random_digits_string(18)
        # date
        elif col == 11:
            val = random_datetime_string("%Y-%m-%d", -3)
        elif col == 12:
            val = random_datetime_string("%d.%m.%Y", -2)
        elif col == 13:
            val = random_datetime_string("%#d/%#m/%Y", -1)
        elif col == 14:
            val = "1/1/" + str(curryear - 94 + row)
        elif col == 15:
            val = random_datetime_string("%d.%m.%y", -80) # two digit year
        elif col == 16:
            val = random_datetime_string("%m/%d/%y", -80) # two digit year
        elif col == 17:
            val = "1/1/" + str(curryear - 94 + row)[-2:]
        elif col == 18:
            val = str(curryear - 94 + row)[-2:] + "/1/1"
        elif col == 19:
            val = random_datetime_string("%Y%m%d", -80)
        elif col == 20:
            val = random_datetime_string("%d%m%Y", -80)
        # time
        elif col == 21:
            val = random_datetime_string("%H:%M:%S")
        elif col == 22:
            val = random_datetime_string("%H:%M:%S.") + random_digits_string(3)
        elif col == 23:
            val = random_datetime_string("%H:%M")
        elif col == 24:
            val = random_datetime_string("%M:%S")
        elif col == 25:
            val = random_datetime_string("%M:%S.000")
        elif col == 26:
            val = random_datetime_string("%H:%M:%S.000")
        elif col == 27:
            val = random_datetime_string("%#H:%M:%S")
        elif col == 28:
            val = random_datetime_string("%#I:%M%p")
        elif col == 29:
            val = random_datetime_string("%#I:%M:%S%p")
        elif col == 30:
            val = random_datetime_string("%H:%M:%S.z00")
        # datetime
        elif col == 31:
            val = random_datetime_string("%Y.%m.%d %H:%M:%S", -10)
        elif col == 32:
            val = random_datetime_string("%#d-%#m-%Y %#H:%M:%S", -10)
        elif col == 33:
            val = random_datetime_string("%m/%d/%Y %#H:%M:%S.") + random_digits_string(3)
        elif col == 34:
            val = random_datetime_string("%d.%m.%y %#H:%M", -50)
        elif col == 35:
            val = ("1/1/%s 1:00" % str(curryear - 94 + row)[-2:])
        elif col == 36:
            val = ("01/01/%s 12:00PM" % str(curryear - 94 + row)[-2:])

        elif col == 37:
            val = "1/1/" + str(curryear - 94 + row)[-2:] + random_datetime_string(" %H:%M:%S.000")
        elif col == 38:
            val = str(curryear - 94 + row)[-2:] + "/1/1" + random_datetime_string(" %#H:%M:%S")
        elif col == 39:
            val = random_datetime_string("%Y%m%d%H%M%S", -10)
        elif col == 40:
            val = random_datetime_string("%d%m%Y %H%M", -30)
        # decimals
        elif col == 41:
           val = ("%d.%s" % ( random.randint(0, 12), random_digits_string(2)))
        elif col == 42:
            val = ("%d.%s" % ( random.randint(-9, 9), random_digits_string(3))).replace("0.", ".")
        elif col == 43:
            val = ("%d.%s" % ( random.randint(-50, 20), random_digits_string(3)))
        elif col == 44:
            val = ("%d,%s" % ( random.randint(1, 9999), random_digits_string(1)))
        elif col == 45:
            r_int = random.randint(-12, 12)
            val = ("%d.%d%s" % ( abs(r_int), (curryear - 94 + row), ("-" if r_int<0 else "") ))
        elif col == 46:
            val = ("%d,%s" % ( random.randint(7, 8), random_digits_string(8)))
        elif col == 47:
            val = ("%d.%s" % ( random.randint(10, 12), random_digits_string(9)))
        elif col == 48:
            val = ("%d,%s" % ( random.randint(11, 13), random_digits_string(10)))
        elif col == 49:
            val = ("%d.%s" % ( random.randint(10, 15), random_digits_string(12)))
        elif col == 50:
            val = ("%d,%s" % ( random.randint(-20, 20), random_digits_string(16)))
        # other
        else:
            val = "other"

        # glitch errors
        #val = "other"

        # add value to column
        tmpval.append(val)

    # determine column name
    colprefix = "integer"
    if 1 <= col <= 10:
        colprefix = "integer"
    elif 11 <= col <= 20:
        colprefix = "date"
    elif 21 <= col <= 30:
        colprefix = "time"
    elif 31 <= col <= 40:
        colprefix = "datetime"
    elif 41 <= col <= 50:
        colprefix = "decimal"
    elif 51 <= col <= 60:
        colprefix = "currency"
    else:
        colprefix = "other"
    colname = ('%s_%02d' % (colprefix, col))
    
    # add column to dataframe
    df[colname] = tmpval

# Observe the result
print(df)

# csv write new output
df.to_csv(FILE_NAME, sep='\t', na_rep='', header=True, index=False, encoding='utf-8')


