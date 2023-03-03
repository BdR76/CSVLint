# Color Circle Test
# 
# To find optimal color for syntax highlighting,
# go through color-circle in steps
# to find sequence of most contrasting colors
# 
# Bas de Reuver - bdr1976@gmail.com (sep 2022)

import math
from math import cos, sin, radians
from PIL import Image, ImageDraw, ImageFont

# get circle position from degree, center and radius
def position_from_degree(dgr, x, y, r):
    # x,y = center position, r = circle radius
    xpos = x + (r * math.cos(radians(dgr)))
    ypos = y + (r * -1 * math.sin(radians(dgr)))
    return (xpos, ypos)

# get color from color wheel by degree
def hex2rbg(clr_bgr):

    # order bgr, example orange = 
    strhex = format(clr_bgr, '06x')

    # return in order rgb
    #return ("%s -- %s" % (strhex, strhex[4:6]))
    return strhex[4:6] + strhex[2:4] + strhex[:2]

# get color from color wheel by degree
def color_from_degree(dgr, v_min, v_max):

    # initialise colors
    while dgr > 360:
        dgr -= 360
    while dgr < 0:
        dgr += 360

    # brighter or darker
    maxval = v_max - v_min
        
    # initialise colors
    rd = 0
    gr = 0
    bl = 0
    if 0.0 <= dgr <= 60.0:
        # red -> yellow (green=0..255)
        rd = maxval
        gr = int( (dgr / 60.0) * maxval)
    elif 60.0 <= dgr <= 120.0:
        # yellow -> green (red=255..0)
        rd = int( ( (120 - dgr) / 60.0) * maxval)
        gr = maxval
    elif 120.0 <= dgr <= 180.0:
        # green -> cyan (blue=0..255)
        gr = maxval
        bl = int( ( (dgr - 120) / 60.0) * maxval)
    elif 180.0<= dgr <= 240.0:
        # cyan -> blue (green=255..0)
        gr = int( ( (240 - dgr) / 60.0) * maxval)
        bl = maxval
    elif 240.0 <= dgr <= 300.0:
        # blue -> purple (red=0..255)
        rd = int( ( (dgr - 240) / 60.0) * maxval)
        bl = maxval
    elif 300.0 <= dgr <= 360.0:
        # purple -> red (blue=255..0)
        rd = maxval
        bl = int( ( (360 - dgr) / 60.0) * maxval)

    # incase brighter
    rd = v_min + rd
    gr = v_min + gr
    bl = v_min + bl

    # built color, note order is blue, green, red (not RGB)
    retval = (bl * 256 * 256) +  (gr * 256) + rd

    return retval


# draw color circle
def draw_color_circle(x, y, rad, v_min, v_max):

    # draw color circle
    print("Draw color circle -- v_min=%d v_max=%d" % (v_min, v_max))
    for dgr in range(0, (1 * 360)):
        # determine color and position
        clr = color_from_degree(dgr, v_min, v_max)
        # degrees 0..360 but line ditch=4
        # so color circle has no white gaps
        (xpos, ypos) = position_from_degree(dgr, x, y, rad)
        draw.line( (x, y, xpos, ypos), fill=clr, width=4)

# draw color circle
def draw_text_tester(x, y, clr1, clr2):
    # draw color circle
    draw.rectangle( ( (x, y), ( x + 64-4, y+30)), fill=clr1)
    draw.text((x, y), "test♥☻", font=myfont, fill=clr2, align ="left") # other "filled-in" character are ♥█►☻♥■

