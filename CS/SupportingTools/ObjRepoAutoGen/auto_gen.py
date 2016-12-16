__author__ = 'tharindal'
import os
import sys
import xlrd
import string

read_csv = os.curdir + "\\" + "element_file.csv"
read_excel = os.curdir + "\\" + "element_file.xlsx"
write_file = os.curdir + "\\" + "element_output.txt"
class_name = 'Public class element_Class \n { \n'
class_packages ='''
                using ArtOfTest.WebAii.Controls.HtmlControls;
                using ArtOfTest.WebAii.Core;
                using ArtOfTest.WebAii.ObjectModel;
                using System;
                using System.Collections.Generic;
                using System.Linq;
                using System.Text;
                using System.Threading.Tasks;\n
                '''
class_constructor = '''
                
                private Manager _manager;


                public constructor(Manager m)
                    {
                        _manager = m;

                    }
            '''
end_bracket = '\n }'

xml_header='<?xml version="1.0" encoding="utf-8" ?> \n '
xml_head_element='<FindParamCollection> \n'
xml_tail_element='</FindParamCollection>'


def write_to_file(msg):
    try:
        file = open(write_file, "a+")
        file.write(msg)
        file.close()

    except Exception as e:
        print(str(e))


def file_cleanup():
    if os.path.exists(write_file):
        os.remove(write_file)


def read_from_file():

    file = open_out_file()
    next(file)
    write_to_file(class_name)
    for line in file:
        list = line.split(",")
        list[-1] = list[-1].strip()
        msg = '''
        public ArtOfTest.WebAii.Controls.HtmlControls.''' + list[0] + " " + list[1] + '''
                {
                    get
                        {
                            return Get<ArtOfTest.WebAii.Controls.HtmlControls.'''+list[0]+">("'"' +list[2]+"=" + list[3] + '"' ''');
                        }
                }

        '''

        write_to_file(msg)
    write_to_file(end_bracket)


def read_from_excel():
    file_cleanup()
    sheet = input("\nIndicate the sheet name of the excel: Press [Enter] if the default is 'Sheet1' : ")
    if sheet is '':
        sheet = "Sheet1"
    workbook = open_excel()
    try:
        sh = workbook.sheet_by_name(sheet)
        list = []
        write_to_file(class_packages)
        write_to_file(class_name)
        write_to_file(class_constructor)
        for row in range(1, sh.nrows):
            for column in range(sh.ncols):
                item = sh.cell_value(row, column)
                list.append(item)

            #print(list)
            msg = '''
            public Element ''' + list[0] + '''
                {get{return _manager.ActiveBrowser.Find.''' + capitalise3rd(list[1],2) + '''("''' + list[2] + '''");}}
            '''
            list = []
            write_to_file(msg)
        write_to_file(end_bracket)
        input("\nOperation successful. Press [Enter] to Exit...")
    except Exception as e:
        print(str(e)+' available. Please retry. (Hint: check case and spelling...)')
        read_from_excel()


def read_from_excel_to_xml():
    file_cleanup()
    sheet = input("\nIndicate the sheet name of the excel: Press [Enter] if it's default 'Sheet1' : ")
    if sheet is '':
        sheet = "Sheet1"
    workbook = open_excel()
    try:
        sh = workbook.sheet_by_name(sheet)
        list = []
        write_to_file(xml_header)
        write_to_file(xml_head_element)
        for row in range(1, sh.nrows):
            for column in range(sh.ncols):
                item = sh.cell_value(row, column)
                list.append(item)
            #print(list)
            #if list[1] != "":
            msg = '''
            <FindParamItem key="''' + list[1] + '''">
                <FindParam TagName="" XPath="" NodeIndexPath="" Type="AttributesOnly" ContentType="TextContent" ContentValue="" TagOccurrenceIndex="-1">
                        <Attributes>
                            <iAttribute Name="''' + list[2] + '''" Value="''' + list[3] + '''" />
                        </Attributes>
                    <PartialAttributes />
                </FindParam>
            </FindParamItem>
            '''
            list = []
            write_to_file(msg)
        write_to_file(xml_tail_element)
        input("\nOperation successful. Press <Enter> to Exit...")
    except Exception as e:
        print(str(e)+' available. Please retry. (Hint: check case and spelling...')
        read_from_excel_to_xml()


def open_out_file():
    try:
        file = open(read_csv, "r")
        return file

    except FileNotFoundError as e:
        print(str(e))
        sys.exit()


def open_excel():
    try:
        workbook = xlrd.open_workbook(read_excel)
        return workbook
    except FileNotFoundError as e:
        print(str(e))
        sys.exit()


def capitalise3rd(s,n):
    return s[:n].capitalize()+s[n:].capitalize()


#read_from_file()

read_from_excel()
#read_from_excel_to_xml()