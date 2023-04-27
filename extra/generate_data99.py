# generate csv with 99 columns
# ----------------------------
# Generate random data with all different datatypes
# for testing automatic column detection
# 
# Bas de Reuver - bdr1976@gmail.com (aug 2022)

import pandas as pd
import random
import math
import datetime
from datetime import timedelta
import string

# constants
FILE_NAME = "columns_99.txt"

lorem_arr = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.".split(' ')
fiction_arr = "Blank Doesnotexist Dreamedup Example Fabricated Fakery Fantasized Fantasy Feigned Fictional Fictitious Fictive Forinstance Imaginary Imagined Invented Madeup Makebelieve Mockup Nonexistent Notreal Phoney Placeholder Pretended Simulated Specimen Standin Testcase Unreal".split(' ')
alpha_arr = "Alfa Bravo Charlie Delta Echo Foxtrot Golf Hotel India Juliet Kilo Lima Mike November Oscar Papa Quebec Romeo Sierra Tango Uniform Victor Whiskey Xray Yankee Zulu".split(' ')

testdate = datetime.datetime.now()
curryear = testdate.year

# random digits as string
def random_digits_string(c_min, c_max=None, c_thou=None):
    # how many digits
    howmany = c_min
    if c_max != None:
        howmany = random.randint(c_min, c_max)

    # built random digits string
    retval = ""
    for i in range(1, howmany+1):
        retval += str(random.randint(0, 9))

    # add thousand separators
    if c_thou != None:
        retval = retval.lstrip("0")
        # insert thousand separators
        for i in range(len(retval)-3, 0, -3): # reversed, step -1
            retval = retval[:i] + c_thou + retval[i:]
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

# random lorem ipsum text
def random_lorem_string(c_min, c_max=None):
    # text how long
    howlong = c_min
    if c_max != None:
        howlong = random.randint(c_min, c_max)

    # built random lorem string
    idx = random.randint(0, len(lorem_arr)-1)
    retval = ""
    while len(retval) < howlong:
        if len(retval + lorem_arr[idx]) > howlong:
            break
        retval += lorem_arr[idx] + " "
        idx = (idx + 1) % len(lorem_arr)

    return retval.strip()

# create an Empty DataFrame object
df = pd.DataFrame()

#for i in range(1, 10+1):
#    print(random_lorem_string(10))
#quit() # quit at this point

print("Generating 99 columns:")