def draw_color_sequence(y, nr, dgrstep, xcirc, ycirc, degrlabel):
    # first/second color always blue/yellow
    dgrline = 60 + (2 * dgrstep)
    testwidth = 64
    xoff = 80

    # xml output
    xml_1 = ("<!-- normal background (%d colors) -->\n<WordsStyle styleID=\"0\" name=\"Default\" fgColor=\"000000\" bgColor=\"FFFFFF\" fontName=\"\" fontStyle=\"0\" />\n" % nr)
    xml_2 = ("<!-- normal foreground (%d colors) -->\n<WordsStyle styleID=\"0\" name=\"Default\" fgColor=\"000000\" bgColor=\"FFFFFF\" fontName=\"\" fontStyle=\"0\" />\n" % nr)
    xml_3 = ("<!-- dark mode pstel (%d colors) -->\n<WordsStyle styleID=\"0\" name=\"Default\" fgColor=\"DCDCCC\" bgColor=\"3F3F3F\" fontName=\"\" fontStyle=\"0\" />\n" % nr)
    xml_4 = ("<!-- dark mode neon (%d colors) -->\n<WordsStyle styleID=\"0\" name=\"Default\" fgColor=\"FFFFFF\" bgColor=\"3F3F3F\" fontName=\"\" fontStyle=\"0\" />\n" % nr)

    # draw nr indicator
    draw.text( (8, y+32), ("%d\ncolors\n\nstep\n-%.1f°" % (nr, dgrstep)), font=myfont, fill=0, align ="center")

    # first/second color always blue/yellow
    for i in range(0, nr+2):
        dgrline -= dgrstep
        (xpos, ypos) = position_from_degree(dgrline, xcirc, ycirc, wheelradius)
        if i > 0:
            draw.line( (xprev, yprev, xpos, ypos), fill=0)
        if degrlabel:
            draw.text((xpos, ypos), ("%d°" % (dgrline % 360)), font=myfont, fill=0, align ="center") # other "filled-in" character are ♥█►☻♥■
        #draw.text((i * 32, 640-64), "test", font=font, align ="left")

            
        # get colors
        clr_bright = color_from_degree(dgrline, 128, 255) # normal background
        clr_fore   = color_from_degree(dgrline,  24, 192) # normal foreground
        clr_pastel = color_from_degree(dgrline, 160, 208) # dark mode pastel
        clr_neon_d = color_from_degree(dgrline,   0,  80) # dark mode neon (background)
        clr_neon   = color_from_degree(dgrline, 128, 255) # dark mode neon (foreground)

        # draw degrees on top
        dg = (dgrline % 360)
        draw.text((xoff+(i * testwidth), y+12), ("%.1f°" % dg), font=myfont, fill=0, align ="left")
        
        # draw text examples
        draw_text_tester(xoff+(i * testwidth), y+32, clr_bright,          0) # normal (background)
        draw_text_tester(xoff+(i * testwidth), y+64,   0xffffff,   clr_fore) # normal (foreground)
        draw_text_tester(xoff+(i * testwidth), y+96,   0x3f3f3f, clr_pastel) # dark pastel
        draw_text_tester(xoff+(i * testwidth), y+128, clr_neon_d,   clr_neon) # dark neon
            
        # remember for next
        xprev = xpos
        yprev = ypos
        
        # xml output for output to console
        if i < nr:
            xml_1 += "<WordsStyle styleID=\"%d\" name=\"ColumnColor%d\" fgColor=\"000000\" bgColor=\"%s\" fontName=\"\" fontStyle=\"0\" />\n" % (i+1, i+1, hex2rbg(clr_neon))
            xml_2 += "<WordsStyle styleID=\"%d\" name=\"ColumnColor%d\" fgColor=\"%s\" bgColor=\"%s\" fontName=\"\" fontStyle=\"1\" />\n" % (i+1, i+1, hex2rbg(clr_fore), hex2rbg(0xffffff))
            xml_3 += "<WordsStyle styleID=\"%d\" name=\"ColumnColor%d\" fgColor=\"%s\" bgColor=\"%s\" fontName=\"\" fontStyle=\"1\" />\n" % (i+1, i+1, hex2rbg(clr_pastel), hex2rbg(0x3f3f3f))
            xml_4 += "<WordsStyle styleID=\"%d\" name=\"ColumnColor%d\" fgColor=\"%s\" bgColor=\"%s\" fontName=\"\" fontStyle=\"1\" />\n" % (i+1, i+1, hex2rbg(clr_neon), hex2rbg(clr_neon_d))

    # output xml to console
    print(xml_1)
    print(xml_2)
    print(xml_3)
    print(xml_4)


# new image, new draw object
rectimg = Image.new("RGB", (1280, 960), (255, 255, 255))
draw = ImageDraw.Draw(rectimg)

# go through circle in steps to get a contrasting color sequence
print("Step through color circle")
xprev = 0
yprev = 0
myfont = ImageFont.truetype(r'C:\Windows\Fonts\cour.ttf', 16)
wheelradius = 160 # radius

# color circle parametrs
draw_color_circle(160,     224, wheelradius, 128, 255) # normal background
draw_color_circle(160+320, 224, wheelradius,  24, 192) # normal foreground
draw_color_circle(160+640, 224, wheelradius, 160, 208) # dark mode pastel
draw_color_circle(160+960, 224, wheelradius,   0,  80) # dark mode neon (background)

# draw title
draw.text( (0, 0), "CSV Lint\nBas de Reuver, sep 2022\nDetermine most contrasting color sequences", font=myfont, fill=0, align ="left")

# draw sample texts
draw_color_sequence(400,      8, 135,   160,     224, True) #  8 colors
draw_color_sequence(400+192, 12, 150,   160+320, 224, False) # 12 colors
draw_color_sequence(400+384, 16, 157.5, 160+640, 224, False) # 16 colors (pairs of 2)
#draw_color_sequence(400+384, 16, 112.5, 160+640, 224, False) # 16 colors (pairs of 3)
    
#draw.ellipse((x-r, y-r, x+r, y+r), fill=(255,0,0,0))

# save as test file png
pngname = "generate_colors.png"
rectimg.save(pngname, "PNG")
print("Image saved as %s" % pngname)
