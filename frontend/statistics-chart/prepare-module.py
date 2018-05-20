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

def saveXml(filename):
	tree.write(filename, encoding="utf-8", default_namespace=None, xml_declaration=True)

def addEntryToElementsXml(document, level, uriPath, filename):
	el = ET.Element('File')
	el.set('Path', 'StatisticsChartModule\\' + uriPath.split('/')[1] + '\\' + filename)
	splittedFName = filename.split('.')

	el.set('Url', 'StatisticsChartModule/' + uriPath.split('/')[1] + '/' + splittedFName[0] + '.' + splittedFName[-1])
	root[level].append(el)

def addEntryToSpDataXml(document, level, uriPath, filename, type):
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

def generateElementsXml(path):
	for filename in os.listdir(path):
		s = os.path.join(path, filename)

		if os.path.isdir(s):
			generateElementsXml(s)

		else:
			addEntryToElementsXml(tree, 0, path, filename)

def generateSpDataXml(path, ignoreFiles):	
	for filename in os.listdir(path):
		s = os.path.join(path, filename)
		
		if os.path.isdir(s):
			generateSpDataXml(s, ignoreFiles)

		else:
			if filename in ignoreFiles:
				print("ignoring " + filename)
				continue

			addEntryToSpDataXml(tree, 0, path, filename, 'ElementFile')
		

def removeFiles(path, filesToRemove):
	for filename in glob.iglob(path + '/**'):
		
		if os.path.isdir(filename):
			removeFiles(filename, filesToRemove)
		else:
			for f in filesToRemove:
				if f in filename:
					print("file " + f + " removed")
					os.remove(filename)
					continue

def renameFiles(path, replacements):
	for filename in glob.iglob(path + '/**'):
		s = filename
		
		if os.path.isdir(s):
			renameFiles(s, replacements)

		else:
			for replacement in replacements:
				if replacement[0] in s:
					os.rename(s, s.replace(replacement[0], replacement[1]))
					print("renaming file " + str(filename))


try:
	# renaming for VS, for to be able correctly
	# include files into project and import them in
	# .ascx page
	removeFiles('dist/', ['index.html', 'favicon.ico'])
	renameFiles('dist/', [['~', '']])

	# updating and saving Elements.xml
	loadXmlFile(
		os.path.join('src/templates/', 'Elements.xml'),
		0,
		'http://schemas.microsoft.com/sharepoint/')

	generateElementsXml('dist/')
	saveXml(os.path.join('dist/', 'Elements.xml'))

	# updating and saving SharepointProjectItem.spdata
	loadXmlFile(
		os.path.join('src/templates/', 'SharePointProjectItem.spdata'),
		0,
		'http://schemas.microsoft.com/VisualStudio/2010/SharePointTools/SharePointProjectItemModel')

	addEntryToSpDataXml(tree, 0, '/', 'Elements.xml', 'ElementManifest')

	generateSpDataXml('dist/', ['Elements.xml'])
	
	saveXml(os.path.join('dist/', 'SharePointProjectItem.spdata'))

except Exception as e:
	print('Error' + str(e))