for col in range(1, 100):
    print(("%s.." % col), end = '')
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
            val = "1/1/" + str(curryear - 94 + (row % 100))
        elif col == 15:
            val = random_datetime_string("%d.%m.%y", -80) # two digit year
        elif col == 16:
            val = random_datetime_string("%m/%d/%y", -80) # two digit year
        elif col == 17:
            val = "1/1/" + str(curryear - 94 + (row % 100))[-2:]
        elif col == 18:
            val = str(curryear - 94 + (row % 100))[-2:] + "/1/1"
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
            val = ("1/1/%s 1:00" % str(curryear - 94 + (row % 100))[-2:])
        elif col == 36:
            val = ("01/01/%s 12:00PM" % str(curryear - 94 + (row % 100))[-2:])

        elif col == 37:
            val = "1/1/" + str(curryear - 94 + (row % 100))[-2:] + random_datetime_string(" %H:%M:%S.000")
        elif col == 38:
            val = str(curryear - 94 + (row % 100))[-2:] + "/1/1" + random_datetime_string(" %#H:%M:%S")
        elif col == 39:
            val = random_datetime_string("%Y%m%d%H%M%S", -10)
        elif col == 40:
            val = random_datetime_string("%d%m%Y %H%M", -30)
        # decimals
        elif col == 41:
            if random.randint(1, 20) == 10:
                val = "<0.3"
            else:
                val = ("%d.%s" % ( random.randint(1, 12), random_digits_string(2)))
        elif col == 42:
            val = ("%d.%s" % ( random.randint(-9, 9), random_digits_string(3))).replace("0.", ".")
        elif col == 43:
            val = ("%d.%s" % ( random.randint(-50, 20), random_digits_string(3)))
        elif col == 44:
            val = ("%d,%s" % ( random.randint(1, 9999), random_digits_string(1)))
        elif col == 45:
            r_int = random.randint(-12, 12)
            val = ("%d.%d%s" % ( abs(r_int), (curryear - 94 + (row % 100)), ("-" if r_int<0 else "") ))
        elif col == 46:
            val = ("%d,%s" % ( random.randint(7, 8), random_digits_string(8)))
        elif col == 47:
            val = ("%d.%s" % ( random.randint(10, 15), random_digits_string(12)))
        elif col == 48:
            val = ("%d,%s" % ( random.randint(-20, 20), random_digits_string(16)))
        elif col == 49:
            val = ("%d.%se%d" % ( random.randint(1, 9), random_digits_string(3), random.randint(10, 12)))
        elif col == 50:
            r_int = random.randint(0, 1)
            val = ("%d,%sE%s%02d" % ( random.randint(-9, 9), random_digits_string(7), ("-" if r_int==1 else ""), random.randint(7, 20)))
        # currency
        elif col == 51:
           val = ("%s%s" % (random.choice(["", "-"]), random_digits_string(5, 7, ".")))
        elif col == 52:
            val = ("%s%s" % (random.choice(["", "-"]), random_digits_string(8, 9, ",")))
        elif col == 53:
            val = ("%s,%02d" % ( random_digits_string(3, 5, "."), (random.randint(1, 19)*5)))
        elif col == 54:
            val = ("%s.%02d" % ( random_digits_string(3, 5, ","), (random.randint(1, 19)*5)))
        elif col == 55:
            val = ("€ %s,00" % (random_digits_string(4, 5, ".")))
        elif col == 56:
            val = ("USD %s.00" % (random_digits_string(4, 5, ",")))
        elif col == 57:
            val = ("€ %s%s,00" % (random.choice(["", "-"]), random_digits_string(4, 5, ",")))
        elif col == 58:
            val = ("%s$ %s.00" % (random.choice(["", "-"]), random_digits_string(4, 5, ",")))
        elif col == 59:
            val = random_digits_string(4, 4, ".") + ",00"
        elif col == 60:
            val = random_digits_string(4, 4, ".") + ",00"
        # other
        elif col == 61:
            val = random.choice(["yes", "no"])
        elif col == 62:
            val = random.choices(["MALE", "FEMALE", "-"], weights=(49, 49, 2))[0]
        elif col == 63:
            val = random.choices(alpha_arr)[0] + random.choice(["street", "road", "lane", "square"]) + " " + str(random.randint(1, 300))
            if random.randint(1, 10) == 5:
                val = random.choices(alpha_arr)[0] + "-" + val # add extra streenname part
            if random.randint(1, 10) == 5:
                val = val + random.choice("ABC") # add letter to housenumber
        elif col == 64:
            val = str(random.randint(1000, 9999)) + random.choice(string.ascii_uppercase) + random.choice(string.ascii_uppercase)
        elif col == 65:
            val = (random.choices(fiction_arr)[0] + random.choice(["city", "town", "ville", "fields", "port", "ton"])).upper()
        elif col == 66:
            dummy = random.randint(1, 100)
            val = (random.choices(alpha_arr)[0] if dummy >=50 else random.choices(fiction_arr)[0])
            if dummy % 3 == 0:
                val = val + (str(random.randint(50, 99)) if dummy >=50 else str(random.randint(1990, 2015)))
            if dummy % 2 == 0:
                val = (random.choices(alpha_arr)[0] if dummy >=50 else random.choices(fiction_arr)[0]) + "." + val
            val = val + "@" + random.choices(fiction_arr)[0] + random.choice([".com", ".org", ".net", ".at", ".ch", ".cz", ".de", ".dk", ".es", ".eu", ".fi", ".fr", ".gr", ".hr", ".hu", ".is", ".it", ".ie", ".nl", ".no", ".pl", ".pt", ".se", ".sm", ".ro ", ".ua", ".uk"])
            val = val.lower()
        elif col == 67:
            val = random.choices(["NEG", "POS", "readrror", "-"], weights=(30, 10, 2, 2))[0]
        elif col == 68:
            val = random.choices(["NEG", "POS", "readrror", "-"], weights=(30, 10, 2, 2))[0]
        elif col == 69:
            val = random.choice(["Hb", "LDL", "HDL"])
        elif col == 70:
            val = random.choices(["mmol/L", "µmol/L", "mg/mL", "g/L", "g/dL", "g/100mL", "g%"], weights=(30, 20, 20, 5, 5, 5, 2))[0]
        # random texts
        elif col == 71:
            val = random_lorem_string(10)
        elif col == 72:
            val = random_lorem_string(20)
        elif col == 73:
            val = random_lorem_string(30)
        elif col == 74:
            val = random_lorem_string(40)
        elif col == 75:
            val = random_lorem_string(50)
        elif col == 76:
            val = random_lorem_string(0, 10)
        elif col == 77:
            val = random_lorem_string(0, 20)
        elif col == 78:
            val = random_lorem_string(0, 30)
        elif col == 79:
            val = random_lorem_string(0, 40)
        elif col == 80:
            val = random_lorem_string(0, 50)
        # random texts
        elif col == 81:
            val = random_lorem_string(10)
        elif col == 82:
            val = random_lorem_string(10)
        elif col == 83:
            val = random_lorem_string(10)
        elif col == 84:
            val = random_lorem_string(10)
        elif col == 85:
            val = random_lorem_string(10)
        elif col == 86:
            val = random_lorem_string(10)
        elif col == 87:
            val = random_lorem_string(10)
        elif col == 88:
            val = random_lorem_string(10)
        elif col == 89:
            val = random_lorem_string(10)
        # random any other column
        elif col == 90:
            rndcol = random.randint(0, 10-1)
            val = df.loc[(row-1),:].values[rndcol]
        elif col == 91:
            rndcol = random.randint(10, 20-1)
            val = df.loc[(row-1),:].values[rndcol]
        elif col == 92:
            rndcol = random.randint(20, 30-1)
            val = df.loc[(row-1),:].values[rndcol]
        elif col == 93:
            rndcol = random.randint(30, 40-1)
            val = df.loc[(row-1),:].values[rndcol]
        elif col == 94:
            rndcol = random.randint(40, 50-1)
            val = df.loc[(row-1),:].values[rndcol]
        elif col == 95:
            rndcol = random.randint(50, 60-1)
            val = df.loc[(row-1),:].values[rndcol]
        elif col == 96:
            rndcol = random.randint(60, 70-1)
            val = df.loc[(row-1),:].values[rndcol]
        elif col == 97:
            rndcol = random.randint(70, 80-1)
            val = df.loc[(row-1),:].values[rndcol]
        elif col == 98:
            rndcol = random.randint(80, 90-1)
            val = df.loc[(row-1),:].values[rndcol]
        elif col == 99:
            rndcol = random.randint(0, 90-1)
            val = df.loc[(row-1),:].values[rndcol]
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
    elif 71 <= col <= 89:
        colprefix = "text"
    elif col >= 90:
        colprefix = "random"
    else:
        colprefix = "other"
    colname = ('%s_%02d' % (colprefix, col))
    
    # add column to dataframe
    df[colname] = tmpval

# Generating columns iss ready
print("")
print("ready.")

# Observe the result
print(df)

# csv write new output
df.to_csv(FILE_NAME, sep='\t', na_rep='', header=True, index=False, encoding='utf-8')


