import shutil
import os
import xml.etree.cElementTree as ET
from xml.dom import minidom
import glob
from os.path import splitext

MODULES_REL_PATH = '../../backend/sharepoint.lanit-task/StatisticsChartModule/'
tree = ''
root = ''


def copytree(src, dst, symlinks=False, ignore=None):
    for item in os.listdir(src):
        s = os.path.join(src, item)
        d = os.path.join(dst, item)

        if(os.path.exists(d)):
            if(os.path.isdir(d)):
                shutil.rmtree(d)
            else:
                os.remove(d)
        if os.path.isdir(s):
            shutil.copytree(s, d, symlinks, ignore)
        else:
            shutil.copy2(s, d)


def loadXmlFile(filename, level, decl):
    ET.register_namespace('', decl)
    global tree
    tree = ET.parse(filename)
    global root
    root = tree.getroot()

    for f in root[level]:
        root[level].remove(f)

    saveXml(filename)


def saveXml(filename):
    tree.write(filename, encoding="utf-8", default_namespace=None, xml_declaration=True)


def addElementsXmlEntry(document, level, uriPath, filename):
    el = ET.Element('File')
    el.set('Path', 'StatisticsChartModule\\' + uriPath.split('/')[1] + '\\' + filename)
    splittedFName = filename.split('.')

    el.set('Url', 'StatisticsChartModule/' + uriPath.split('/')[1] + '/' + splittedFName[0] + '.' + splittedFName[-1])
    root[level].append(el)

def addSpDataXmlEntry(document, level, uriPath, filename, type):
    el = ET.Element('ProjectItemFile')
    source = ''
    target = ''

    if(uriPath.split('/')[1] == ''):
        source = filename
        target = 'StatisticsChartModule\\'

    else:
        source = uriPath.split('/')[1] + '\\' + filename
        target = 'StatisticsChartModule\\' + uriPath.split('/')[1] + '\\'


    el.set('Source', source)
    el.set('Target', target)
    el.set('Type', type)
    root[level].append(el)


def updateElementsXml(path):
    for item in os.listdir(path):
        s = os.path.join(path, item)
        if os.path.isdir(s):
            updateElementsXml(s)
        else:
            addElementsXmlEntry(tree, 0, path, item)

def updateSpDataXml(path):
    for item in os.listdir(path):
        s = os.path.join(path, item)
        if os.path.isdir(s):
            updateSpDataXml(s)
        else:
            addSpDataXmlEntry(tree, 0, path, item, 'ElementFile')

def preprocessFiles(path):
    unusedFiles = ['index.html', 'favicon.ico']
    for filename in glob.iglob(path + '/**'):
        for uf in unusedFiles:
            if uf in filename:
                print("file " + uf + " removed")
                os.remove(filename)
                continue

        s = filename
        if (os.path.isdir(s)):
            preprocessFiles(s)
        else:
            if '~' in s:
                os.rename(s, s.replace('~', ''))
                print("renaming file " + str(filename))


try:
    preprocessFiles('dist/')

    loadXmlFile(
        os.path.join(MODULES_REL_PATH, 'Elements.xml'),
        0,
        'http://schemas.microsoft.com/sharepoint/')

    updateElementsXml('dist/')
    saveXml(os.path.join('module/', 'Elements.xml'))

    loadXmlFile(
        os.path.join(MODULES_REL_PATH, 'SharePointProjectItem.spdata'),
        0,
        'http://schemas.microsoft.com/VisualStudio/2010/SharePointTools/SharePointProjectItemModel')

    updateSpDataXml('dist/')
    addSpDataXmlEntry(tree, 0, '/', 'Elements.xml', 'ElementManifest')

    saveXml(os.path.join('module/', 'SharePointProjectItem2.spdata'))
except Exception as e:
    print('Error' + str(e))
